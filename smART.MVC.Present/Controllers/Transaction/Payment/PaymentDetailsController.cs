using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.MVC.Present.Helpers;
using Omu.ValueInjecter;
using smART.Common;
using System.Transactions;

namespace smART.MVC.Present.Controllers.Transaction {

  [Feature(EnumFeatures.Transaction_PaymentDetails)]
  public class PaymentDetailsController : BaseGridController<PaymentReceiptDetailsLibrary, PaymentReceiptDetails> {

    #region /* Constructors */

    public PaymentDetailsController()
      : base("PaymentDetails", new string[] { "Payment" }, new string[] { "Settlement", "PaymentReceipt" }) {
    }

    #endregion

    #region /* Supporting Actions - Display Actions */

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Update(PaymentReceiptDetails data, GridCommand command, bool isNew = false) {
      try {
        if (data.Apply_Amount == data.Balance_Amount)
          data.Paid_In_Full = true;

        ValidateEntity(data);

        if (ModelState.IsValid) {
          if (isNew) {
            //TODO: Add logic to update in memory data
            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
            //UpdateUnPaidExpenses(data);
          }
          else {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
              IsolationLevel = IsolationLevel.ReadCommitted
            })) {
              Library.Modify(data, new string[] { "Settlement", "PaymentReceipt", "ExpenseRequest" });
              scope.Complete();
            }
          }
        }
      }
      catch (Exception ex) {
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
      }

      return Display(command, data.PaymentReceipt.ID.ToString(), isNew);
    }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<PaymentReceiptDetails> resultList;
      PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      ScaleLibrary libScale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      if (isNew || id == "0") {
        resultList = lib.GetAllByPaging(TempEntityList, out totalRows,
                                            command.Page,
                                            command.PageSize,
                                            command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                            command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                            IncludePredicates,
                                            (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                           );
      }
      else {
        resultList = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                        .GetAllByPagingByParentID(out totalRows,
                                                                                    int.Parse(id.ToString()),
                                                                                    command.Page,
                                                                                    command.PageSize == 0 ? 20 : command.PageSize,
                                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                    new string[] { "PaymentReceipt", "Settlement.Scale.Party_ID", "Settlement.Scale.Purchase_Order", "ExpenseRequest.Paid_Party_To", "ExpenseRequest.Scale_Ref", "ExpenseRequest.Dispatcher_Request_Ref.Booking_Ref_No", "ExpenseRequest.Dispatcher_Request_Ref.Container" },
                                                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                  );
        foreach (var item in resultList) {
          if (item.ExpenseRequest != null && item.ExpenseRequest.Dispatcher_Request_Ref != null && item.ExpenseRequest.Dispatcher_Request_Ref.Container != null) {
            item.ExpenseRequest.Scale_Ref = libScale.GetScalesByContainerId(item.ExpenseRequest.Dispatcher_Request_Ref.Container.ID);
          }
        }
       
      }
  
      return View(new GridModel { Data = resultList, Total = totalRows });
    }


    protected override ActionResult Display(GridCommand command, PaymentReceiptDetails entity, bool isNew = false) {
      if (entity.PaymentReceipt != null && entity.PaymentReceipt.ID != 0)
        return Display(command, entity.PaymentReceipt.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }
         
    public ActionResult GetUnPaidTickets(GridCommand command, string partyId, string locationId = "0") {
      TempEntityList.Clear();
      if (Convert.ToInt32(partyId) > 0) {
        string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
        SettlementLibrary settlementLib = new SettlementLibrary();
        settlementLib.Initialize(dbContextConnectionString);
        IEnumerable<Settlement> results = settlementLib.GetUnPaidTickets(new string[] { "Scale", "Scale.Party_ID", "Scale.Purchase_Order", "Scale.Party_Address" }, int.Parse(partyId), int.Parse(locationId));

        if (results != null && results.Count() > 0) {
          PaymentReceiptDetails paymentDetails;
          int id = 0;
          foreach (var item in results) {
            //if (item.Scale != null && item.Scale.Purchase_Order == null) {
            //  item.Scale.Purchase_Order = new PurchaseOrder();
            //}
            id += 1;
            paymentDetails = new PaymentReceiptDetails() {
              ID = id,
              Settlement = item,
              Balance_Amount = item.Amount - item.Amount_Paid_Till_Date,
              PaymentReceipt = new PaymentReceipt()       
            };

            TempEntityList.Add(paymentDetails);
          }
        }

      }
      return Display(command, "0", true);
    }

    public ActionResult GetUnPaidExpenses(GridCommand command, string partyId, string bookingId = "0") {
      TempEntityList.Clear();

      if (Convert.ToInt32(partyId) > 0) {
        string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
        ExpensesRequestLibrary lib = new ExpensesRequestLibrary();
        lib.Initialize(dbContextConnectionString);
        IEnumerable<ExpensesRequest> results = lib.GetUnPaidExpenses(new string[] { "Paid_Party_To", "Scale_Ref", "Dispatcher_Request_Ref.Booking_Ref_No", "Dispatcher_Request_Ref.Container" },
                                                                     int.Parse(partyId), int.Parse(bookingId)
                                                                    );
        if (results != null && results.Count() > 0) {
          PaymentReceiptDetails paymentDetails;
          int id = 0;
          foreach (var item in results) {
            id += 1;
            paymentDetails = new PaymentReceiptDetails() {
              ID = id,
              ExpenseRequest = item,
              Balance_Amount = Convert.ToDecimal(item.Amount_Paid - item.Amount_Paid_Till_Date),
              PaymentReceipt = new PaymentReceipt()

            };
            if (paymentDetails.Settlement!= null )
                paymentDetails.Settlement.Scale = null;
            if (paymentDetails.ExpenseRequest.Dispatcher_Request_Ref != null)
              paymentDetails.ExpenseRequest.Dispatcher_Request_Ref.TruckingCompany = null;
            TempEntityList.Add(paymentDetails);
          }
        }

      }   
      return Display(command,"0", true);
    }


    [GridAction(EnableCustomBinding = true)]
    public ActionResult _BindPaymentDetails(GridCommand command, string id, string partyId, string paymentType, string bookingId = "0", string locationId = "0", bool isNew = false) {
      //if (Session["PaymentDetails"] != null && ((IList<PaymentReceiptDetails>)Session["PaymentDetails"]).Count > 0 && Convert.ToInt32(partyId) == 0 && Convert.ToInt32(id) == 0)
      // For pagging or filter. 
      if (Session["PaymentDetails"] != null && ((IList<PaymentReceiptDetails>)Session["PaymentDetails"]).Count > 0 && isNew == false && Convert.ToInt32(id) == 0)
        return Display(command, id, isNew);
      if (Convert.ToInt32(id) > 0)
        return _Index(command, id, isNew);
      else
        if (paymentType == "Expenses")
          return GetUnPaidExpenses(command, partyId, bookingId);
        else
          return GetUnPaidTickets(command, partyId, locationId);
    }

    [HttpGet]
    public string _GetTotalAppliedAmount(string id) {
      decimal totalApplyAmt = 0;
      int parentId = Convert.ToInt32(id);
      if (parentId > 0) {
        PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<PaymentReceiptDetails> resultList = lib.GetAllByParentID(parentId);
        totalApplyAmt = resultList.Sum(s => s.Apply_Amount);
        return totalApplyAmt.ToString();
      }
      if (Session["PaymentDetails"] != null && ((IList<PaymentReceiptDetails>)Session["PaymentDetails"]).Count > 0) {
        totalApplyAmt = TempEntityList.Sum(s => s.Apply_Amount);
      }
      return totalApplyAmt.ToString();
    }


    [HttpGet]
    public string _GetTotalDueAmount(string id, string paymentType, string bookingId = "0", string locationId = "0") {
      int intPartyId = Convert.ToInt32(id);
      if (intPartyId == 0)
        return "0";
      if (paymentType == "Expenses")
        return GetTotalExpenseDueAmount(intPartyId, int.Parse(bookingId));
      else
        return GetTotalScaleDueAmount(intPartyId, int.Parse(locationId));
    }
    
    private string GetTotalScaleDueAmount(int partyId, int locationId = 0) {
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      SettlementLibrary settlementLib = new SettlementLibrary();
      settlementLib.Initialize(dbContextConnectionString);
      return settlementLib.GetTotalDueAmount(partyId, locationId).ToString();

    }

    private string GetTotalExpenseDueAmount(int partyId, int bookingId = 0) {
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      ExpensesRequestLibrary lib = new ExpensesRequestLibrary();
      lib.Initialize(dbContextConnectionString);
      return lib.GetTotalDueAmount(partyId, bookingId).ToString();
    }


    public  ActionResult _GetQScalePaymentHistory()
    {
        int totalRows = 0;
        ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
        PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<PaymentReceiptDetails> resultList = lib.GetQScalePaymentHistoryWithPaging(out totalRows, 1, ViewBag.PageSize, "", "Asc", new string[] { "PaymentReceipt.Party", "Settlement.Scale" });
        return View("~/Views/Transaction/Payment/_List.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public  ActionResult _GetQScalePaymentHistory(GridCommand command) {
      int totalRows = 0;
      IEnumerable<PaymentReceiptDetails> resultList = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                                        .GetQScalePaymentHistoryWithPaging(                                                    
                                                                                   out totalRows,
                                                                                   command.Page,
                                                                                   10,
                                                                                   "Created_Date",
                                                                                   "Desc" ,
                                                                                   new string[] { "PaymentReceipt.Party", "Settlement.Scale" },
                                                                                   (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                   );        
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    #endregion

    #region Private Methods

    protected override void ValidateEntity(PaymentReceiptDetails entity) {
      if (entity.PaymentReceipt.ID > 0)//&& !IsApplied_AmountZero(entity))
        ModelState.AddModelError("Applied_Amount", "Can not modify payment details."); //columns.Bound(o => o.Settlement.ID).Hidden(true);

      if (entity.Apply_Amount > entity.Balance_Amount)
        ModelState.AddModelError("Balance_Amount", "Eenter amount can not be larger than amount due.");
    }

    //private void UpdateUnPaidExpenses(PaymentReceiptDetails entity) {
    //  PaymentExpenseController expCont = new PaymentExpenseController();
    //  if (entity.Apply_Amount > 0) {
    //    AddUnPaidPurchaseExpenses(entity.Settlement.Scale.ID);
    //  }
    //  else {
    //    RemoveUnPaidPurchaseExpenses(entity.Settlement.Scale.ID);
    //  }
    //}

    //public void AddUnPaidPurchaseExpenses(int scaleId) {
    //  ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //  IEnumerable<ExpensesRequest> expenses = lib.GetAllPurchaseExepneseByScaleId(scaleId, new[] { "Payment","Paid_Party_To"});
    //  if (Session["PaymentExpense"] == null)
    //    Session["PaymentExpense"] = new List<PaymentReceiptExpense>();
    //  IList<ExpensesRequest> expensesTempList = (IList<ExpensesRequest>)Session["PaymentExpense"];
    //  foreach (var item in expenses) {
    //    if (expensesTempList.FirstOrDefault(o => o.Reference_Table == "Scale" && o.Reference_ID == scaleId) == null)
    //      if (item .Paid_Party_To== null)
    //          item.Paid_Party_To= new Party ();
    //      expensesTempList.Add(item);
    //  }
    //}

    //public void RemoveUnPaidPurchaseExpenses(int scaleId) {
    //  if (Session["PaymentExpense"] != null) {
    //    IList<ExpensesRequest> expenses = (IList<ExpensesRequest>)Session["PaymentExpense"];
    //    ExpensesRequest exp = expenses.FirstOrDefault(o => o.Reference_Table == "Scale" && o.Reference_ID == scaleId);
    //    if (exp != null)
    //      expenses.Remove(exp);
    //  }
    //}

    //public bool IsApplied_AmountZero(PaymentReceiptDetails data) {
    //  PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(Configuration.GetsmARTDBContextConnectionString());
    //  IEnumerable<PaymentReceiptDetails> paymentDetails = lib.GetAllByParentID(data.PaymentReceipt.ID, new string[] { "PaymentReceipt" });
    //  PaymentReceiptDetails paymentDetail = paymentDetails.FirstOrDefault();

    //  decimal parentTotalAmountPaid = 0;
    //  if (paymentDetail != null && paymentDetail.PaymentReceipt != null) {
    //    parentTotalAmountPaid = paymentDetails.FirstOrDefault().PaymentReceipt.Total_Amount_Paid;
    //  }
    //  decimal childTotalAmoundPaid = paymentDetails.Where(m => m.ID != data.ID).Sum(p => p.Apply_Amount) + data.Apply_Amount;
    //  return parentTotalAmountPaid == childTotalAmoundPaid;
    //}
    #endregion Private Methods

  }
}