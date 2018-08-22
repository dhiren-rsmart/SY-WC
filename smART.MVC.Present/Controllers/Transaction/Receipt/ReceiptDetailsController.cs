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
  [Feature(EnumFeatures.Transaction_ReceiptDetails)]
  public class ReceiptDetailsController : BaseGridController<PaymentReceiptDetailsLibrary, PaymentReceiptDetails> {

    #region /* Constructors */

    public ReceiptDetailsController()
      : base("ReceiptDetails", new string[] { "Receipt" }, new string[] { "Invoice.Booking.Sales_Order_No.Party", "Invoice.Sales_Order_No.Party", "PaymentReceipt" }) {
    }

    #endregion

    #region /* Supporting Actions - Display Actions */

  
    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(PaymentReceiptDetails data, GridCommand command, bool isNew = false)
    //{
    //    ModelState.Clear();


    //    if (ModelState.IsValid)
    //    {
    //        if (isNew)
    //        {
    //            TempEntityList.Add(data);
    //        }
    //        else
    //        {
    //            data = Library.Add(data);
    //        }
    //    }


    //    return Display(command, data, isNew);
    //}

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Update(PaymentReceiptDetails data, GridCommand command, bool isNew = false) {
      try {
        if (!isNew && !IsApplied_AmountZero(data))
          ModelState.AddModelError("Applied_Amount", "Receipt details amount mismetch to total amount.");

        if (ModelState.IsValid) {
          if (isNew) {
            //TODO: Add logic to update in memory data
            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
          }
          else {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
              IsolationLevel = IsolationLevel.ReadCommitted
            })) {
              Library.Modify(data, new string[] { "Invoice", "PaymentReceipt" });
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



    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public  ActionResult _Update(GridCommand command, bool isNew = false)
    //{
    //    PaymentReceiptDetails data = null;
    //    if (!isNew && !IsApplied_AmountZero(data))
    //        ModelState.AddModelError("Applied_Amount", "Receipt details amount mismetch to total amount.");

    //    if (ModelState.IsValid)
    //    {
    //        if (isNew)
    //        {
    //            //TODO: Add logic to update in memory data
    //            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //        }
    //        else
    //        {
    //            Library.Modify(data, new string[] { "Invoice" });
    //        }
    //    }
    //    return Display(command, data.ID.ToString(), isNew);
    //}


    public bool IsApplied_AmountZero(PaymentReceiptDetails data) {
      PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<PaymentReceiptDetails> paymentDetails = lib.GetAllByParentID(data.PaymentReceipt.ID, new string[] { "PaymentReceipt" });
      PaymentReceiptDetails receiptDetails = paymentDetails.FirstOrDefault();
      decimal parentTotalAmountPaid = 0;
      if (receiptDetails != null && receiptDetails.PaymentReceipt != null) {
        parentTotalAmountPaid = receiptDetails.PaymentReceipt.Total_Amount_Paid;
      }

      decimal childTotalAmoundPaid = paymentDetails.Where(m => m.ID != data.ID).Sum(p => p.Apply_Amount) + data.Apply_Amount;
      return parentTotalAmountPaid == childTotalAmoundPaid;
    }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<PaymentReceiptDetails> resultList;

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<PaymentReceiptDetails>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Invoice.Booking.Sales_Order_No.Party","PaymentReceipt"});
      }

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, PaymentReceiptDetails entity, bool isNew = false) {
      if (entity.PaymentReceipt != null && entity.PaymentReceipt.ID != 0)
        return Display(command, entity.PaymentReceipt.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }


    public ActionResult GetUnPaidInvoices(GridCommand command, string partyId) {
      int totalRows = 0;
      TempEntityList.Clear();
      if (Convert.ToInt32(partyId) > 0) {
        string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
        InvoiceLibrary lib = new InvoiceLibrary();
        lib.Initialize(dbContextConnectionString);
        IEnumerable<Invoice> results = lib.GetUnPaidInvoicesWithPaging(out totalRows,
                                                                         command.Page, command.PageSize == 0 ? 20 : command.PageSize,
                                                                          "", "Asc",
                                                                          new string[] { "Booking.Sales_Order_No.Party", "Sales_Order_No.Party" },
                                                                          null,
                                                                          int.Parse(partyId)
                                                                         );

        if (results != null && results.Count() > 0) {
          PaymentReceiptDetails paymentDetails;
          int id = 0;
          foreach (var item in results) {
            id += 1;
            if (item .Booking == null ) item .Booking = new Booking();
            paymentDetails = new PaymentReceiptDetails() {
              ID = id,
              Invoice = item,              
              Balance_Amount = item.Net_Amt - item.Amount_Paid_Till_Date,
              PaymentReceipt = new PaymentReceipt()
            };

            TempEntityList.Add(paymentDetails);
          }
        }

      }

      IEnumerable<PaymentReceiptDetails> resultList = TempEntityList;     
      return View(new GridModel {Data = resultList,Total = totalRows});

    }

    [GridAction(EnableCustomBinding = true)]
    public ActionResult _BindReceiptDetails(GridCommand command, string id, string partyId, bool isNew = false) {
      if (Session["ReceiptDetails"] != null && ((IList<PaymentReceiptDetails>)Session["ReceiptDetails"]).Count > 0 && Convert.ToInt32(partyId) == 0 && Convert.ToInt32(id) == 0)
        return Display(command, id, isNew);
      else if (Convert.ToInt32(id) > 0)
        return _Index(command, id, isNew);
      else
        return GetUnPaidInvoices(command, partyId);
    }

    [HttpGet]
    public string _GetTotalDueAmount(string id) {
      int intPartyId = Convert.ToInt32(id);
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      InvoiceLibrary lib = new InvoiceLibrary();
      lib.Initialize(dbContextConnectionString);
      return lib.GetTotalDueAmount(intPartyId).ToString();
    }

    #endregion

  }
}