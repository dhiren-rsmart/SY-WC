using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;
using smART.MVC.Present.Helpers;
using System.Transactions;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Transaction_ExpensesRequest)]
  public class GenericExpensesRequestController : BaseFormController<ExpensesRequestLibrary, ExpensesRequest> {

    #region Constructor

    public GenericExpensesRequestController()
      : base(ConfigurationHelper.GetsmARTDBContextConnectionString()) {
    }

    #endregion Constructor

    #region Override Methods

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Index(GridCommand command) {
      return Display(command);
    }

   [GridAction(EnableCustomBinding = true)]
    public ActionResult Display(GridCommand command) {
      int totalRows = 0;
      Dictionary<string, string> filters = new Dictionary<string, string>();
      filters.Add("Dispatcher_Request_Ref.Booking_No", "Dispatcher_Request_Ref.Booking_Ref_No.Booking_Ref_No");
      filters.Add("Scale_Ref.Party_Name", "Party_ID.Party_Name");
      filters.Add("Dispatcher_Request_Ref.Container", "Dispatcher_Request_Ref.Container.Container_No");

      ApplyFilterDescriptor(command, filters);

      ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<ExpensesRequest> resultList = lib.GetAllUnApprovedExpensesWithPagging(
                                                           out totalRows,
                                                           command.Page,
                                                           command.PageSize,
                                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                           new string[] { "Paid_Party_To", "Payment", "Invoice", "Scale_Ref", "Dispatcher_Request_Ref.Booking_Ref_No", "Dispatcher_Request_Ref.TruckingCompany", "Dispatcher_Request_Ref.Container" },
                                                           (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
          
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _Update(ExpensesRequest data, GridCommand command, bool isNew = false) {
      try {
        // Validate entity object.
        ValidateEntity(data);

        // Model is valid.  
        if (ModelState.IsValid) {
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            Library.Modify(data, new string[] { "Paid_Party_To", "Payment", "Invoice" });
            scope.Complete();
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
      return Display(command);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _Insert(ExpensesRequest data, GridCommand command, bool isNew = false) {
      try {
        // Validate entity object.  
        ValidateEntity(data);

        // Model is valid.  
        if (ModelState.IsValid) {
          // Using transaction.
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            data = Library.Add(data);
            data.Reference_ID = data.ID;
            Library.Modify(data, new string[] { "Paid_Party_To", "Payment", "Invoice" });
            scope.Complete();
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
      return Display(command);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _DeleteExpense(ExpensesRequest data, GridCommand command, bool isNew = false) {
      try {
        // Using transaction.
        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
          IsolationLevel = IsolationLevel.ReadCommitted
        })) {
          Library.Delete(data.ID.ToString());
          scope.Complete();
        }
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      return Display(command);
    }

    protected override void ValidateEntity(ExpensesRequest entity) {
      ModelState.Clear();

      if (string.IsNullOrWhiteSpace(entity.EXPENSE_TYPE)) {
        ModelState.AddModelError("EXPENSE_TYPE", "Expense type is required.");
      }

      if (string.IsNullOrWhiteSpace(entity.Paid_By)) {
        ModelState.AddModelError("Paid_By", "Paid By is required.");
      }

      if (entity.Amount_Paid == 0) {
        ModelState.AddModelError("Amount_Paid", "Approved amount is required.");
      }

      // Validate Party.    
      if (entity.Paid_Party_To.ID == 0)
        ModelState.AddModelError("Party", "Party is required");
    }


    public ActionResult _SettledExpenses() {
      int totalRows = 0;

      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<ExpensesRequest> resultList = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllUnApprovedExpensesWithPagging(
                                                                                      out totalRows,
                                                                                      1,
                                                                                      ViewBag.PageSize,
                                                                                      "",
                                                                                      "Asc",
                                                                                       new string[] { "Paid_Party_To", "Payment", "Invoice", "Scale_Ref", "Dispatcher_Request_Ref.Booking_Ref_No", "Dispatcher_Request_Ref.TruckingCompany", "Dispatcher_Request_Ref.Container" }
                                                                                      );
      return View("~/Views/Transaction/GenericExpensesRequest/SettledExpenses.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _SettledExpenses(GridCommand command) {
      int totalRows = 0;
      foreach (FilterDescriptor filterDesc in command.FilterDescriptors) {
        if (filterDesc.Member == "Dispatcher_Request_Ref.Booking_No")
          filterDesc.Member = "Dispatcher_Request_Ref.Booking_Ref_No.Booking_Ref_No";
        else if (filterDesc.Member == "Scale_Ref.Party_Name")
          filterDesc.Member = "Party_ID.Party_Name";
        else if (filterDesc.Member == "Dispatcher_Request_Ref.Container")
          filterDesc.Member = "Dispatcher_Request_Ref.Container.Container_No";

      }
      IEnumerable<ExpensesRequest> resultList = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetUnPaidExpensesWithPaging(
                                                                                                          out totalRows,
                                                                                                          command.Page,
                                                                                                          command.PageSize,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                          new string[] { "Paid_Party_To", "Payment", "Invoice", "Scale_Ref", "Dispatcher_Request_Ref.Booking_Ref_No", "Dispatcher_Request_Ref.TruckingCompany", "Dispatcher_Request_Ref.Container" },
                                                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                          );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }


    public ActionResult Undo(int? expenseId) {
      if (expenseId.HasValue) {
        try {
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            ExpensesRequest exp = Library.GetByID(expenseId.ToString(), new string[] { "Paid_Party_To", "Payment", "Invoice" });
            if (exp != null && exp.Amount_Paid_Till_Date <= 0) {
              exp.Expense_Status = " ";
              Library.Modify(exp, new string[] { "Paid_Party_To", "Payment", "Invoice" });
              scope.Complete();
            }
          }
        }
        catch (Exception ex) {
          ModelState.AddModelError("Error", ex.Message);
        }
      }
      return RedirectToAction("New", "Payment");
    }

    //public void UpdateRefObject(ExpensesRequest expenseRequest) {
    //  switch (expenseRequest.Reference_Table) {
    //    case "DispatcherRequest":
    //      DispatcherRequestLibrary dispLib = new DispatcherRequestLibrary(Configuration.GetsmARTDBContextConnectionString());
    //      DispatcherRequest dispatcherReq = dispLib.GetByID(expenseRequest.Reference_ID.ToString(),new string[] { "TruckingCompany","Booking_Ref_No"});
    //      if (dispatcherReq.Booking_Ref_No == null ) dispatcherReq .Booking_Ref_No = new Booking ();
    //      expenseRequest.DispatcherRequest = dispatcherReq;
    //      break;
    //    case "Scale":
    //      ScaleLibrary scaleLib = new ScaleLibrary(Configuration.GetsmARTDBContextConnectionString());
    //      Scale scale = scaleLib.GetByID(expenseRequest.Reference_ID.ToString(),new string[]{"Party_ID"});
    //      expenseRequest.Scale = scale;
    //      break;
    //    default:
    //      break;
    //  }
    //}

    #endregion

  }
}
