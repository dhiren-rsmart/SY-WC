using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using smART.ViewModel;
using smART.Library;

using smART.Integration.Camera;
using smART.Integration.Camera.IPCamera;
using smART.Integration.Camera.IPCamera.Axis;
using System.Net;
using System.Text;
using smART.MVC.Present.Helpers;

using Omu.ValueInjecter;
using System.Drawing;
using System.Drawing.Imaging;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    public class ThumbScannerController : Controller
    {

        public enum FileFormat
        {
            raw,
            jpg
        }

        public ActionResult _Scan(string id)
        {
            return PartialView("~/Views/Integration/ThumbScanner/_ThumbScanner.cshtml", id);
        }


        [HttpPost]
        public void _SaveThumbImages(int id, string image1, string image2)
        {
            SaveThumbImages(id, image1, image2, FileFormat.raw);
        }

        [HttpGet]
        public JsonResult _SaveCustomerThumbImage(string scaleId, string documentRefId1, string documentRefId2)
        {
            try
            {
                // Save thumb image1
                if (!string.IsNullOrEmpty(documentRefId1))
                {
                    FilelHelper fileHelper = new FilelHelper();
                    string filePath = fileHelper.GetFilePathByFileRefId(documentRefId1);
                    if (System.IO.File.Exists(filePath))
                    {
                        Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                        SaveThumbScannerAttachment(Convert.ToInt32(scaleId), photoBytes, "Thumb-Image1", EnumAttachmentRefType.Thumbprint1, FileFormat.jpg, false);
                    }
                }

                // Save thumb image2
                if (!string.IsNullOrEmpty(documentRefId2))
                {
                    FilelHelper fileHelper = new FilelHelper();
                    string filePath = fileHelper.GetFilePathByFileRefId(documentRefId2);
                    if (System.IO.File.Exists(filePath))
                    {
                        Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                        SaveThumbScannerAttachment(Convert.ToInt32(scaleId), photoBytes, "Thumb-Image2", EnumAttachmentRefType.Thumbprint2, FileFormat.jpg, false);
                    }
                }
                return Json(new { Sucess = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public void SaveThumbImages(int id, string image1, string image2, FileFormat fileFormat)
        {
            if (!string.IsNullOrEmpty(image1))
                SaveThumbScannerAttachment(id, Encoding.ASCII.GetBytes(image1), "Thumb-Image1", EnumAttachmentRefType.Thumbprint1, fileFormat);
            if (!string.IsNullOrEmpty(image2))
                SaveThumbScannerAttachment(id, Encoding.ASCII.GetBytes(image2), "Thumb-Image2", EnumAttachmentRefType.Thumbprint2, fileFormat);
        }
        private void SaveThumbScannerAttachment(int Id, byte[] data, string fileName, EnumAttachmentRefType refType, FileFormat fileFormat, bool updateParty = true)
        {
            Guid guid = SaveFile(data, string.Format("{0}.{1}", fileName, fileFormat.ToString()));

            ScaleAttachments attachments = new ScaleAttachments();
            attachments.Document_Name = string.Format("{0}.{1}", fileName, fileFormat.ToString());// "Thumb-Image.jpeg";
            attachments.Document_RefId = guid;
            attachments.Document_Size = data.LongLength;
            attachments.Document_Title = "Thumb-Image";
            attachments.Document_Type = "jpeg";
            attachments.Ref_Type = (int)refType;

            attachments.Updated_By = User.Identity.Name;
            attachments.Created_By = User.Identity.Name;
            attachments.Created_Date = DateTime.Now;
            attachments.Last_Updated_Date = DateTime.Now;
            attachments.Parent = new Scale { ID = Id };

            string destinationPath;
            string sourcePath;

            FilelHelper fileHelper = new FilelHelper();
            destinationPath = fileHelper.GetSourceDirByFileRefId(attachments.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Scale.Document_RefId.ToString());
            sourcePath = fileHelper.GetTempSourceDirByFileRefId(attachments.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Scale.Document_RefId.ToString());

            attachments.Document_Path = fileHelper.GetFilePath(sourcePath);

            if (Id > 0)
            {
                fileHelper.MoveFile(attachments.Document_Name, sourcePath, destinationPath);
                ScaleAttachmentsLibrary ScaleLibrary = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                attachments.Document_Name = string.Format("{0}.{1}", fileName, "jpg");
                ScaleAttachments scaleAttachment =  ScaleLibrary.Add(attachments);

                if (updateParty)
                {
                    Scale scale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(Id.ToString(), new string[] { "Party_ID" });
                    PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                    Party party = partyLib.GetByID(scale.Party_ID.ID.ToString());
                    if (party != null)
                    {
                        party.PhotoRefId = scaleAttachment.Document_RefId.ToString();
                        partyLib.Modify(party);
                    }
                }
            }
            else
            {
                if (Session["ScaleAttachments"] == null)
                    Session["ScaleAttachments"] = new List<ScaleAttachments>();

                IList<ScaleAttachments> iList = (IList<ScaleAttachments>)Session["ScaleAttachments"];
                iList.Add(attachments);
            }

        }

        private Guid SaveFile(byte[] data, string fileName)
        {
            CameraFile cFile = new CameraFile(new MemoryStream(data), "image/jpeg", fileName);
            List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
            files.Add(cFile);

            Guid guid = Guid.NewGuid();

            Transaction.ScaleAttachmentsController controller = new Transaction.ScaleAttachmentsController();
            controller.Save(files, guid.ToString());
            controller.Dispose();
            return guid;
        }

    }
}