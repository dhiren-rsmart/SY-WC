using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Helpers;
using smART.Common;


namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Administration_QBLog)]
  public class QBLogController : BaseFormController<QuickBookLibrary, QBLog> {

    #region /* Constructors */

    public QBLogController()
      : base("", null) {
    }

    #endregion

    #region Override Methods

    [HttpGet]
    public ActionResult Index() {
      QBLog log = new QBLog();
      return View("~/Views/Administration/QBLog/New.cshtml", log);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _ParentIndex(GridCommand command) {
      return Display(command);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _ChildIndex(GridCommand command, string id) {
      return Display(command, Convert.ToInt32(id));
    }

    [GridAction(EnableCustomBinding = true)]
    public ActionResult Display(GridCommand command) {
      int totalRows = 0;

      //Get all parent logs.
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();

      QuickBookLibrary lib = new smART.Library.QuickBookLibrary();
      lib.Initialize(dbContextConnectionString);

      IEnumerable<QBLog> resultList = lib.GetPendingParentQBLogsByStatusWithPagging("Failed",
                                                                              out totalRows,
                                                                              command.Page,
                                                                              command.PageSize,
                                                                              "ID",
                                                                              "Asc",
                                                                              null,
                                                                              (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                            );

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [GridAction(EnableCustomBinding = true)]
    public ActionResult Display(GridCommand command, int id) {
      int totalRows = 0;

      //Get all parent logs.
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();

      QuickBookLibrary lib = new smART.Library.QuickBookLibrary();
      lib.Initialize(dbContextConnectionString);

      IEnumerable<QBLog> resultList = lib.GetByPrentIDWithPagging(id,
                                                                  out totalRows,
                                                                  command.Page,
                                                                  command.PageSize,
                                                                  "ID",
                                                                  "Asc",
                                                                  null,
                                                                  (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                 );

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _UpdateParent(QBLog data, GridCommand command) {
      _Update(data);
      return Display(command);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _UpdateChild(QBLog data, GridCommand command) {
      _Update(data, false);
      return Display(command, data.Parent_ID);
    }

    public void _Update(QBLog data, bool isParent = true) {
      try {
        ValidateEntity(data);

        if (ModelState.IsValid) {
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            if (isParent)
              // Update Parent.
              Library.Modify(data);

            // Update Details.
            UpdateDetails(data, isParent);

            //Commit Transaction.
            scope.Complete();
          }
        }
      }
      catch (Exception ex) {
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
      }
    }

    private void UpdateDetails(QBLog parent, bool isParent = true) {
      //Get all parent logs.
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      QuickBookLibrary lib = new smART.Library.QuickBookLibrary(dbContextConnectionString);
      IEnumerable<QBLog> childList = lib.GetByParentID(isParent ? parent.ID : parent.Parent_ID);
      foreach (var item in childList) {
        if (isParent) {
          item.Status = parent.Status;
          item.Name = parent.Name;
        }
        else {
          item.Account_Name = parent.Account_Name;
        }
        lib.Modify(item);
      }
    }

    #endregion Override Methods

  }
}

