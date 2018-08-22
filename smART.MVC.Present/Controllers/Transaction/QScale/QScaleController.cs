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
using System.Transactions;
using smART.MVC.Present.Extensions;
using System.Drawing.Printing;
using System.Drawing;
using System.Diagnostics;
using smART.MVC.Present.Controllers.Transaction;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Transaction_Scale)]
  public class QScaleController : ScaleController {
    public const string ReceivingTicketType = "Receiving Ticket";
    public enum TicketStatus {
      Open,
      Close
    }


    public ActionResult Index_Qscale() {
      return View("Index");
    }

    protected override ActionResult Display(Scale entity) {
      if (entity.Party_ID != null)
        entity.PrimaryAddress = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetPrimaryAddressesByPartyId(entity.Party_ID.ID);
      if (entity.PrimaryAddress == null)
        entity.PrimaryAddress = new AddressBook();


      if (entity.ID == 0) {
        entity.Ticket_Status = TicketStatus.Open.ToString();
        entity.Created_Date = DateTime.Now;
        entity.Created_By = HttpContext.User.Identity.Name;
        entity.Unique_ID = Convert.ToInt32(Session["Unique_ID"]);
      }
      else {
        entity.Payment = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).IsPaidTicket(entity.ID, new string[] { "PaymentReceipt", "Settlement.Scale" });
      }
      return PartialView("New", entity);
    }

    protected override ActionResult Display(string id) {
      Scale result = ((ILibrary<Scale>)Library).GetByID(id, _includeEntities);
      if (result.Party_ID != null)
        result.PrimaryAddress = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetPrimaryAddressesByPartyId(result.Party_ID.ID);
      if (result.PrimaryAddress == null)
        result.PrimaryAddress = new AddressBook();
      return PartialView("New", result);
    }

    public ActionResult Navigation() {
      return PartialView("Navigation");
    }

    public override ActionResult _OpenTickets() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<Scale> resultList = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByStatus(TicketStatus.Open.ToString(),
                                                                                                            out  totalRows,
                                                                                                            1,
                                                                                                            ViewBag.PageSize,
                                                                                                            "",
                                                                                                            "Asc",
                                                                                                            _predicates,
                                                                                                            null,
                                                                                                           ReceivingTicketType
                                                                                                            );
      return View("~/Views/Transaction/QScale/_OpenTickets.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _OpenTickets(GridCommand command) {
      int totalRows = 0;
      IEnumerable<Scale> resultList = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByStatus(
                                                                                                          TicketStatus.Open.ToString(),
                                                                                                          out totalRows,
                                                                                                          command.Page,
                                                                                                          command.PageSize,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                         _predicates,
                                                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors),
                                                                                                          ReceivingTicketType);
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpGet]
    public override ActionResult _Index() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> resultList = scaleLib.GetAllByPaging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities, null, ReceivingTicketType);
      return View("~/Views/Transaction/QScale/_List.cshtml", resultList);
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
                                                       ReceivingTicketType
                                                      );

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }


    public void ValidQScaleItems(Scale entity, bool isPayment = false) {

      IEnumerable<ScaleDetails> scaleDetails = GetDetailItemsByScaleId(entity.ID);
      if (scaleDetails == null) {
        return;
      }
      foreach (var scaleItem in scaleDetails) {
        if (scaleItem.Apply_To_Item == null || scaleItem.Apply_To_Item.ID == 0)
          ModelState.AddModelError("Item", "Item is a required field");

        if (entity.Ticket_Status == "Close" && scaleItem.NetWeight <= 0) {
          ModelState.AddModelError("", string.Format("Net weight is required for item {0}.", scaleItem.Item_Received.Short_Name));

          // Amount is required    
          if (scaleItem.Rate <= 0 && isPayment)
            ModelState.AddModelError("Price", "Price is a required field.");

          // Price is required    
          if (scaleItem.Amount <= 0 && isPayment)
            ModelState.AddModelError("Amount", "Amount is a required field.");
        }
      }
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _MakePayment(Scale data, bool isNew = false) {
      try {
        // Start transaction.
        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
          IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
        })) {

          // Save scale record        
          ActionResult result = base.Save(data);
          data = ModelFromActionResult<Scale>(result);

          if (data.Ticket_Settled == false) {
            // Settlement
            IEnumerable<ScaleDetails> scaleDetails = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(data.ID);
            Settlement addSettlement = AddSettlement(data, scaleDetails);

            // Payment
            Settlement settlement = new SettlementLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(addSettlement.ID.ToString(), new string[] { "Scale.Party_ID" });
            IEnumerable<SettlementDetails> settlementDetails = new SettlementDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(settlement.ID);
            AddPayment(settlement, settlementDetails);

            // Cash
            AddCash(settlement);
          }

          // Complete transaction.
          scope.Complete();
          ModelState.Clear();
        }
        return Display(data.ID.ToString());
      }
      catch (Exception ex) {
        Response.StatusCode = 500;
        if (ex.GetBaseException() is smART.Common.DuplicateException)
          ModelState.AddModelError("Error", ex.GetBaseException().Message);
        else
          ModelState.AddModelError("Error", ex.Message);
        return Display(data);
      }
    }

    private Settlement AddSettlement(Scale scale, IEnumerable<ScaleDetails> details) {
      // Add Settlement
      SettlementController settlementCont = new SettlementController();
      Settlement newSettlement = settlementCont.SaveSettlement(scale, scale.Item_Amount, scale.Net_Weight);

      // Add Settlement Details
      List<SettlementDetails> setDetailList = new List<SettlementDetails>();
      foreach (var item in details) {
        SettlementDetails settlementDetails = new SettlementDetails();
        settlementDetails.Settlement_ID = new Settlement() {
          ID = newSettlement.ID
        };
        settlementDetails.Settlement_ID = newSettlement;
        settlementDetails.Rate = item.Rate;
        settlementDetails.Amount = item.Amount;
        settlementDetails.Item_UOM = "LBS";
        settlementDetails.Item_UOM_NetWeight = item.NetWeight;
        settlementDetails.Scale_Details_ID = new ScaleDetails() {
          ID = item.ID
        };
        setDetailList.Add(settlementDetails);
      }
      settlementCont.SaveSettlementDetails(setDetailList);
      return newSettlement;
    }

    private void AddPayment(Settlement settlement, IEnumerable<SettlementDetails> settlementDetails) {
      // Add Payment
      //PaymentReceiptLibrary paymentLibrary = new PaymentReceiptLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      PaymentReceipt payment = new PaymentReceipt();
      payment.Transaction_Type = "Payment";
      payment.Transaction_Mode = "Cash";
      payment.Transaction_Date = DateTime.Now;
      payment.Transaction_Status = "Closed";
      payment.Cash_Amount = settlement.Amount;
      payment.Bank_Amount = 0;
      payment.Cash_Drawer = "1";
      payment.Check_Wire_Transfer = "1";
      payment.Created_By = HttpContext.User.Identity.Name;
      payment.Created_Date = DateTime.Now;
      payment.Active_Ind = true;
      payment.Updated_By = HttpContext.User.Identity.Name;
      payment.Last_Updated_Date = DateTime.Now;
      payment.Party = new Party() {
        ID = settlement.Scale.Party_ID.ID
      };
      payment.Account_Name = new Bank();
      payment.Net_Amt = settlement.Amount;
      payment.Payment_Receipt_Type = "Tickets";
      PaymentController payCont = new PaymentController();
      payment = payCont.SavePayment(payment);

      //payment = paymentLibrary.Add(payment);

      // Add Payment Details
      PaymentReceiptDetails paymentDetails = new PaymentReceiptDetails();
      paymentDetails.PaymentReceipt = new PaymentReceipt() {
        ID = payment.ID
      };
      paymentDetails.Apply_Amount = settlement.Amount; ;
      paymentDetails.Balance_Amount = 0;
      paymentDetails.Paid_In_Full = true;
      paymentDetails.Settlement = new Settlement() {
        ID = settlement.ID
      };
      paymentDetails.Created_By = HttpContext.User.Identity.Name;
      paymentDetails.Created_Date = DateTime.Now;
      paymentDetails.Active_Ind = true;
      paymentDetails.Updated_By = HttpContext.User.Identity.Name;
      paymentDetails.Last_Updated_Date = DateTime.Now;
      payCont.SavePaymentDetails(paymentDetails);

    }

    private void AddCash(Settlement settlement) {
      Cash cash = new Cash();
      cash.Amount = settlement.Amount;
      cash.Transaction_Type = "Payment";
      cash.Created_By = HttpContext.User.Identity.Name;
      cash.Created_Date = DateTime.Now;
      cash.Active_Ind = true;
      cash.Updated_By = HttpContext.User.Identity.Name;
      cash.Last_Updated_Date = DateTime.Now;
      CashController cashCont = new CashController();
      cashCont.Save(cash);
    }

    private IEnumerable<ScaleDetails> GetDetailItemsByScaleId(int scaleID) {
      IEnumerable<ScaleDetails> scaleDetails;
      if (scaleID <= 0)
        scaleDetails = (IList<ScaleDetails>)Session["ScaleDetails"];
      else
        scaleDetails = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(scaleID, new string[] { "Scale", "Item_Received" });
      return scaleDetails;
    }

    protected override void ValidateEntity(Scale entity) {
      ModelState.Clear();
      entity.Ticket_Type = ReceivingTicketType;
      entity.QScale = true;
      // Add new scale id
      if (entity.ID == 0) {
        ScaleLibrary lib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        entity.Scale_Ticket_No = lib.GenerateUniqueId(Session["Unique_ID"].ToString());
      }
      if (string.IsNullOrEmpty(entity.Ticket_Status)) entity.Ticket_Status = TicketStatus.Open.ToString();


      if (entity.Party_ID != null && !string.IsNullOrEmpty(entity.Party_ID.License_No)) {

        if (string.IsNullOrEmpty(entity.Party_ID.Party_Name))
          throw new Exception("Party name is required.");

        PartyController partyCont = new PartyController();
        entity.Party_ID.Created_By = HttpContext.User.Identity.Name;
        string thumbImage1RefId, thumbImage2RefId, photoRefId, signatureImageRefId, drivingLicImageRefId, vechicleImageRefId, cashCardImageRefId;
        GetCustomerImageRefIds(entity.ID, out thumbImage1RefId, out thumbImage2RefId, out photoRefId, out signatureImageRefId, out drivingLicImageRefId, out vechicleImageRefId, out cashCardImageRefId);
        entity.Party_ID.ThumbImage1RefId = thumbImage1RefId;
        entity.Party_ID.ThumbImage2RefId = thumbImage2RefId;
        entity.Party_ID.PhotoRefId = photoRefId;
        entity.Party_ID.SignatureImageRefId = signatureImageRefId;
        entity.Party_ID.LicenseImageRefId = drivingLicImageRefId;
        entity.Party_ID.VehicleImageRegId = vechicleImageRefId;
        entity.Party_ID.CashCardImageRefId = cashCardImageRefId;
        entity.Party_ID.State = entity.State;
        Party newParty = partyCont.SaveParty(entity.Party_ID, entity.PrimaryAddress);
        entity.Party_ID.ID = newParty.ID;

        if (entity.Party_ID.ID <= 0)
          throw new Exception("Issue to add new party.");
      }

      Validate(entity);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult Validate(Scale data) {
      ModelState.Clear();

      data.Ticket_Type = ReceivingTicketType;
      data.QScale = true;

      if (string.IsNullOrEmpty(data.Ticket_Status)) data.Ticket_Status = TicketStatus.Open.ToString();

      if (data.Party_ID != null && data.Party_ID.ID == 0 && !string.IsNullOrEmpty(data.Party_ID.License_No)) {
        PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        Party party = partyLib.GetByLicenseNo(data.Party_ID.License_No);
        if (party != null)
          ModelState.AddModelError("Party", "Duplicate License#.");
      }

      // Request Status is required.
      if (data.Ticket_Type != null && data.Ticket_Status.Contains("Select"))
        ModelState.AddModelError("Ticket Status", "Request Status is a required field.");

      // Ticket Type is required.
      if (data.Ticket_Type != null && data.Ticket_Type.Contains("Select"))
        ModelState.AddModelError("Ticket Type", "Ticket type is a required field.");

      //// Vehicle Plate is required.
      //if (string.IsNullOrEmpty(data.Vehicle_Plate_No))
      //  ModelState.AddModelError("Vehicle plate number ", "Vehicle plate number is a required field.");


      if (data.Ticket_Status != null && data.Ticket_Status.ToLower().Contains("close")) {

        // Party is required    
        if (data.Party_ID.ID == 0 && string.IsNullOrEmpty(data.Party_ID.License_No))
          ModelState.AddModelError("Party", "Driving licence number is a required field.");

        // Item is reuired.
        if (!IsLineItemExits(data.ID))
          ModelState.AddModelError("ScaleDetails", "There is at least one line item is required in the item details.");

        // Gross Weight is required.
        if (data.Net_Weight <= 0)
          ModelState.AddModelError("Net_Weight ", "Net weight is a required field.");

        // Amount is required    
        if (data.Item_Amount <= 0)
          ModelState.AddModelError("Amount", "Amount is a required field.");

        // State is rquire
        if (string.IsNullOrEmpty(data.State.Trim()))
          ModelState.AddModelError("State", "State is a required field.");


        // Zip Code is rquire
        if (data.PrimaryAddress != null && string.IsNullOrEmpty(data.PrimaryAddress.Zip_Code))
          ModelState.AddModelError("Zip_Code", "Zip code is a required field.");

        // DOB is required    
        if (data.Party_ID != null && string.IsNullOrEmpty(data.Party_ID.Party_DOB))
          ModelState.AddModelError("Party_DOB", "Party DOB is a required field.");

        // Vehicle Year is required    
        if (string.IsNullOrEmpty(data.Vehicle_Year.Trim()))
          ModelState.AddModelError("Vehicle_Year", "Vehicle Year is a required field.");

        // Vehicle Sate is required    
        if (string.IsNullOrEmpty(data.Plate_State.Trim()))
          ModelState.AddModelError("Plate_State", "Plate State is a required field.");

        //// Amount is required    
        //if (data.Ticket_Settled)
        //    ModelState.AddModelError("Settled", "Ticket already paid.");
      }

      ValidQScaleItems(data);


      if (!ModelState.IsValid) {
        return Json(new {
          success = false,
          errors = ModelState.Errors()
        });

      }
      return Json(new {
        success = true,
        errors = ""
      });
    }

    public override ActionResult _Delete(string id) {
      Scale scale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id);
      if (scale.Ticket_Settled == true)
        throw new Exception("Can not delete paid ticket.");
      return base._Delete(id);
    }

    //protected override void Form_OnAdded(Scale entity)
    //{
    //    base.Form_OnAdded(entity);
    //    AddNewMakeAndModel(entity);
    //}

    //protected override void Form_OnModified(Scale entity)
    //{
    //    base.Form_OnModified(entity);
    //    AddNewMakeAndModel(entity);
    //}

    [HttpGet]
    public JsonResult _ExportTicketToPDF(string id) {
      try {
        Dictionary<string, object> parameter = new Dictionary<string, object>();
        parameter.Add("ScaleID", id);
        SSRSReport ssrsHelper = new SSRSReport("QScale.rdl", parameter);
        string attachment = ssrsHelper.ExportReportToPDF();
        string appPath = ConfigurationHelper.GetsmARTQBIntegrationBatchFilePath();
        string batchFilePath = Path.Combine(appPath, "PrintTicekt.bat");

        string filename = Path.Combine(ConfigurationHelper.GetsmARTPrintFilePath(), id + ".pdf");
        //PDF.PrintPDFs(filename);      
        return Json(new {
          Path = batchFilePath
        }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception) {
        var data = new {
          Sucess = false
        };
        return Json(data, JsonRequestBehavior.AllowGet);
      }
    }

    #region Private Methods

    //private void AddNewMakeAndModel(Scale entity)
    //{
    //    LOVLibrary lib = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //    if (!string.IsNullOrEmpty(entity.Make))
    //    {
    //        // Make does not exists in database.
    //        LOV makeLOV = lib.GetByValue(entity.Make);
    //        if (makeLOV == null)
    //        {
    //            LOVTypeLibrary libType = new LOVTypeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //            LOVType modelLOVType = libType.GetByLOVType("Make");
    //            makeLOV = new LOV()
    //            {
    //                Active_Ind = true,
    //                LOV_Active = true,
    //                Parent_Type_ID = modelLOVType.ID,
    //                LOV_Value = entity.Make,
    //                LOV_Display_Value = entity.Make,
    //                LOVType = modelLOVType,
    //                Parent = new LOV()
    //            };
    //            lib.Add(makeLOV);
    //        }
    //    }

    //    if (!string.IsNullOrEmpty(entity.Model) && !string.IsNullOrEmpty(entity.Make))
    //    {
    //        // Model does not exists in database.
    //        LOV modelLOV = lib.GetByValueAndParent(entity.Model, entity.Make);
    //        if (modelLOV == null)
    //        {
    //            LOVTypeLibrary libType = new LOVTypeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //            LOVType modelLOVType = libType.GetByLOVType("Model");
    //            LOV parentModelLOV = lib.GetByValue(entity.Make);
    //            modelLOV = new LOV()
    //            {
    //                Active_Ind = true,
    //                LOV_Active = true,
    //                Parent_Type_ID = modelLOVType.ID,
    //                LOV_Value = entity.Model,
    //                LOV_Display_Value = entity.Model,
    //                LOVType = modelLOVType,
    //                Parent = parentModelLOV
    //            };
    //            lib.Add(modelLOV);
    //        }
    //    }
    //}

    public bool GetCustomerImageRefIds(int scaleId, out  string thumbImage1, out string thumbImage2, out string photo, out string signatureImage, out string licenseImage, out string vehicleImageRefId, out string cashCardImageRefId) {
      thumbImage1 = thumbImage2 = photo = signatureImage = licenseImage = vehicleImageRefId = cashCardImageRefId = string.Empty;
      ScaleAttachments thumb1Attach = null;
      ScaleAttachments thumb2Attach = null;
      ScaleAttachments photoAttach = null;
      ScaleAttachments signatureAttach = null;
      ScaleAttachments dlLicenseAttach = null;
      ScaleAttachments vehicleImageAttach = null;
      ScaleAttachments cashCardImageAttach = null;

      if (scaleId > 0) {
        ScaleAttachmentsLibrary lib = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        thumb1Attach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.Thumbprint1, scaleId).FirstOrDefault();
        thumb2Attach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.Thumbprint2, scaleId).FirstOrDefault();
        photoAttach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.Customer, scaleId).FirstOrDefault();
        signatureAttach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.Signature, scaleId).FirstOrDefault();
        dlLicenseAttach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.DriverLicense, scaleId).FirstOrDefault();
        vehicleImageAttach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.Vehicle, scaleId).FirstOrDefault();
        cashCardImageAttach = lib.GetAttachmentsByRefTypeAndParentId(EnumAttachmentRefType.CashCard, scaleId).FirstOrDefault();
      }
      else {
        if (Session["ScaleAttachments"] != null) {
          IEnumerable<ScaleAttachments> resultList = (IList<ScaleAttachments>)Session["ScaleAttachments"];
          thumb1Attach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Thumbprint1);
          thumb2Attach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Thumbprint2);
          photoAttach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Customer);
          signatureAttach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Signature);
          dlLicenseAttach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.DriverLicense);
          vehicleImageAttach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Vehicle);
          cashCardImageAttach = resultList.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.CashCard);
        }
      }

      if (thumb1Attach != null)
        thumbImage1 = thumb1Attach.Document_RefId.ToString();

      if (thumb2Attach != null)
        thumbImage2 = thumb2Attach.Document_RefId.ToString();

      if (photoAttach != null)
        photo = photoAttach.Document_RefId.ToString();

      if (signatureAttach != null)
        signatureImage = signatureAttach.Document_RefId.ToString();

      if (dlLicenseAttach != null)
        licenseImage = dlLicenseAttach.Document_RefId.ToString();

      if (vehicleImageAttach != null)
        vehicleImageRefId = vehicleImageAttach.Document_RefId.ToString();

      if (cashCardImageAttach != null)
        cashCardImageRefId = cashCardImageAttach.Document_RefId.ToString();

      return true;
    }
    #endregion Private Methods
  }
}
