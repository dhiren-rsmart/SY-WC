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
    public class UtilityController : Controller
    {
        public enum FileFormat
        {
            jpeg,
            jpg,
            bmp
        }
        
        [HttpGet]
        public JsonResult _SaveCustomerImages(string scaleId, string partyId)
        {
            try
            {                
                if (!string.IsNullOrEmpty(partyId))
                {
                    // Get party
                    PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                    smART.ViewModel.Party party = partyLib.GetByID(partyId);
                    if (party != null)
                    {
                        FilelHelper fileHelper = new FilelHelper();
                        string filePath;
                        // Save Customer Image
                        if (!string.IsNullOrEmpty(party.PhotoRefId))
                        {
                            filePath = fileHelper.GetFilePathByFileRefId(party.PhotoRefId);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                                SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.Customer), EnumAttachmentRefType.Customer, FileFormat.jpeg);
                            }
                        }
                        // Signature Image
                        if (!string.IsNullOrEmpty(party.SignatureImageRefId))
                        {
                            filePath = fileHelper.GetFilePathByFileRefId(party.SignatureImageRefId);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                                SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.Signature), EnumAttachmentRefType.Signature, FileFormat.bmp);
                            }
                        }
                        // Thumb Image
                        if (!string.IsNullOrEmpty(party.ThumbImage1RefId))
                        {
                            filePath = fileHelper.GetFilePathByFileRefId(party.ThumbImage1RefId);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                                SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.Thumbprint1), EnumAttachmentRefType.Thumbprint1, FileFormat.jpg);
                            }
                        }
                        // License Image
                        if (!string.IsNullOrEmpty(party.LicenseImageRefId))
                        {
                            filePath = fileHelper.GetFilePathByFileRefId(party.LicenseImageRefId);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                                SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.DriverLicense), EnumAttachmentRefType.DriverLicense, FileFormat.jpg);
                            }
                        }

                        // Vehicle Image
                        if (!string.IsNullOrEmpty(party.VehicleImageRegId))
                        {
                            filePath = fileHelper.GetFilePathByFileRefId(party.VehicleImageRegId);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                                SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.Vehicle), EnumAttachmentRefType.Vehicle, FileFormat.jpg);
                            }
                        }

                        // Cash Card Image
                        if (!string.IsNullOrEmpty(party.CashCardImageRefId)) {
                          filePath = fileHelper.GetFilePathByFileRefId(party.CashCardImageRefId);
                          if (System.IO.File.Exists(filePath)) {
                            Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                            SaveAttachment(Convert.ToInt32(scaleId), photoBytes, CommonHelper.GetFileNameByDocType((int)EnumAttachmentRefType.CashCard), EnumAttachmentRefType.CashCard, FileFormat.jpg);
                          }
                        }
                    }                 
                   
                }              
                return Json(new { Sucess = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveAttachment(int Id, byte[] data, string fileName, EnumAttachmentRefType refType, FileFormat fileFormat)
        {
            Guid guid = SaveFile(data, fileName);

            ScaleAttachments attachments = new ScaleAttachments();
            attachments.Document_Name = fileName;// "Thumb-Image.jpeg";
            attachments.Document_RefId = guid;
            attachments.Document_Size = data.LongLength;
            attachments.Document_Title = fileName;
            attachments.Document_Type = fileFormat.ToString();
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
                attachments.Document_Name = fileName;
                ScaleAttachments scaleAttachment = ScaleLibrary.Add(attachments);
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