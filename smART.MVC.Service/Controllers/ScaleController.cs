using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using smART.Library;
using smART.ViewModel;
using System.Transactions;
using System.IO;
//using Microsoft.WindowsAzure.ServiceRuntime;
using smART.Common;


namespace smART.MVC.Service.Controllers {

  public class ScaleController : BaseController<ScaleLibrary, smART.ViewModel.Scale> {

    public ScaleController() {
    }

    // POST api/values
    [HttpPost]
    public HttpResponseMessage SaveTicket([FromBody]Ticket value) {
      try {
        if (value.Scale == null)
          return Request.CreateResponse(HttpStatusCode.BadRequest);

        // Start transaction.
        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
          IsolationLevel = IsolationLevel.ReadCommitted
        })) {

          smART.ViewModel.Scale newScale = new Scale();

          // Add new party if already exists
          if (!string.IsNullOrEmpty(value.Scale.License_No)) {
            PartyLibrary partyLib = new PartyLibrary(ConString);
            Party party = partyLib.GetByLicenseNo(value.Scale.License_No);
            if (party != null) {
              newScale.Party_ID = party;
            }
            else {
              // Add new party
              party = new Party();
              party.Party_Name = value.Scale.Customer_Name;
              party.Party_Short_Name = value.Scale.Customer_Name;
              party.License_No = value.Scale.License_No;
              party.Party_Type = "Individual";
              party.Created_By = value.Scale.Created_By;
              party.Updated_By = value.Scale.Created_By;
              party.Created_Date = value.Scale.Created_Date;
              party.Last_Updated_Date = value.Scale.Created_Date;
              party.Active_Ind = true;
              party.IsActive = true;
              party.State = !string.IsNullOrEmpty(value.Scale.Customer_State) ? value.Scale.Customer_State.ToString().Trim() : "";
              party.ACLicense_ID = value.Scale.Customer_ACLicense_ID;
              party.Party_DOB = value.Scale.Customer_DOB;
              //string dobString = value.Scale.Customer_DOB;
              //DateTime dobDt;
              //if (smART.Common.DateTimeHelper.IsValidDate(dobString, out dobDt))
              //    party.Party_DOB = dobDt;

              party = partyLib.Add(party);
              newScale.Party_ID = party;

              // Add new Address
              AddressBook address = new AddressBook();
              address.Address1 = value.Scale.Customer_Address;
              address.City = value.Scale.Customer_City;
              address.State = value.Scale.Customer_State;
              address.Country = value.Scale.Customer_Country;
              address.Party = party;
              address.Created_By = value.Scale.Created_By;
              address.Updated_By = value.Scale.Created_By;
              address.Created_Date = value.Scale.Created_Date;
              address.Last_Updated_Date = value.Scale.Created_Date;
              address.Primary_Flag = true;
              address.Active_Ind = true;
              address.Zip_Code = value.Scale.Customer_Zip;
              AddressBookLibrary addressLib = new AddressBookLibrary(ConString);
              addressLib.Add(address);
            }
          }

          // Save Scale
          ScaleLibrary lib = new ScaleLibrary(ConString);
          newScale.Ticket_Status = "Open";
          newScale.QScale = true;
          newScale.Gross_Weight = value.ScaleDetails.Sum(s => s.GrossWeight);
          newScale.Tare_Weight = value.ScaleDetails.Sum(s => s.TareWeight); ;
          newScale.Net_Weight = value.ScaleDetails.Sum(s => s.NetWeight);
          value.Scale.MapServiceEntityToServerEntity(newScale);
          smART.ViewModel.Scale scale = lib.Add(newScale);

          // Save scale detail  
          if (value.ScaleDetails != null) {
            ScaleDetailsLibrary libScaleDetail = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            ItemLibrary libItem = new ItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            foreach (var item in value.ScaleDetails) {
              smART.ViewModel.ScaleDetails newScaleDetails = new smART.ViewModel.ScaleDetails();
              newScaleDetails.Apply_To_Item = libItem.GetByID(item.Item_ID.ToString());
              newScaleDetails.Item_Received = libItem.GetByID(item.Item_ID.ToString());
              newScaleDetails.Scale = scale;
              item.MapServiceEntityToServerEntity(newScaleDetails);
              ScaleDetails scaleDetails = libScaleDetail.Add(newScaleDetails);

              // Set docuent related id if document is related to item
              if (value.ScaleAttachments != null && value.ScaleAttachments.Count > 0) {
                Model.ScaleAttachments modelAttach = value.ScaleAttachments.Where(w => w.Document_RelatedID == item.ID).FirstOrDefault();
                if (modelAttach != null && modelAttach.Document_RelatedTo == 1)
                  modelAttach.Document_RelatedID = scaleDetails.ID;
              }
            }
          }

          // Save Max Ticket ID in Device Settings
          DeviceSettingLibrary deviceLib = new DeviceSettingLibrary(ConString);
          DeviceSettings deviceSettings = deviceLib.GetByUniueID(scale.Unique_ID.Value);
          deviceSettings.MaxTicket_ID = value.Scale.ID;
          deviceLib.Modify(deviceSettings);


          // Save Attachments      
          if (value.ScaleAttachments != null) {
            ScaleAttachmentsLibrary libAttach = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            FilelHelper fileHelper = new FilelHelper();

            foreach (var item in value.ScaleAttachments) {
              smART.ViewModel.ScaleAttachments newScaleAttachment = new smART.ViewModel.ScaleAttachments();

              // Save file
              Guid docRefId = Guid.NewGuid();
              string destinationPath = fileHelper.GetSourceDirByFileRefId(docRefId.ToString());
              fileHelper.MoveFile(item.Document_Title, fileHelper.GetTempSourceDirByFileRefId(item.Document_Path), destinationPath);

              // Save attachment
              newScaleAttachment.Parent = scale;
              newScaleAttachment.Document_RefId = docRefId;
              newScaleAttachment.Document_Path = Path.Combine(destinationPath, item.Document_Title);
              item.MapServiceEntityToServerEntity(newScaleAttachment);
              libAttach.Add(newScaleAttachment);
            }
          }

          // Complete transaction.
          scope.Complete();
        }
        return Request.CreateResponse(HttpStatusCode.OK);
      }
      catch (Exception ex) {
        ExceptionHandler.HandleException(ex, "An error occured in SaveTicket.");
        //string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "SaveTicket", ex.Message, ex.StackTrace.ToString());
        //smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return Request.CreateResponse(HttpStatusCode.InternalServerError);
      }
    }

    [HttpPost]
    public string SaveTicketImage(string fileName) {
      var docfiles = new List<string>();
      try {
        var httpRequest = HttpContext.Current.Request;
        if (httpRequest.Files.Count > 0) {
          FilelHelper fileHelper = new FilelHelper();
          foreach (string file in httpRequest.Files) {
            string docRefID = Guid.NewGuid().ToString();
            var destinationPath = fileHelper.GetTempSourceDirByFileRefId(docRefID); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefID);        
            fileHelper.CreateDirectory(destinationPath);
            var postedFile = httpRequest.Files[file];
            postedFile.SaveAs(Path.Combine(destinationPath, fileName));
            docfiles.Add(docRefID);
          }
        }
      }
      catch (Exception ex) {
        ExceptionHandler.HandleException(ex, "An error occured in SaveTicketImage from device.");

        //string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "SaveTicketImage", ex.Message, ex.StackTrace.ToString());
        //smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
      return docfiles.FirstOrDefault();
    }

    [HttpPost]
    public string SaveTicketImage(String scaleId, string documentType) {
      var docfiles = new List<string>();
      try {

        int intScaleId = Convert.ToInt32(scaleId);
        if (intScaleId <= 0)
          throw new Exception("Invalid ticket id.");

        int intDocType = Convert.ToInt32(documentType);
        string fileName = CommonHelper.GetFileNameByDocType(intDocType);
        if (string.IsNullOrEmpty(fileName))
          throw new Exception("Invalid document type.");


        var httpRequest = HttpContext.Current.Request;
        if (httpRequest.Files.Count > 0) {
          // Get scale                    
          Scale scale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(scaleId, new string[] { "Party_ID" });
          if (scale == null)
            throw new Exception("Given scale id not found in database.");

          FilelHelper fileHelper = new FilelHelper();
          foreach (string file in httpRequest.Files) {
            // Save file 
            string docRefID = Guid.NewGuid().ToString();
            var destinationPath = fileHelper.GetSourceDirByFileRefId(docRefID); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefID);        
            fileHelper.CreateDirectory(destinationPath);
            var postedFile = httpRequest.Files[file];
            postedFile.SaveAs(Path.Combine(destinationPath, fileName));
            docfiles.Add(docRefID);

            // Add attachment
            ScaleAttachments attachments = new ScaleAttachments();
            attachments.Document_Name = fileName;
            attachments.Document_RefId = new Guid(docRefID);
            attachments.Document_Size = 0;
            attachments.Document_Title = fileName;
            attachments.Document_Type = "jpeg";
            attachments.Ref_Type = intDocType;

            attachments.Updated_By = User.Identity.Name;
            attachments.Created_By = User.Identity.Name;
            attachments.Created_Date = DateTime.Now;
            attachments.Last_Updated_Date = DateTime.Now;
            attachments.Parent = new Scale {
              ID = intScaleId
            };
            attachments.Document_Path = destinationPath;

            ScaleAttachmentsLibrary ScaleLibrary = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            attachments.Document_Name = fileName;
            ScaleAttachments scaleAttachment = ScaleLibrary.Add(attachments);

            // Update file ref in party master
            if (scale.Party_ID != null && scale.Party_ID.ID > 0) {
              PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              Party party = partyLib.GetByID(scale.Party_ID.ID.ToString());
              if (party != null) {
                SetPartyImageRefByDocType(intDocType, scaleAttachment.Document_RefId.ToString(), party);
                partyLib.Modify(party);
              }
            }

          }
        }
      }
      catch (Exception ex) {
        //string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "SaveTicketImage", ex.Message, ex.StackTrace.ToString());
        ExceptionHandler.HandleException(ex, "An error occured in SaveTicketImage utils.");
        //smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
      return docfiles.FirstOrDefault();
    }

    private void SetPartyImageRefByDocType(int docType, String docRefId, Party party) {
      switch (docType) {
        case (int)EnumAttachmentRefType.Customer:
          party.PhotoRefId = docRefId;
          break;
        case (int)EnumAttachmentRefType.Thumbprint1:
          party.ThumbImage1RefId = docRefId;
          break;
        case (int)EnumAttachmentRefType.Thumbprint2:
          party.ThumbImage2RefId = docRefId;
          break;
        case (int)EnumAttachmentRefType.Signature:
          party.SignatureImageRefId = docRefId;
          break;
        case (int)EnumAttachmentRefType.DriverLicense:
        case (int)EnumAttachmentRefType.License:
          party.LicenseImageRefId = docRefId;
          break;
        case (int)EnumAttachmentRefType.CashCard:
          party.CashCardImageRefId = docRefId;
          break;
        default:
          break;
      }
    }
  }
}
