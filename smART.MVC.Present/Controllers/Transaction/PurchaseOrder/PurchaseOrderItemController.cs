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
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_PurchaseOrderItem)]
  public class PurchaseOrderItemController : PurchaseOrderChildGridController<PurchaseOrderItemLibrary, PurchaseOrderItem> {
    public PurchaseOrderItemController()
      : base("PurchaseOrderItem", new string[] { "PurchaseOrder", "Item" }, new string[] { "Item" }) {
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(PurchaseOrderItem data, GridCommand command, bool isNew = false)
    //{
    //    ModelState.Clear();
    //    if (data.Item == null || data.Item.ID == 0)
    //    {
    //        ModelState.AddModelError("Item", "Item is required");
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
    //public override ActionResult _Update(PurchaseOrderItem data, GridCommand command, bool isNew = false)
    //{
    //    if (isNew)
    //    {
    //        //TODO: Add logic to update in memory data
    //        TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //    }
    //    else
    //    {
    //        Library.Modify(data, new string[] { "Item" });
    //    }

    //    return Display(command, data, isNew);
    //}
    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<PurchaseOrderItem> resultList;    // = ((IPurchaseOrderChildLibrary<TEntity>)Library).GetAllByPagingByPurchaseOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<PurchaseOrderItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
      }
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, PurchaseOrderItem entity, bool isNew = false) {
      return Display(command, entity.PurchaseOrder.ID.ToString(), isNew);
    }

    //protected override ActionResult Display(GridCommand command, bool isNew = false) {
    //  int totalRows = 0;
    //  IEnumerable<PurchaseOrderItem> resultList = null;

    //  if (isNew) {
    //    resultList = TempEntityList;
    //    totalRows = TempEntityList.Count;
    //  }
    //  else {
    //    resultList = Library.GetAllByPaging(out totalRows, command.Page, command.PageSize, "", "Asc", IncludePredicates,
    //                                                (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
    //  }

    //  return View(new GridModel {
    //    Data = resultList,
    //    Total = totalRows
    //  });
    //}

    protected override void ValidateEntity(PurchaseOrderItem entity) {
      ModelState.Clear();
      if (entity.Item == null || entity.Item.ID == 0) {
        ModelState.AddModelError("Item", "Item is required");
      }
      else {
        if (string.IsNullOrEmpty(entity.Item_Override)) {
          entity.Item_Override = entity.Item.Short_Name;
        }
      }

      if (string.IsNullOrEmpty(entity.Item_Override)) {
        ModelState.AddModelError("Item", "Item override is required");
      }
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<PurchaseOrderItem> resultList = ((ILibrary<PurchaseOrderItem>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc",
    //                                                    new string[] { "Item" },
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    [HttpGet]
    public ActionResult _OpenPurchaseOrderItems() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<PurchaseOrderItem> resultList = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenPOItemsWithPaging(
                                                                                            out totalRows,
                                                                                            1,
                                                                                            ViewBag.PageSize,
                                                                                            "",
                                                                                            "Asc",
                                                                                             new string[] { "PurchaseOrder", "PurchaseOrder.Party", "PurchaseOrder.Contact", "Item" }
                                                            );

      return View("~/Views/Transaction/PurchaseOrder/_OpenPurchaseOrderItems.cshtml", resultList);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenPurchaseOrderItems(GridCommand command, int partyId = 0) {
      int totalRows = 0;
      IEnumerable<PurchaseOrderItem> resultList = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenPOItemsWithPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", new string[] { "PurchaseOrder", "PurchaseOrder.Party", "PurchaseOrder.Contact", "Item" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                                                                      partyId
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }


    [HttpGet]
    public ActionResult _GetAllItems() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<PurchaseOrderItem> resultList = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
                                                                                            out totalRows,
                                                                                            1,
                                                                                            ViewBag.PageSize,
                                                                                            "",
                                                                                            "Asc",
                                                                                             new string[] { "PurchaseOrder", "PurchaseOrder.Party", "PurchaseOrder.Contact", "Item" }
                                                            );

      return View("~/Views/Transaction/PurchaseOrder/_List.cshtml", resultList);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetAllItems(GridCommand command) {
      int totalRows = 0;
      IEnumerable<PurchaseOrderItem> resultList = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", new string[] { "PurchaseOrder", "PurchaseOrder.Party", "PurchaseOrder.Contact", "Item" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)                                                                                                  
                                                                                                      );
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      PurchaseOrderItem entity = Library.GetByID(id.ToString(), new string[] { "PurchaseOrder.Party", "PurchaseOrder.Contact","Item" });
      return Json(entity);
    }

  }
}