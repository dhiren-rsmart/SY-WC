using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using Omu.ValueInjecter;
using System.Transactions;

namespace smART.MVC.Present.Controllers {

  public abstract class BaseGridController<TLibrary, TEntity> : BaseController<TLibrary, TEntity>
    where TLibrary : ILibrary<TEntity>, new()
    where TEntity : BaseEntity, new() {

    #region /* Local Members */

    protected delegate void delegateActionPerforming(TEntity businessEntity);
    protected event delegateActionPerforming OnAdding, OnModifying, OnDeleting;
    protected event delegateActionPerforming OnAdded, OnModified, OnDeleted;
    protected string[] _includeModifyPredicates;

    #endregion

    #region /* Properties */

    protected string SessionName {
      get;
      set;
    }

    protected string[] IncludePredicates {
      get;
      set;
    }

    protected IList<TEntity> TempEntityList {
      get {
        if (Session[SessionName] == null)
          Session[SessionName] = new List<TEntity>();

        return (IList<TEntity>)Session[SessionName];
      }
      set {
        Session[SessionName] = value;
      }
    }

    #endregion

    #region /* Constructors */

    public BaseGridController(string sessionName, string[] includePredicates = null, string[] includeModifyPredicates = null)
      : base() {
      SessionName = sessionName;
      IncludePredicates = includePredicates;
      _includeModifyPredicates = includeModifyPredicates;
      Initialize();
    }

    public BaseGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null, string[] includeModifyPredicates = null)
      : base(dbContextConnectionString) {
      SessionName = sessionName;
      IncludePredicates = includePredicates;
      _includeModifyPredicates = includeModifyPredicates;
      Initialize();
    }

    #endregion

    #region /* Private Methods */

    private void Initialize() {
      OnAdding += new delegateActionPerforming(ChildGrid_OnAdding);
      OnModifying += new delegateActionPerforming(ChildGrid_OnModifying);
      OnModified += new delegateActionPerforming(ChildGrid_OnModified);
    }

    #endregion

    #region /* Ajax Controller Actions */

    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _Index(GridCommand command, string id, bool isNew = false) {
      return Display(command, id, isNew);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _Update(TEntity data, GridCommand command, bool isNew = false) {
      try {
        // Validate entity object.
        ValidateEntity(data);

        // Model is valid.  
        if (ModelState.IsValid) {

          // Raise OnModifying event.
          if (OnModifying != null)
            OnModifying(data);

          // Modify into Temp List.
          if (isNew) {
            //TODO: Add logic to update in memory data
            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
          }
          else {
            // Using transaction. 
            // Modify into dtabase.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted })) {
              Library.Modify(data, _includeModifyPredicates);
              OnModified(data);
              scope.Complete();
            }           
          }
        }
      }
      catch (Exception ex) {
        // Duplicate exception occured.
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
      }
      return Display(command, data, isNew);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _Insert(TEntity data, GridCommand command, bool isNew = false) {
      try {
        // Validate entity object.  
        ValidateEntity(data);

        // Model is valid.  
        if (ModelState.IsValid) {

          // Raise OnAdding event.
          if (OnAdding != null)
            OnAdding(data);

          // Add into temp list.   
          if (isNew) {
            data.ID = TempEntityList.Count + 1;
            TempEntityList.Add(data);
          }
          // Add into database.
          else {
            // Using transaction.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted })) {
              data = Library.Add(data);
              scope.Complete();
            }
          }
        }
      }
      catch (Exception ex) {
        // Duplicate exception occured.
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
      }
      return Display(command, data, isNew);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
      try {
        if (isNew) {
          //TODO: Delete entity with id
          TEntity entity = TempEntityList.FirstOrDefault(m => m.ID == int.Parse(id));
          TempEntityList.Remove(entity);
        }
        else {
          // Using transaction.
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted })) {
            Library.Delete(id);
            scope.Complete();
          }
        }
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      if (string.IsNullOrEmpty(MasterID))
        return Display(command, isNew);
      else
        return Display(command, MasterID, isNew);
    }

    #endregion

    #region /* Display Functions */

    protected virtual ActionResult Display(GridCommand command, bool isNew = false) {
      int totalRows = 0;
      IEnumerable<TEntity> resultList = null;

      if (isNew) {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = Library.GetAllByPaging(out totalRows,
                                           command.Page,
                                           command.PageSize,
                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                           IncludePredicates,
                                           (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                          );
      }
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    protected virtual ActionResult Display(GridCommand command, string id, bool isNew = false) {
      return Display(command, isNew);
    }

    protected virtual ActionResult Display(GridCommand command, TEntity entity, bool isNew = false) {
      return Display(command, isNew);
    }

    protected virtual void ValidateEntity(TEntity entity) { }

    protected virtual void ChildGrid_OnAdding(TEntity entity) { }

    protected virtual void ChildGrid_OnModifying(TEntity entity) { }

    protected virtual void ChildGrid_OnModified(TEntity entity) { }

    #endregion

  }
}
