using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_PurchaseOrder)]
  public class PurchaseOrderController : BaseFormController<PurchaseOrderLibrary, PurchaseOrder> {
    #region Constructor

    public PurchaseOrderController()
      : base("~/Views/Transaction/PurchaseOrder/_List.cshtml",
            new string[] { "Party", "Contact", "Price_List" },
            new string[] { "PurchaseOrderItem", "PurchaseOrderNotes", "PurchaseOrderAttachments" }, new string[] { "Party", "Contact", "Price_List" }) {
    }

    #endregion Constructor

    #region Override Methods

    //public override ActionResult Index(int? id)
    //{
    //    if (id.HasValue)
    //    {
    //        PurchaseOrder entity = Library.GetByID(id.ToString(), new string[] { "Party", "Contact", "Price_List" });
    //        return Display(entity);
    //    }
    //    else
    //        return RedirectToAction("New");
    //}    

    //[HttpPost]
    //public override ActionResult Save(PurchaseOrder entity)
    //{
    //    ModelState.Clear();

    //    // Need to find an easier way

    //    if (entity.Party.ID == 0)
    //        ModelState.AddModelError("Party", "Party is required");

    //    if (entity.Contact.ID == 0)
    //        ModelState.AddModelError("Contact", "Contact is required");
    //    if (entity.Order_Status == string.Empty || entity.Order_Status=="-- SelectValue ---")
    //        ModelState.AddModelError("Order_Status", "Order Status is required");
    //    if (entity.Order_Type == string.Empty || entity.Order_Type == "-- SelectValue ---")
    //        ModelState.AddModelError("Order_Type", "Order Type is required");


    //    if (ModelState.IsValid)
    //    {
    //        if (entity.ID == 0)
    //        {
    //            entity.Order_Created_By = HttpContext.User.Identity.Name;
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
    //            Library.Modify(entity, new string[] { "Party", "Contact", "Price_List" });
    //        }
    //        ModelState.Clear();
    //    }
    //    return Display(entity);
    //}

    protected override void ValidateEntity(PurchaseOrder entity) {
      ModelState.Clear();

      // Need to find an easier way

      if (entity.Party.ID == 0)
        ModelState.AddModelError("Party", "Party is required");

      if (entity.Contact.ID == 0)
        ModelState.AddModelError("Contact", "Contact is required");
      if (entity.Order_Status == string.Empty || entity.Order_Status == "-- SelectValue ---")
        ModelState.AddModelError("Order_Status", "Order Status is required");
      if (entity.Order_Type == string.Empty || entity.Order_Type == "-- SelectValue ---")
        ModelState.AddModelError("Order_Type", "Order Type is required");


    }
    protected override void SaveChildEntities(string[] childEntityList, PurchaseOrder entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "PurchaseOrderItem":
            if (Session[ChildEntity] != null) {
              PurchaseOrderItemLibrary PurchaseorderItemLibrary = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderItem> resultList = (IList<PurchaseOrderItem>) Session[ChildEntity];
              foreach (PurchaseOrderItem PurchaseorderItem in resultList) {
                PurchaseorderItem.PurchaseOrder = new PurchaseOrder {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                PurchaseorderItemLibrary.Add(PurchaseorderItem);
              }
            }
            break;

          case "PurchaseOrderNotes":
            if (Session[ChildEntity] != null) {
              PurchaseOrderNotesLibrary PurchaseorderNotesLibrary = new PurchaseOrderNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderNotes> resultList = (IList<PurchaseOrderNotes>) Session[ChildEntity];
              foreach (PurchaseOrderNotes PurchaseorderNote in resultList) {
                PurchaseorderNote.Parent = new PurchaseOrder {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                PurchaseorderNotesLibrary.Add(PurchaseorderNote);
              }
            }
            break;

          case "PurchaseOrderAttachments":
            if (Session[ChildEntity] != null) {
              PurchaseOrderAttachmentsLibrary PurchaseorderLibrary = new PurchaseOrderAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderAttachments> resultList = (IList<PurchaseOrderAttachments>) Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (PurchaseOrderAttachments Purchaseorder in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(Purchaseorder.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), PurchaseOrder.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(Purchaseorder.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), PurchaseOrder.Document_RefId.ToString());
                Purchaseorder.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(Purchaseorder.Document_Name, sourcePath, destinationPath);

                Purchaseorder.Parent = new PurchaseOrder {
                  ID = entity.ID
                };
                PurchaseorderLibrary.Add(Purchaseorder);
              }
            }
            break;

          #endregion
        }
      }
    }

    protected override void DeleteChildEntities(string[] childEntityList, string parentID) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "PurchaseOrderItem":
            if (Session[ChildEntity] != null) {
              PurchaseOrderItemLibrary PurchaseorderItemLibrary = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderItem> resultList = (IList<PurchaseOrderItem>) Session[ChildEntity];
              foreach (PurchaseOrderItem item in resultList) {
                PurchaseorderItemLibrary.Delete(item.ID.ToString());
              }
            }
            break;

          case "PurchaseOrderNotes":
            if (Session[ChildEntity] != null) {
              PurchaseOrderNotesLibrary PurchaseorderNotesLibrary = new PurchaseOrderNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderNotes> resultList = (IList<PurchaseOrderNotes>) Session[ChildEntity];
              foreach (PurchaseOrderNotes notes in resultList) {
                PurchaseorderNotesLibrary.Delete(notes.ID.ToString());
              }
            }
            break;

          case "PurchaseOrderAttachments":
            if (Session[ChildEntity] != null) {
              PurchaseOrderAttachmentsLibrary PurchaseorderLibrary = new PurchaseOrderAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PurchaseOrderAttachments> resultList = (IList<PurchaseOrderAttachments>) Session[ChildEntity];
              foreach (PurchaseOrderAttachments attachments in resultList) {
                PurchaseorderLibrary.Delete(attachments.ID.ToString());
              }
            }
            break;

          #endregion
        }
      }
    }

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      PurchaseOrder entity = Library.GetByID(id.ToString(), new string[] { "Party", "Contact" });

      return Json(entity);
    }

    #endregion Override Methods

    #region Supporring Methods

    public ActionResult Copy(int? refID) {
      if (refID.HasValue) {
        PurchaseOrder entity = Library.GetByID(refID.ToString(), new string[] { "Party", "Contact", "Price_List" });
        CopyChildEntities(entity.ID);
        entity.ID = 0;
        //return Save(entity);
        ViewBag.IsCopy = true;
        return Display(entity);
      }
      else
        return RedirectToAction("New");
    }

    public void CopyChildEntities(int ParentID) {
      string con = ConfigurationHelper.GetsmARTDBContextConnectionString();

      PurchaseOrderItemLibrary poItemLib = new PurchaseOrderItemLibrary();
      poItemLib.Initialize(con);
      IEnumerable<PurchaseOrderItem> poItems = poItemLib.GetAllByParentID(ParentID, new string[] { "PurchaseOrder", "Item" });
      Session["PurchaseOrderItem"] = poItems;

      PurchaseOrderNotesLibrary poNotesLib = new PurchaseOrderNotesLibrary();
      poNotesLib.Initialize(con);
      IEnumerable<PurchaseOrderNotes> poNotess = poNotesLib.GetAllByParentID(ParentID);
      Session["PurchaseOrderNotes"] = poNotess;

      PurchaseOrderAttachmentsLibrary poAttachmentsLib = new PurchaseOrderAttachmentsLibrary();
      poAttachmentsLib.Initialize(con);
      IEnumerable<PurchaseOrderAttachments> poAttachmentss = poAttachmentsLib.GetAllByParentID(ParentID);
      Session["PurchaseOrderAttachments"] = poAttachmentss;

    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenPurchaseOrders(GridCommand command, int partyId = 0) {
      int totalRows = 0;
      IEnumerable<PurchaseOrder> resultList = new PurchaseOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenPOWithPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", new string[] { "Party", "Contact" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                                                                      partyId
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenBrokeragePurchaseOrders(GridCommand command, int partyId = 0) {
      int totalRows = 0;
      IEnumerable<PurchaseOrder> resultList = new PurchaseOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenBrokeragePOWithPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", new string[] { "Party", "Contact" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                                                                      partyId
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }
    #endregion Supporting Methods

    #region Deleted

    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<PurchaseOrder> resultList = ((ILibrary<PurchaseOrder>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc", new string[] { "Party", "Contact", "Price_List" });
    //    return View("~/Views/Transaction/PurchaseOrder/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<PurchaseOrder> resultList = ((ILibrary<PurchaseOrder>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc",
    //                                                    new string[] { "Party", "Contact" },
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    //[HttpPost]
    //public ActionResult _GetPurchaseOrder(int id)
    //{
    //    //return new JsonResult { Data = Helpers.sa(id.ToString()) };
    //}


    //[HttpPost]
    //public override ActionResult _Delete(string id)
    //{
    //    return RedirectToAction("New");
    //}


    #endregion Deleted

  }
}
