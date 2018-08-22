using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Helpers;
using System.Transactions;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  public abstract class BaseFormController<TLibrary, TEntity> : BaseController<TLibrary, TEntity>
    where TLibrary : ILibrary<TEntity>, new()
    where TEntity : BaseEntity, new() {
    protected string[] ChildEntityList {
      get;
      set;
    }
    protected string _listViewUrl;
    protected string[] _includeEntities;
    protected string[] _includeModifyPredicates;

    #region /* Constructors */

    public BaseFormController(string listViewUrl, string[] includeEntities, string[] childEntityList = null, string[] includeModifyPredicates = null)
      : base() {
      ChildEntityList = childEntityList;
      _listViewUrl = listViewUrl;
      _includeEntities = includeEntities;
      _includeModifyPredicates = includeModifyPredicates;
    }

    public BaseFormController(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    #endregion

    #region /* Accessible Actions */
    public override ActionResult Index(int? id) {
      if (id.HasValue) {
        TEntity entity = Library.GetByID(id.ToString(), _includeEntities);
        return Display(entity);
      }
      else
        return RedirectToAction("New");
    }

    public virtual ActionResult New(string id) {
      // Create new object
      ClearChildEntities(ChildEntityList);
      TEntity entity = new TEntity();

      return Display(entity);
    }

    [HttpPost]
    public virtual ActionResult Save(TEntity entity) {
      // For new entity
      bool isNew = entity.ID == 0;
      try {
        // Validate Entity.
        ValidateEntity(entity);

        // Check entity is valid  
        if (ModelState.IsValid) {

          // Start transaction.
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            if (entity.ID == 0) {

              // Add parent entity.
              entity = Library.Add(entity);

              // Also save all relevant child records in database
              if (ChildEntityList != null) {
                SaveChildEntities(ChildEntityList, entity);
                ClearChildEntities(ChildEntityList);
              }

              // Execute On_Added
              Form_OnAdded(entity);
            }
            else {
              // Modify entity.
              Library.Modify(entity, _includeModifyPredicates);

              // Execute On_Modified 
              Form_OnModified(entity);
            }

            // Complete transaction.
            scope.Complete();
          }
          ModelState.Clear();
        }
        else
          return Display(entity);

        return Display(entity.ID.ToString());
      }
      catch (Exception ex) {
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
        if (isNew)
          entity.ID = 0;
        return Display(entity);
      }
    }

    #endregion

    #region /* Ajax Methods */
    [HttpGet]
    public virtual ActionResult _Index() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<TEntity> resultList = ((ILibrary<TEntity>)Library).GetAllByPaging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities);
      return View(_listViewUrl, resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _Index(GridCommand command) {
      int totalRows = 0;

      FilterDescriptor filterDesc = new FilterDescriptor("Active_Ind", FilterOperator.IsNotEqualTo, "false");
      command.FilterDescriptors.Add(filterDesc);
      //IEnumerable<TEntity> resultList = ((ILibrary<TEntity>)Library).GetAllByPaging(out totalRows, command.Page, (command.PageSize == 0 ? Configuration.GetsmARTLookupGridPageSize() : command.PageSize), "", "Asc",_includeEntities, (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      IEnumerable<TEntity> resultList = ((ILibrary<TEntity>)Library).GetAllByPaging(
                                                      out totalRows,
                                                      command.Page,
                                                      command.PageSize,
                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                      _includeEntities,
                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

      return View(new GridModel {Data = resultList,Total = totalRows});
    }


    [HttpPost]
    public virtual ActionResult _Delete(string id) {
      try {
        if (!string.IsNullOrWhiteSpace(id)) {
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            DeleteChildEntities(ChildEntityList, id);
            Library.Delete(id);          
            scope.Complete();
          }
        }
        return RedirectToAction("New");
      }
      catch (Exception ex) {
        TEntity entity = Library.GetByID(id);
        ModelState.AddModelError("Error", ex.Message);
        return View(entity);
      }


    }

    #endregion

    #region /* Supporting Actions - Non accessible for public */

    protected virtual ActionResult Display(string id) {
      TEntity result = ((ILibrary<TEntity>)Library).GetByID(id, _includeEntities);

      return View("New", result);
    }

    protected virtual ActionResult Display(TEntity entity) {
      return View("New", entity);
    }

    protected virtual void SaveChildEntities(string[] childEntityList, TEntity entity) {
    }

    protected virtual void ClearChildEntities(string[] childEntityList) {
      if (childEntityList != null)
        foreach (string childEntity in childEntityList)
          Session.Remove(childEntity);
    }

    protected virtual void DeleteChildEntities(string[] childEntityList, string parentID) {
    }

    protected virtual void ValidateEntity(TEntity entity) {
    }


    protected virtual void Form_OnModified(TEntity entity) {
    }

    protected virtual void Form_OnAdded(TEntity entity) {
    }

    public T ModelFromActionResult<T>(ActionResult actionResult) {
      object model;
      if (actionResult.GetType() == typeof(ViewResult)) {
        ViewResult viewResult = (ViewResult) actionResult;
        model = viewResult.Model;
      }
      else if (actionResult.GetType() == typeof(PartialViewResult)) {
        PartialViewResult partialViewResult = (PartialViewResult) actionResult;
        model = partialViewResult.Model;
      }
      else {
        throw new smART.Common.InvalidOperationException(string.Format("Actionresult of type {0} is not supported by ModelFromResult extractor.", actionResult.GetType()));
      }
      T typedModel = (T) model;
      return typedModel;
    }
    #endregion
  }

}
