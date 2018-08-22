using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using Omu.ValueInjecter;
using smART.Common;
using System.Transactions;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_SalesOrderItem)]
  public class SalesOrderItemController : SalesOrderChildGridController<SalesOrderItemLibrary, SalesOrderItem> {
    public SalesOrderItemController()
      : base("SalesOrderItem", new string[] { "SalesOrder", "Item" }, new string[] { "Item"}) {
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(SalesOrderItem data, GridCommand command, bool isNew = false)
    //{
    //    ModelState.Clear();
    //    if (data.Item == null || data.Item.ID==0)
    //    {
    //        ModelState.AddModelError("Item", "Item is required");
    //    }
    //    else
    //    {
    //        if (data.Item_Override.ID == 0)
    //        {
    //            data.Item_Override = data.Item;
    //        }
    //    }

    //    if (data.Item_Override == null || data.Item_Override.ID == 0)
    //    {
    //        ModelState.AddModelError("Item", "Item override is required");
    //    }

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

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Update(SalesOrderItem data, GridCommand command, bool isNew = false) {
    //  try {
    //    // Validate entity.
    //    ValidateEntity(data);

    //    // Model is valid.  
    //    if (ModelState.IsValid) {

    //      // Modify into Temp List.
    //      if (isNew) {
    //        //TODO: Add logic to update in memory data
    //        TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //      }
    //      else {
    //        // Modify into dtabase.
    //        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
    //          IsolationLevel = IsolationLevel.ReadCommitted
    //        })) {
    //          Library.Modify(data, _includeModifyPredicates);

    //          // Update invoice if invoice is generated.
    //          InvoiceLibrary lib = new InvoiceLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //          Invoice invoice = lib.GetInvoiceBySalesOrder(data.SalesOrder.ID);
    //          if (invoice != null) {
    //            decimal amt = new InvoiceItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetTotal(invoice.Booking.ID);
    //            if (invoice.Total_Amt != amt) {
    //              invoice.Total_Amt = amt;
    //              lib.Modify(invoice, new string[] { "Booking" });
    //            }
    //          }
    //          scope.Complete();
    //        }
    //      }
    //    }
    //  }
    //  catch (Exception ex) {
    //    if (ex.GetBaseException() is smART.Common.DuplicateException)
    //      ModelState.AddModelError("Error", ex.GetBaseException().Message);
    //  }
    //  return Display(command, data, isNew);
    //}


    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<SalesOrderItem> resultList;    // = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((ISalesOrderChildLibrary<SalesOrderItem>) Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
        //resultList = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
      }

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, bool isNew = false) {
      int totalRows = 0;
      IEnumerable<SalesOrderItem> resultList = null;

      if (isNew) {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = Library.GetAllByPaging(out totalRows, command.Page, command.PageSize, "", "Asc", IncludePredicates,
                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      }

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override void ValidateEntity(SalesOrderItem entity) {
      ModelState.Clear();
      if (entity.Item == null || entity.Item.ID == 0) {
        ModelState.AddModelError("Item", "Item is required");
      }
      else {
        if (string .IsNullOrEmpty(entity.Item_Override)) {
          entity.Item_Override = entity.Item.Short_Name;
        }
      }

      if (string.IsNullOrEmpty( entity.Item_Override)) {
        ModelState.AddModelError("Item", "Item override is required");
      }
    }

    [HttpGet]
    public ActionResult _GetAllItems() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<SalesOrderItem> resultList = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
                                                                                            out totalRows,
                                                                                            1,
                                                                                            ViewBag.PageSize,
                                                                                            "",
                                                                                            "Asc",
                                                                                             new string[] { "SalesOrder", "SalesOrder.Party", "Item" }
                                                            );

      return View("~/Views/Transaction/SalesOrder/_List.cshtml", resultList);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetAllItems(GridCommand command) {
      int totalRows = 0;
      IEnumerable<SalesOrderItem> resultList = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", new string[] { "SalesOrder", "SalesOrder.Party", "Item" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<SalesOrderItem> resultList = ((ILibrary<SalesOrderItem>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc",
    //                                                    new string[] { "Item" },
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

  }


}