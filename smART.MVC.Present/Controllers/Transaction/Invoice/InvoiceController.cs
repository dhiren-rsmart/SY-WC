using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Invoice)]
  public class InvoiceController : BaseFormController<InvoiceLibrary, Invoice> {
    #region Local Members

    private string[] _predicates = { "Booking.Sales_Order_No.Party", "Sales_Order_No.Party" };

    #endregion Local Members

    #region Constructor

    public InvoiceController()
      : base
        (
        "~/Views/Transaction/Invoice/_List.cshtml",
        new string[] { "Booking.Sales_Order_No.Party", "Sales_Order_No.Party" },
        new string[] { "InvoiceItem", "InvoiceExpense", "InvoiceNotes", "InvoiceAttachments" },
       new string[] { "Booking", "Sales_Order_No" }

        ) {
    }

    #endregion Constructor

    #region Override Methods

    protected override void ValidateEntity(Invoice entity) {
      ModelState.Clear();

      if (entity.ID == 0 && new string[] { "exports", "brokerage" }.Any(s => s == entity.Invoice_Type.ToLower()) && entity.Booking.ID == 0)
        ModelState.AddModelError("Booking", "Booking is required");
      if (entity.ID == 0 && new string[] { "local sales", "trading" }.Any(s => s == entity.Invoice_Type.ToLower()) && entity.Sales_Order_No.ID == 0)
        ModelState.AddModelError("SalesOrder", "Sales Order is required");
      if (entity.Net_Amt <= 0)
        ModelState.AddModelError("Net_Amt", "Net invoice amount is required.");
      if (new string[] { "brokerage" }.Any(s => s == entity.Invoice_Type.ToLower()) && entity.Booking != null && entity.Booking.ID > 0) {
        ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        Scale scale = scaleLib.GetScaleByBookingId(entity.Booking.ID);
        if (scale == null)
          ModelState.AddModelError("Brokerage1", "To generate a Invoice, valid scale ticket has to be created and closed.");
        else if (scale != null) {
          ContainerLibrary contLib = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          IEnumerable<Container> containers = contLib.GetAllByParentID(entity.Booking.ID, new string[] { "Booking" });
          decimal totalContNetWeight = containers.Sum(s => s.Net_Weight);
          if (totalContNetWeight != scale.Net_Weight || scale.Ticket_Status != "Close")
            ModelState.AddModelError("Booking2", "Net weight on scale ticket should match with total net weight of all containers for this invoice.");
        }
      }
    }

    //[HttpPost]
    //public override ActionResult Save(Invoice entity)
    //{
    //    ModelState.Clear();

    //    // Need to find an easier way

    //    if (entity.ID == 0 && entity.Booking.ID == 0)
    //        ModelState.AddModelError("Booking", "Booking is required");
    //    if (entity.Net_Amt<= 0)
    //        ModelState.AddModelError("Net_Amt", "Net Amount is required");

    //    if (ModelState.IsValid)
    //    {
    //        if (entity.ID == 0)
    //        {
    //            entity = Library.Add(entity);

    //            // Also save all relevant child records in database
    //            if (ChildEntityList != null)
    //            {
    //                SaveChildEntities(ChildEntityList, entity);
    //                ClearChildEntities(ChildEntityList);
    //            }
    //        }
    //        else
    //        {
    //            Library.Modify(entity);
    //        }

    //        ModelState.Clear();
    //    }
    //    else
    //        return Display(entity);

    //    return Display(entity.ID.ToString());
    //}

    protected override void SaveChildEntities(string[] childEntityList, Invoice entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "InvoiceExpense":
            if (Session[ChildEntity] != null) {
              InvoiceExpenseLibrary InvoiceExpenseLibrary = new InvoiceExpenseLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ExpensesRequest> resultList = (IList<ExpensesRequest>) Session[ChildEntity];
              foreach (ExpensesRequest InvoiceExpense in resultList) {
                if (InvoiceExpense.Reference_Table == new Model.Scale().GetType().Name || InvoiceExpense.Reference_Table == new Model.DispatcherRequest().GetType().Name) {
                  InvoiceExpense.Payment = new PaymentReceipt {
                    ID = 0
                  };
                  InvoiceExpense.Invoice = new Invoice {
                    ID = entity.ID
                  };
                  InvoiceExpenseLibrary.Modify(InvoiceExpense, new string[] { "Paid_Party_To", "Payment", "Invoice" });
                }
                else {
                  InvoiceExpense.Reference = new Invoice {
                    ID = entity.ID
                  };
                  InvoiceExpense.Reference_Table = entity.GetType().Name;
                  InvoiceExpense.Reference_ID = entity.ID;
                  InvoiceExpense.Invoice = new Invoice {
                    ID = entity.ID
                  };
                  InvoiceExpenseLibrary.Add(InvoiceExpense);
                }
              }
            }
            break;
          case "InvoiceNotes":
            if (Session[ChildEntity] != null) {
              InvoiceNotesLibrary InvoiceNotesLibrary = new InvoiceNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<InvoiceNotes> resultList = (IList<InvoiceNotes>) Session[ChildEntity];
              foreach (InvoiceNotes InvoiceNote in resultList) {
                InvoiceNote.Parent = new Invoice {
                  ID = entity.ID
                };
                InvoiceNotesLibrary.Add(InvoiceNote);
              }
            }
            break;

          case "InvoiceAttachments":
            if (Session[ChildEntity] != null) {
              InvoiceAttachmentsLibrary InvoiceLibrary = new InvoiceAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<InvoiceAttachments> resultList = (IList<InvoiceAttachments>) Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (InvoiceAttachments Invoice in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(Invoice.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Invoice.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(Invoice.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Invoice.Document_RefId.ToString());
                Invoice.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(Invoice.Document_Name, sourcePath, destinationPath);

                Invoice.Parent = new Invoice {
                  ID = entity.ID
                };
                InvoiceLibrary.Add(Invoice);
              }
            }
            break;

          #endregion
        }
      }
    }

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      Invoice entity = Library.GetByID(id.ToString(), new string[] { "Party", "Contact" });

      return Json(entity);
    }


    protected override void Form_OnModified(Invoice entity) {
      AddInQB(entity);
    }

    protected override void Form_OnAdded(Invoice entity) {
      AddInQB(entity);
    }

    private void AddInQB(Invoice entity) {
      if (entity.ID > 0 && entity.Invoice_Status == "Closed" && entity.Invoice_Type == "Exports" && entity.QB == false) {
        entity.QB = true;
        Library.Modify(entity);
        //InvoiceItemLibrary invOps = new InvoiceItemLibrary(Configuration.GetsmARTDBContextConnectionString());
        //decimal totalAvgCostAmt = invOps.GetTotalAvgCostAmt(entity.Booking.ID, new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" });

        //QuickBookLibrary qbLibrary = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        //qbLibrary.AddInvoiceIntoQBLog(entity.ID,totalAvgCostAmt);
      }
    }

    #endregion Override Methods

    #region Public Methods

    public ActionResult _UnPaidInvoices() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<Invoice> resultList = new InvoiceLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetUnPaidInvoicesWithPaging(
                                                                                                                              out totalRows,
                                                                                                                              1,
                                                                                                                              ViewBag.PageSize,
                                                                                                                              "",
                                                                                                                              "Asc",
                                                                                                                              _predicates,
                                                                                                                              null
                                                                                                                              );
      return View("~/Views/Transaction/Invoice/_UnPaidInvoices.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _UnPaidInvoices(GridCommand command) {
      int totalRows = 0;
      IEnumerable<Invoice> resultList = new InvoiceLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                                      .GetUnPaidInvoicesWithPaging(out totalRows,
                                                                                                   command.Page,
                                                                                                   command.PageSize,
                                                                                                   command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                   command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                   _predicates,
                                                                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                   );
      return View(new GridModel {
        Data = resultList, Total = totalRows
      });
    }

    protected override ActionResult Display(Invoice entity) {
      if (entity.ID > 0) {
        decimal total = 0;
        switch (entity.Invoice_Type) {
          case "Brokerage":
            InvoiceBrokerageLibrary ibLib = new InvoiceBrokerageLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            total = ibLib.GetTotal(entity.Booking.ID, new string[] { "Booking.Sales_Order_No.Party" });
            break;
          case "Exports":
            InvoiceItemLibrary iiLib = new InvoiceItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            total = iiLib.GetTotal(entity.Booking.ID, new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" });
            break;
          case "Trading":
          case "Local Sales":
            InvoiceLocalSalesLibrary ilLib = new InvoiceLocalSalesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            total = ilLib.GetTotal(entity.Sales_Order_No.ID, new string[] { "Scale.Sales_Order.Party", "Item_Received", "Apply_To_Item" });
            break;
        }
        if (total != entity.Total_Amt) {
          decimal netAmt = total + entity.Expences_Amt + entity.Advance_Amt - entity.Discount + entity.Tax_Amt;
          entity.Total_Amt = total;
          entity.Net_Amt = netAmt;
          Library.Modify(entity);
        }
      }
      return base.Display(entity);

    }

    #endregion  Public Methods

  }
}
