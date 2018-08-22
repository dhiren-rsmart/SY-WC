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
  [Feature(EnumFeatures.Transaction_SalesOrder)]
  public class SalesOrderController : BaseFormController<SalesOrderLibrary, SalesOrder> {
    #region Constructor

    public SalesOrderController()
      : base("~/Views/Transaction/SalesOrder/_List.cshtml",
             new string[] { "Party", "Contact" },
             new string[] { "SalesOrderItem", "SalesOrderNotes", "SalesOrderAttachments" }, new string[] { "Party", "Contact" }) {
    }

    #endregion Constructor

    #region Override Methods

    //public override ActionResult Index(int? id)
    //{
    //    if (id.HasValue)
    //    {
    //        SalesOrder entity = Library.GetByID(id.ToString(), new string[] { "Party","Contact" });
    //        return Display(entity);
    //    }
    //    else
    //        return RedirectToAction("New");
    //}           



    //[HttpPost]
    //public override ActionResult Save(SalesOrder entity)
    //{
    //    ModelState.Clear();

    //    // Need to find an easier way

    //    if (entity.Party.ID==0)
    //        ModelState.AddModelError("Party", "Party is required");

    //    if (entity.Contact.ID == 0)
    //        ModelState.AddModelError("Contact", "Contact is required");


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
    //            Library.Modify(entity, new string[] { "Party", "Contact" });
    //        }
    //        ModelState.Clear();
    //    }
    //    return Display(entity);
    //}

    protected override void ValidateEntity(SalesOrder entity) {
      ModelState.Clear();

      // Need to find an easier way

      if (entity.Party.ID == 0)
        ModelState.AddModelError("Party", "Party is required");

      if (entity.Contact.ID == 0)
        ModelState.AddModelError("Contact", "Contact is required");

    }

    protected override void SaveChildEntities(string[] childEntityList, SalesOrder entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "SalesOrderItem":
            if (Session[ChildEntity] != null) {
              SalesOrderItemLibrary salesorderItemLibrary = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderItem> resultList = (IList<SalesOrderItem>)Session[ChildEntity];
              foreach (SalesOrderItem salesorderItem in resultList) {
                salesorderItem.SalesOrder = new SalesOrder {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                salesorderItemLibrary.Add(salesorderItem);
              }
            }
            break;

          case "SalesOrderNotes":
            if (Session[ChildEntity] != null) {
              SalesOrderNotesLibrary salesorderNotesLibrary = new SalesOrderNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderNotes> resultList = (IList<SalesOrderNotes>)Session[ChildEntity];
              foreach (SalesOrderNotes salesorderNote in resultList) {
                salesorderNote.Parent = new SalesOrder {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                salesorderNotesLibrary.Add(salesorderNote);
              }
            }
            break;

          case "SalesOrderAttachments":
            if (Session[ChildEntity] != null) {
              SalesOrderAttachmentsLibrary salesorderLibrary = new SalesOrderAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderAttachments> resultList = (IList<SalesOrderAttachments>)Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (SalesOrderAttachments salesorder in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(salesorder.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), SalesOrder.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(salesorder.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), SalesOrder.Document_RefId.ToString());
                salesorder.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(salesorder.Document_Name, sourcePath, destinationPath);

                salesorder.Parent = new SalesOrder {
                  ID = entity.ID
                };
                salesorderLibrary.Add(salesorder);
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
          case "SalesOrderItem":
            if (Session[ChildEntity] != null) {
              SalesOrderItemLibrary salesorderItemLibrary = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderItem> resultList = (IList<SalesOrderItem>) Session[ChildEntity];
              foreach (SalesOrderItem item in resultList) {           
                salesorderItemLibrary.Delete(item.ToString());
              }
            }
            break;

          case "SalesOrderNotes":
            if (Session[ChildEntity] != null) {
              SalesOrderNotesLibrary salesorderNotesLibrary = new SalesOrderNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderNotes> resultList = (IList<SalesOrderNotes>) Session[ChildEntity];
              foreach (SalesOrderNotes note in resultList) {            
                salesorderNotesLibrary.Delete(note.ToString());
              }
            }
            break;

          case "SalesOrderAttachments":
            if (Session[ChildEntity] != null) {
              SalesOrderAttachmentsLibrary salesorderLibrary = new SalesOrderAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SalesOrderAttachments> resultList = (IList<SalesOrderAttachments>) Session[ChildEntity];          
              foreach (SalesOrderAttachments attachment in resultList) {                
                salesorderLibrary.Delete(attachment.ToString());
              }
            }
            break;

          #endregion
        }
      }
    }
    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenSalesOrders(GridCommand command, int partyId = 0) {
      int totalRows = 0;
      SalesOrderLibrary salesOrderLibrary = new SalesOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<SalesOrder> resultList = salesOrderLibrary.GetOpenSOWithPagging(
                                                    out totalRows,
                                                    command.Page,
                                                    command.PageSize,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                    new string[] { "Party", "Contact" },
                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                    partyId
                                                  );
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenBrokerageSalesOrders(GridCommand command, int partyId = 0) {
      int totalRows = 0;
      SalesOrderLibrary salesOrderLibrary = new SalesOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<SalesOrder> resultList = salesOrderLibrary.GetOpenBrokerageSOWithPagging(
                                                    out totalRows,
                                                    command.Page,
                                                    command.PageSize,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                    new string[] { "Party", "Contact" },
                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                    partyId
                                                  );
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      SalesOrder entity = Library.GetByID(id.ToString(), new string[] { "Party", "Contact" });

      return Json(entity);
    }

    #endregion Override Methods

    #region Supporting Methods

    public ActionResult Copy(int? refID) {
      if (refID.HasValue) {
        SalesOrder entity = Library.GetByID(refID.ToString(), new string[] { "Party", "Contact" });
        CopyChildEntities(entity.ID);
        ViewBag.IsCopy = true;
        entity.ID = 0;
        return Display(entity);
      }
      else
        return RedirectToAction("New");
    }

    public void CopyChildEntities(int ParentID) {
      string con = ConfigurationHelper.GetsmARTDBContextConnectionString();

      SalesOrderItemLibrary soItemLib = new SalesOrderItemLibrary();
      soItemLib.Initialize(con);
      IEnumerable<SalesOrderItem> soItems = soItemLib.GetAllBySalesOrderID(ParentID, new string[] { "SalesOrder", "Item" });
      Session["SalesOrderItem"] = soItems;

      SalesOrderNotesLibrary soNotesLib = new SalesOrderNotesLibrary();
      soNotesLib.Initialize(con);
      IEnumerable<SalesOrderNotes> soNotess = soNotesLib.GetAllByParentID(ParentID);
      Session["SalesOrderNotes"] = soNotess;

      SalesOrderAttachmentsLibrary soAttachmentsLib = new SalesOrderAttachmentsLibrary();
      soAttachmentsLib.Initialize(con);
      IEnumerable<SalesOrderAttachments> soAttachmentss = soAttachmentsLib.GetAllByParentID(ParentID);
      Session["SalesOrderAttachments"] = soAttachmentss;

    }

    public ActionResult _Print() {
      //int totalRows = 0;
      ViewBag.ID = 1;
      //IEnumerable<SalesOrder> resultList = ((ILibrary<SalesOrder>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc");
      return View("~/Views/Transaction/SalesOrder/_Print.cshtml");
    }

    #endregion Supporting Methods

    #region Deleted
    //[HttpPost]
    //public ActionResult _GetSalesOrder(int id)
    //{
    //    //return new JsonResult { Data = Helpers.sa(id.ToString()) };
    //}

    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<SalesOrder> resultList = ((ILibrary<SalesOrder>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc", new string[] { "Party", "Contact" });
    //    return View("~/Views/Transaction/SalesOrder/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<SalesOrder> resultList = ((ILibrary<SalesOrder>)Library).GetAllByPaging(
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
    //public override ActionResult _Delete(string id)
    //{
    //    return RedirectToAction("New");
    //}
    #endregion Deleted
  }
}
