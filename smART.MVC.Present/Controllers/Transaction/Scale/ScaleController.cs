using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Helpers;
using smART.Common;
using System.Data;
using Telerik.Web.Mvc.UI;
using smART.Notification;
using System.IO;
using System.Activities;
using smART.Integration.Email;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Scale)]
  public class ScaleController : BaseFormController<ScaleLibrary, Scale> {
    #region Local Members

    private static bool _sendMail = false;

    protected  string[] _predicates = { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No.Booking.Sales_Order_No.Party", "Purchase_Order.Party", "Party_Address", "Sales_Order","PriceList" };

    #endregion Local Members

    #region Constructor

    public ScaleController()
      : base("~/Views/Transaction/Scale/_List.cshtml",
             new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No.Booking.Sales_Order_No.Party", "Purchase_Order.Party", "Party_Address", "Sales_Order", "Local_Sales_AND_Trading_Party", "Asset", "Booking.Sales_Order_No.Party","PriceList" },
             new string[] { "ScaleDetails", "ScaleNotes", "ScaleAttachments", "ScaleExpense"},
             new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No", "Party_Address", "Sales_Order", "Invoice", "Local_Sales_AND_Trading_Party", "Asset", "Booking", "PriceList" }
      ) {
    }

    #endregion Constructor

    #region Override Methods
    
    [HttpGet]
    public override ActionResult _Index() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> resultList = scaleLib.GetAllByPaging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities, null, "LocalSales");
      return View("~/Views/Transaction/Scale/_List.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Index(GridCommand command) {
      int totalRows = 0;

      FilterDescriptor filterDesc = new FilterDescriptor("Active_Ind", FilterOperator.IsNotEqualTo, "false");
      command.FilterDescriptors.Add(filterDesc);
      ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> resultList = scaleLib.GetAllByPaging(
                                                      out totalRows,
                                                      command.Page,
                                                      command.PageSize,
                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                      _includeEntities,
                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                       "LocalSales"
                                                      );

      return View(new GridModel {
        Data = resultList, Total = totalRows
      });
    }

    public virtual ActionResult _OpenTickets() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<Scale> resultList = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByStatus(
                                                                                                          "Open",
                                                                                                            out  totalRows,
                                                                                                            1,
                                                                                                            ViewBag.PageSize,
                                                                                                            "",
                                                                                                            "Asc",
                                                                                                            _predicates,
                                                                                                            null,
                                                                                                            "LocalSales"
                                                                                                            );

      return View("~/Views/Transaction/Scale/_OpenTickets.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public virtual ActionResult _OpenTickets(GridCommand command) {
      int totalRows = 0;
      IEnumerable<Scale> resultList = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByStatus(
                                                                                                          "Open",
                                                                                                          out totalRows,
                                                                                                          command.Page,
                                                                                                          command.PageSize,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                         _predicates,
                                                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors), "LocalSales");
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override void SaveChildEntities(string[] childEntityList, Scale entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */

          case "ScaleDetails":
            if (Session[ChildEntity] != null) {
              ScaleDetailsLibrary ScaleDetailsLibrary = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleDetails> resultList = (IList<ScaleDetails>)Session[ChildEntity];
              foreach (ScaleDetails scaleDetails in resultList) {
                scaleDetails.Scale = new Scale {
                  ID = entity.ID
                };
                ScaleDetailsLibrary.Add(scaleDetails);
              }
            }
            break;

          case "ScaleNotes":
            if (Session[ChildEntity] != null) {
              ScaleNotesLibrary ScaleNotesLibrary = new ScaleNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleNotes> resultList = (IList<ScaleNotes>)Session[ChildEntity];
              foreach (ScaleNotes ScaleNote in resultList) {
                ScaleNote.Parent = new Scale {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                ScaleNotesLibrary.Add(ScaleNote);
              }
            }
            break;

          case "ScaleAttachments":
            if (Session[ChildEntity] != null) {
              ScaleAttachmentsLibrary ScaleLibrary = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleAttachments> resultList = (IList<ScaleAttachments>)Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (ScaleAttachments Scale in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(Scale.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Scale.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(Scale.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Scale.Document_RefId.ToString());
                Scale.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(Scale.Document_Name, sourcePath, destinationPath);

                Scale.Parent = new Scale {
                  ID = entity.ID
                };
                ScaleLibrary.Add(Scale);
              }
            }
            break;
          //case "ScaleIDCardAttachments":
          //  if (Session[ChildEntity] != null) {
          //    ScaleIDCardAttachmentsLibrary ScaleLibrary = new ScaleIDCardAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          //    IEnumerable<ScaleIDCardAttachments> resultList = (IList<ScaleIDCardAttachments>) Session[ChildEntity];
          //    string destinationPath;
          //    string sourcePath;
          //    FilelHelper fileHelper = new FilelHelper();
          //    foreach (ScaleIDCardAttachments Scale in resultList) {
          //      destinationPath = fileHelper.GetSourceDirByFileRefId(Scale.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Scale.Document_RefId.ToString());
          //      sourcePath = fileHelper.GetTempSourceDirByFileRefId(Scale.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Scale.Document_RefId.ToString());
          //      Scale.Document_Path = fileHelper.GetFilePath(sourcePath);
          //      fileHelper.MoveFile(Scale.Document_Name, sourcePath, destinationPath);

          //      Scale.Parent = new Scale {
          //        ID = entity.ID
          //      };
          //      ScaleLibrary.Add(Scale);
          //    }
          //  }
          //  break;
          case "ScaleExpense":
            if (Session[ChildEntity] != null) {
              ScaleExpenseLibrary lib = new ScaleExpenseLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ExpensesRequest> resultList = (IList<ExpensesRequest>)Session[ChildEntity];
              foreach (ExpensesRequest exp in resultList) {
                exp.Reference = new Invoice {
                  ID = entity.ID
                };
                exp.Reference_Table = entity.GetType().Name;
                exp.Reference_ID = entity.ID;
                lib.Add(exp);
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

          case "ScaleDetails":
            if (Convert.ToInt32(parentID) > 0) {
              ScaleDetailsLibrary ScaleDetailsLibrary = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleDetails> resultList = ScaleDetailsLibrary.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (ScaleDetails scaleDetails in resultList) {

                ScaleDetailsLibrary.Delete(scaleDetails.ID.ToString());
              }
            }
            break;

          case "ScaleNotes":
            if (Convert.ToInt32(parentID) > 0) {
              ScaleNotesLibrary ScaleNotesLibrary = new ScaleNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleNotes> resultList = ScaleNotesLibrary.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (ScaleNotes ScaleNote in resultList) {

                ScaleNotesLibrary.Delete(ScaleNote.ID.ToString());
              }
            }
            break;

          case "ScaleAttachments":
            if (Convert.ToInt32(parentID) > 0) {
              ScaleAttachmentsLibrary ScaleLibrary = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ScaleAttachments> resultList = ScaleLibrary.GetAllByParentID(Convert.ToInt32(parentID));

              foreach (ScaleAttachments scaleAttachment in resultList) {

                ScaleLibrary.Delete(scaleAttachment.ID.ToString());
              }
            }
            break;
          //case "ScaleIDCardAttachments":
          //  if (Convert.ToInt32(parentID) > 0) {
          //    ScaleIDCardAttachmentsLibrary ScaleLibrary = new ScaleIDCardAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          //    IEnumerable<ScaleIDCardAttachments> resultList = ScaleLibrary.GetAllByParentID(Convert.ToInt32(parentID));

          //    foreach (ScaleIDCardAttachments scaleAttachment in resultList) {

          //      ScaleLibrary.Delete(scaleAttachment.ID.ToString());
          //    }
          //  }
            //break;
          #endregion
        }
      }
    }

    protected override void ValidateEntity(Scale entity) {

      ModelState.Clear();

      // Set Party for all ticket type. 
      //entity.Party_ID = entity.Scale_Type_Party;

      // For brokerage type ticket type.
      if (entity.Ticket_Type != null && new string[] { "brokerage" }.Any(s => s == entity.Ticket_Type.ToLower())) {
        entity.Party_ID = entity.Brokerage_Party;
        entity.Purchase_Order = entity.Brokerage_Purchase_Order;
      }

      // Set purchase order when it is null.
      if ((entity.Purchase_Order != null && entity.Purchase_Order.ID > 0) && (entity.Party_ID.ID == 0)) {
        PurchaseOrder po = new PurchaseOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(entity.Purchase_Order.ID.ToString(), new[] { "Party" });
        entity.Party_ID = po.Party;
      }

      // Driver is required
      if (entity.Ticket_Type != null && new string[] { "receiving ticket" }.Any(s => s == entity.Ticket_Type.ToLower()) && string.IsNullOrEmpty(entity.Driver_Name))
        ModelState.AddModelError("Driver", "Driver is required.");

      // Request Status is required.
      if (entity.Ticket_Type != null && entity.Ticket_Status.Contains("Select"))
        ModelState.AddModelError("Ticket Status", "Request Status is required.");

      // Ticket Type is required.
      if (entity.Ticket_Type != null && entity.Ticket_Type.Contains("Select"))
        ModelState.AddModelError("Ticket Type", "Ticket Type is required.");

      // Vehicle Plate is required.
      if (entity.Ticket_Type != null && new string[] { "receiving ticket" }.Any(s => s == entity.Ticket_Type.ToLower()) && string.IsNullOrEmpty(entity.Vehicle_Plate_No))
        ModelState.AddModelError("Vehicle plate number ", "Vehicle Plate No# is required.");

      // Party is required    
      if (entity.Ticket_Type != null && new string[] { "receiving ticket", "brokerage", "trading" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Party_ID.ID == 0)
        ModelState.AddModelError("Party", "Party is required.");

      // Container is reuired.
      if (entity.Ticket_Type != null && new string[] { "shipping ticket" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Container_No.ID == 0)
        ModelState.AddModelError("Container", "Container is required.");

      // Container is reuired.
      if (entity.Ticket_Type != null && new string[] { "brokerage" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Booking.ID == 0)
        ModelState.AddModelError("Booking", "Booking is required.");

      if (entity.Ticket_Status != null && entity.Ticket_Status.ToLower().Contains("close")) {

        // Item is reuired.
        if (!IsLineItemExits(entity.ID))
          ModelState.AddModelError("ScaleDetails", "There is at least one line item is required in the item details.");

        // Gross Weight is required.
        if (entity.Gross_Weight <= 0)
          ModelState.AddModelError("Gross_Weight ", "Gross Weight is required.");

        // Tare Weight is required.
        if (entity.Tare_Weight <= 0)
          ModelState.AddModelError("Tare_Weight ", "Tare Weight is required.");

        // Tare weight is required.
        if (entity.Gross_Weight <= entity.Tare_Weight)
          ModelState.AddModelError("Tare_Weight_Diff ", "Tare Weight could not be more then or equal to Gross weight.");

        // Container is reuired.
        if (entity.Ticket_Type != null && new string[] { "brokerage" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Purchase_Order.ID == 0)
          ModelState.AddModelError("PO", "Purchase Order is required.");

      }

      if (new string[] { "local sales", "trading" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Local_Sales_AND_Trading_Party.ID == 0)
        ModelState.AddModelError("LocaleSaleParty", "Local Sales/Trading Party is required.");

      ValidItem(entity);
    }

    #endregion Override Methods

    #region Private Methods

    public bool IsLineItemExits(int scaleId) {
      bool exits = true;
      IEnumerable<ScaleDetails> resultList;

      if (scaleId <= 0) {
        resultList = (IList<ScaleDetails>)Session["ScaleDetails"];
      }
      else {
        ScaleDetailsLibrary ScaleDetailsLibrary = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        resultList = ScaleDetailsLibrary.GetAllByParentID(scaleId);
      }

      if (resultList == null || resultList.Count() <= 0)
        exits = false;

      return exits;
    }

    public void ValidItem(Scale entity) {

      IEnumerable<ScaleDetails> scaleDetails;
      if (entity.ID <= 0)
        scaleDetails = (IList<ScaleDetails>)Session["ScaleDetails"];
      else
        scaleDetails = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(entity.ID, new string[] { "Scale", "Item_Received" });

      if (scaleDetails != null && scaleDetails.Count() > 0) {

        // Ticket Types are Receiving/Trading  
        if (entity.Ticket_Type != null && new string[] { "receiving ticket", "trading" }.Any(s => s == entity.Ticket_Type.ToLower()))
          ValidatePOItem(entity, scaleDetails);

        // Ticket Types are Shipping/Trading
        else if (entity.Ticket_Type != null && new string[] { "shipping ticket", "trading" }.Any(s => s == entity.Ticket_Type.ToLower()) && entity.Container_No != null) {
          Container container = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(entity.Container_No.ID.ToString(), new string[] { "Booking.Sales_Order_No" });
          if (container != null && container.Booking != null && container.Booking.Sales_Order_No != null) {
            ValidateSOItem(container.Booking.Sales_Order_No.ID, scaleDetails);
          }
        }

        // Ticket Types is Local Sale
        else if (entity.Ticket_Type != null && entity.Ticket_Type.ToLower() == "local sale" && entity.Sales_Order != null)
          ValidateSOItem(entity.Sales_Order.ID, scaleDetails);

       // Ticket Types is Brokerage.
        else if (entity.Ticket_Type != null && entity.Ticket_Type.ToLower() == "brokerage") {

          // Validate SO Item
          if (entity.Booking != null && entity.Booking.ID>0) {
            BookingLibrary bookingLib = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            Booking booking = bookingLib.GetByID(entity.Booking.ID.ToString(), new string[] { "Sales_Order_No" });
            if (booking.Sales_Order_No != null) {
              ValidateSOItem(booking.Sales_Order_No.ID, scaleDetails);
            }
          }
          // Validate PO Item          
          ValidatePOItem(entity, scaleDetails);
        }
      }
    }

    private void ValidateSOItem(int soId, IEnumerable<ScaleDetails> scaleDetails) {
      if (soId > 0) {
        SalesOrderItemLibrary soItemLib = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<SalesOrderItem> soItems = soItemLib.GetAllBySalesOrderID(soId, new string[] { "Item" });
        foreach (var scaleItem in scaleDetails) {
          var isSOItem = (from i in soItems
                          where i.Item.ID == scaleItem.Item_Received.ID
                          select i).FirstOrDefault();
          if (isSOItem == null)
            ModelState.AddModelError("Item_Received", string.Format("Scale details item {0} mismatch to selected sales order items.", scaleItem.Item_Received.Short_Name));
        }
      }
    }

    private void ValidatePOItem(Scale scale, IEnumerable<ScaleDetails> scaleDetails) {
      if (scale.Purchase_Order != null && scale.Purchase_Order.ID > 0) {
        PurchaseOrderItemLibrary poItemLib = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<PurchaseOrderItem> poItems = poItemLib.GetAllByParentID(scale.Purchase_Order.ID, new string[] { "Item" });
        foreach (var scaleItem in scaleDetails) {
          var isPOItem = (from i in poItems
                          where i.Item.ID == scaleItem.Item_Received.ID
                          select i).FirstOrDefault();
          if (isPOItem == null)
            ModelState.AddModelError("Item_Received", string.Format("Scale details item {0} mismatch to selected purchase order items.", scaleItem.Item_Received.Short_Name));
        }
      }
    }

    #endregion Private Methods

    #region public Methods

    //[HttpGet]
    //public ActionResult SelectOpneBookingItem(int? id) {
    //  if (id.HasValue) {
    //    Container container = new ContainerLibrary(Configuration.GetsmARTDBContextConnectionString()).GetByID(id.ToString(), new string[] { "Booking.Sales_Order_No.Party" });
    //    if (container != null) {
    //      ClearChildEntities(new string[] { "ScaleDetails", "ScaleNotes", "ScaleAttachments" , "ScaleExpense" });
    //      Scale result = new Scale();
    //      result.Ticket_Type = "Shipping Ticket";
    //      result.Container_No = new Container();
    //      result.Container_No = container;
    //      ViewBag.IsFromOpneBooking = true;
    //      result.ID = 0;
    //      return Display(result);
    //    }
    //  }

    //  return RedirectToAction("New");
    //}
    [HttpGet]
    public ActionResult SelectOpneBookingItem(int? id) {
      if (id.HasValue) {

        // Update Container details
        Container container = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id.ToString(), new string[] { "Booking.Sales_Order_No.Party" });
        if (container != null) {
          ClearChildEntities(new string[] { "ScaleDetails", "ScaleNotes", "ScaleAttachments", "ScaleExpense" });

          Scale result = new Scale();
          result.Ticket_Type = "Shipping Ticket";
          result.Container_No = new Container();
          result.Container_No = container;

          // Update Scale Items
          if (container.Booking != null && container.Booking.Sales_Order_No != null) {
            IEnumerable<SalesOrderItem> soItems = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                      .GetAllBySalesOrderID(container.Booking.Sales_Order_No.ID,
                                                      new string[] { "SalesOrder.Party", "Item" }
                                                    );
            List<ScaleDetails> scaleDetails = new List<ScaleDetails>();
            int count = 0;
            foreach (var soItem in soItems) {
              count += 1;
              ScaleDetails scaleDetail = new ScaleDetails() {
                ID = count,
                Item_Received = soItem.Item,
                Apply_To_Item = soItem.Item,
                Split_Value = 100,
                Scale = new Scale(),
                Created_By = HttpContext.User.Identity.Name,
                Updated_By = HttpContext.User.Identity.Name,
                Created_Date = DateTime.Now,
                Last_Updated_Date = DateTime.Now
              };
              scaleDetails.Add(scaleDetail);
            }
            Session["ScaleDetails"] = scaleDetails;
          }
          ViewBag.IsFromOpneBooking = true;
          result.ID = 0;
          return Display(result);
        }
      }

      return RedirectToAction("New");
    }

    [HttpGet]
    public ActionResult SelectOpenPOItem(int? id) {
      if (id.HasValue) {
        PurchaseOrderItem poItem = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                            .GetByID(id.ToString(),
                                                      new string[] { "PurchaseOrder", "PurchaseOrder.Party", "PurchaseOrder.Contact", "Item" }
                                                    );
        if (poItem != null) {
          ClearChildEntities(new string[] { "ScaleDetails", "ScaleNotes", "ScaleAttachments", "ScaleExpense" });

          // Update Scale
          Scale result = new Scale();
          result.Ticket_Type = "Receiving Ticket";
          result.Purchase_Order = poItem.PurchaseOrder;
          result.Party_ID = poItem.PurchaseOrder.Party;


          // Update Scale Items
          List<ScaleDetails> scaleDetails = new List<ScaleDetails>(){
            new ScaleDetails(){ID =1,Item_Received = poItem.Item,Apply_To_Item = poItem.Item,Split_Value=100,Scale= new Scale(),Created_By= HttpContext.User.Identity.Name,Updated_By= HttpContext.User.Identity.Name ,Created_Date= DateTime.Now,Last_Updated_Date= DateTime.Now

            }    
           
          };
          Session["ScaleDetails"] = scaleDetails;

          ViewBag.IsFromOpnePOItem = true;
          result.ID = 0;
          return Display(result);
        }
      }
      return RedirectToAction("New");
    }

    //[HttpGet]
    //public ActionResult GetWeight() {
    //  double weight = 0.00;

    //  string ip = Configuration.GetsmARTScaleWeightIPAddress();
    //  string port = Configuration.GetsmARTScaleWeightPort();
    //  string waitTime = Configuration.GetsmARTScaleWeightWaitTime();
    //  string command = Configuration.GetsmARTScaleWeightCommand();

    //  int iPort = int.Parse(port);
    //  int iwTime = int.Parse(waitTime);

    //  //Get weight
    //  weight = Library.GetScaleWeight(ip, iPort, iwTime, command);

    //  return Json(new {
    //    Weight = weight
    //  }, JsonRequestBehavior.AllowGet);
    //}

    [HttpGet]
    public ActionResult GetWeight() {
      double weight = 0.00;

      string comPort = ConfigurationHelper.GetsmARTScaleWeightComPort();
      int baudRate = int.Parse(ConfigurationHelper.GetsmARTScaleWeightBaudRate());
      int dataBits = int.Parse(ConfigurationHelper.GetsmARTScaleWeightDataBits());
      int stopBits = int.Parse(ConfigurationHelper.GetsmARTScaleWeightStopBits());
      int timeout = int.Parse(ConfigurationHelper.GetsmARTScaleWeightTimeout());
      string logFileName = ConfigurationHelper.GetsmARTScaleWeightLogFile();

      //Get weight
      weight = Library.GetScaleWeight(comPort, baudRate, dataBits, stopBits, timeout, logFileName);

      return Json(new {
        Weight = weight
      }, JsonRequestBehavior.AllowGet);
    }
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetScaleTicketsBySO(GridCommand command, string soId) {
      int totalRows = 0;
      var aggregates = new Dictionary<string, object>();
      ScaleLibrary lib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> resultList = lib.GetTicketsBySOWithPagging(Convert.ToInt32(soId),
                                                                    out totalRows,
                                                                    command.Page,
                                                                    command.PageSize,
                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                    _predicates,
                                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      if (command.Aggregates.Any()) {
        aggregates["Net_Weight_SOUOM"] = new { Sum = resultList.Sum(p => p.Net_Weight_SOUOM) };
      }
      return View(new GridModel { Data = resultList, Total = totalRows, Aggregates = aggregates });
    }


    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetScaleTicketsByPO(GridCommand command, string poId) {
      int totalRows = 0;
      var aggregates = new Dictionary<string, object>();
      ScaleLibrary lib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> resultList = lib.GetTicketsByPOWithPagging(Convert.ToInt32(poId),
                                                                    out totalRows,
                                                                    command.Page,
                                                                    command.PageSize,
                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                    _predicates,
                                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      if (command.Aggregates.Any()) {
        aggregates["Net_Weight_POUOM"] = new { Sum = resultList.Sum(p => p.Net_Weight_POUOM) };
      }
      return View(new GridModel { Data = resultList, Total = totalRows, Aggregates = aggregates });
    }

    [HttpGet]
    public ActionResult SendEmail(string id) {
      try {
        if (string.IsNullOrEmpty(id))
          throw new Exception("Email send failed due to scale ID not found.");

        ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        Scale scale = scaleLib.GetByID(id, new string[] { "Party_ID" });

        if (scale == null || scale.Party_ID == null || scale.Ticket_Type != "Receiving Ticket" || scale.Ticket_Status != "Close")
          throw new Exception("Email send failed.");

        IEnumerable<Contact> contacts = Helpers.ContactHelper.GetEmailContactsByPartyId(scale.Party_ID.ID);
        if (contacts.Count() <= 0)
          throw new Exception("There is no email contact exists.");

        NotificationDefinition notDef = new NotificationDefinition();
        notDef.ToRecipients = new System.Net.Mail.MailAddressCollection();
        foreach (var item in contacts) {
          notDef.ToRecipients.Add(new System.Net.Mail.MailAddress(item.Email, item.ListText));
        }

        EmployeeHelper employeeHelper = new EmployeeHelper();
        Employee employee = employeeHelper.GetEmployeeByUsername(System.Web.HttpContext.Current.User.Identity.Name);
        if (employee == null || string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Email_Password))
          throw new Exception("Sender email and password is required.");


        string xslPath = Path.Combine(ConfigurationHelper.GetsmARTXslPath(), "ScaleEmailBody.xslt");
        string smtpAddress = ConfigurationHelper.GetsmARTSMTPServer();

        Dictionary<string, object> parameter = new Dictionary<string, object>();
        parameter.Add("ScaleID", id);

        SSRSReport ssrsHelper = new SSRSReport("ScaleReceiveTicket.rdl", parameter);
        string attachment = ssrsHelper.ExportReportToPDF();

        if (string.IsNullOrEmpty(attachment))
          throw new Exception("There is no attachment found.");

        notDef.Attachments = new List<System.Net.Mail.Attachment>();
        notDef.Attachments.Add(new System.Net.Mail.Attachment(attachment));
        notDef.DeliveryType = EnumNotificationDeliveryType.Email;
        notDef.FormatType = EnumFormatType.HTML;
        notDef.Sender = new System.Net.Mail.MailAddress(employee.Email, employee.Emp_Name);
        notDef.SMTPServer = smtpAddress;
        notDef.SMTPServerCredentialID = employee.Email;
        notDef.SMTPServerCredentialPwd = employee.Email_Password;
        notDef.Subject = "Ticket#" + id.ToString();
        NotificationHelper.StartNotificationWF(id, scale.Party_Name, PartyHelper.GetOrganizationName(), employee.Emp_Name, notDef, xslPath, NotificationWFCompleted);

        //return Display(command, container);
        if (_sendMail == true)
          return Json(new { Sucess = "Email send sucessfully." }, JsonRequestBehavior.AllowGet);
        else
          return Json(new { Sucess = "Email send failed." }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception ex) {
        return Json(new { Sucess = ex.Message }, JsonRequestBehavior.AllowGet);
      }

    }

    // This method called on notification workflow completion.
    public void NotificationWFCompleted(WorkflowApplicationCompletedEventArgs e) {
      switch (e.CompletionState) {
        // If workflow complated without any error.
        case ActivityInstanceState.Closed:
          string id = Convert.ToString(e.Outputs["Result"]);
          ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          Scale scale = scaleLib.GetByID(id, new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No", "Party_Address", "Sales_Order", "Invoice" });
          scale.Send_Mail = true;
          scale.Mail_Send_On = DateTime.Now;
          scaleLib.Modify(scale, new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No", "Party_Address", "Sales_Order", "Invoice" });
          _sendMail = true;
          break;
        // If any error occurred during workflow execution, will be handled here.
        case ActivityInstanceState.Faulted:
          Exception ex = e.TerminationException;
          _sendMail = false;
          ExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error, "Error occured in Notification Workflow", "PresentPolicy");
          break;
      }
    }

    #endregion Public Methods

    #region Deleted

    #endregion Deleted

  }

}

