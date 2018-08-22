using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using smART.ViewModel;
using smART.Library;

//using smART.Integration.LicScanner;
using System.Net;
using System.Text;
using smART.MVC.Present.Helpers;

using Omu.ValueInjecter;
using System.Drawing;
using System.Drawing.Imaging;
using smART.Common;

namespace smART.MVC.Present.Controllers {

  public class ScannerController : Controller {

    //[HttpGet]
    //public JsonResult _ScanDrivingLicenceID(string id, string state) {
    //  try {
    //    Int32 customerID = 0;
    //    var data = new {
    //      CustomerID = customerID,
    //      CustomerLicenceID = "",
    //      CustomerName = "",
    //      State = "",
    //      LicenseImageRefId = ""
    //    };

    //    using (Scanner scanner = new Scanner()) {
    //      LicenceInfo liceInfo = scanner.ScanDrivingLicence("Texas");
    //      int scaleId = Convert.ToInt32(id);

    //      PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //      Party party = partyLib.GetByLicenseNo(liceInfo.LicenceNo);
    //      if (party != null) {
    //        customerID = party.ID;
    //      }

    //      Guid refID = SaveScaleIDCardAttachment(scaleId, liceInfo.BinaryImage);
    //      data = new {
    //        CustomerID = customerID,
    //        CustomerLicenceID = liceInfo.LicenceNo,
    //        CustomerName = liceInfo.DriverName,
    //        State = liceInfo.State,
    //        LicenseImageRefId = refID.ToString()
    //      };
    //    }
    //    return Json(data, JsonRequestBehavior.AllowGet);
    //  }
    //  catch (Exception ex) {
    //    string message = ex.Message;
    //    //ExceptionFormatter formater = new ExceptionFormatter();
    //    //string formatedException = formater.Format("Scan error", ex.Message);
    //    Common.MessageLogger.Instance.LogMessage(ex, ex.Message, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Critical, "Scanner Error", "Scanner");
    //    throw ex;
    //  }
    //}

    [HttpGet]
    public JsonResult _SavePartyLicenceID(string scaleId, string licenseImageRefId) {
      try {
        bool result = false;  
        if (!string.IsNullOrEmpty(licenseImageRefId)) {
          ScaleAttachmentsLibrary scaleAttOps = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          ScaleAttachments scaleAttachment = scaleAttOps.GetScaleAttachmentByRefId(new Guid(licenseImageRefId));
          if (scaleAttachment != null) {
            FilelHelper fileHelper = new FilelHelper();
            string imagePath = fileHelper.GetFilePathByFileRefId(licenseImageRefId);
            byte[] imageBytes = fileHelper.GetBytesFromFile(imagePath);
            SaveScaleIDCardAttachment(Convert.ToInt32(scaleId), imageBytes);
            result = true;
          }
        }
        var data = new {Success  = result};
        return Json(data, JsonRequestBehavior.AllowGet);
      }
      catch (Exception ex) {
        string message = ex.Message;
        Common.MessageLogger.Instance.LogMessage(ex, ex.Message, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Critical, "Scanner Error", "Scanner");
        throw ex;
      }
    }

    private Guid SaveFile(byte[] data) {
      CameraFile cFile = new CameraFile(new MemoryStream(data), "image/jpeg", "LicenceID-Image.JPEG ");
      List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
      files.Add(cFile);

      Guid guid = Guid.NewGuid();

      Transaction.ScaleAttachmentsController controller = new Transaction.ScaleAttachmentsController();
      controller.Save(files, guid.ToString());
      controller.Dispose();

      return guid;
    }

    public Guid SaveScaleIDCardAttachment(int Id, byte[] data) {
      Guid guid = SaveFile(data);

      ScaleAttachments attachments = new ScaleAttachments();

      attachments.Document_Name = "LicenceID-Image.jpeg";
      attachments.Document_RefId = guid;
      attachments.Document_Size = data.LongLength;
      attachments.Document_Title = "LicenceID-Image";
      attachments.Document_Type = "jpeg";

      attachments.Updated_By = User.Identity.Name;
      attachments.Created_By = User.Identity.Name;
      attachments.Created_Date = DateTime.Now;
      attachments.Last_Updated_Date = DateTime.Now;

      attachments.Parent = new Scale {
        ID = Id
      };

      string destinationPath;
      string sourcePath;

      FilelHelper fileHelper = new FilelHelper();
      destinationPath = fileHelper.GetSourceDirByFileRefId(attachments.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Scale.Document_RefId.ToString());
      sourcePath = fileHelper.GetTempSourceDirByFileRefId(attachments.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Scale.Document_RefId.ToString());

      attachments.Document_Path = fileHelper.GetFilePath(sourcePath);

      if (Id > 0) {
        fileHelper.MoveFile(attachments.Document_Name, sourcePath, destinationPath);
        ScaleAttachmentsLibrary ScaleLibrary = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        ScaleLibrary.Add(attachments);
      }
      else {
        if (Session["ScaleAttachments"] == null)
          Session["ScaleAttachments"] = new List<ScaleAttachments>();
        IList<ScaleAttachments> iList = (IList<ScaleAttachments>) Session["ScaleAttachments"];
        iList.Add(attachments);
      }
      return guid;
    }
  }
}
