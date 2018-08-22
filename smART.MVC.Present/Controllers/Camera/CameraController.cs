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
using Telerik.Web.Mvc;


namespace smART.MVC.Present.Controllers
{
    public class CameraController : Controller
    {
        ICamera camera;

        public CameraController()
        {
            camera = new AxisIPCamera(ConfigurationHelper.GetCameraIPAddress());
        }

        public ActionResult _OpenCamera(EnumCameraInitiator cameraInitiator, string id)
        {
            if (string.IsNullOrEmpty(id)) id = "-1";
            string parameters = string.Format("id={0},InitiatorId={1},cameratype={2}", id, id, 3);
            string xapPath = String.Format("{0}://{1}{2}/ClientBin/smART.MVC.Silverlight.xap", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/")).Replace("//ClientBin", "/ClientBin");
            CameraInput cameraInput = new CameraInput() { InitiatorId = int.Parse(id), CameraInitiator = cameraInitiator, XAPPath = xapPath, SourceAddress = ConfigurationHelper.GetCameraSourceAddress(), ParameterString = parameters };
            return PartialView("~/Views/Integration/Camera/_Camera.cshtml", cameraInput);
        }

        public JsonResult _GetCameraDetails()
        {
            var sessionVideos = new List<string> { "Keynote", "Silverlight Boot Camp", 
                "Fun with ASP.NET MVC 3 and MEF", "ASP.NET MVC 3 @:The Time is Now" };
            return Json(sessionVideos, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void _SaveCameraImage(string id, string data)
        {
            //var todoJson = new StreamReader(Request.InputStream).ReadToEnd();

            if (string.IsNullOrEmpty(id)) id = "-1";
            string decodedData = HttpUtility.UrlDecode(data);

            // Save File as attachment
            byte[] byteData = Convert.FromBase64String(decodedData.Replace(" ", "+"));
            SaveScaleAttachment(int.Parse(id), byteData);

            //if (string.IsNullOrEmpty(id)) id = "-1";
            //string decodedData = HttpUtility.UrlDecode(data);

            //// Save File as attachment
            //byte[] buffer = Convert.FromBase64String(decodedData.Replace(" ", "+"));


            //Bitmap bmp = new Bitmap(320, 240);
            //using (MemoryStream stream = new MemoryStream(buffer))
            //{
            //    bmp = new Bitmap(stream);
            //    string saveString = "c:\\a.jpg";
            //    bmp.Save(saveString, System.Drawing.Imaging.ImageFormat.Jpeg);
            //}


            //    using (FileStream fs = System.IO.File.Create(Server.MapPath("/" + "a.jpeg" )))
            //{
            //    SaveFile(Request.InputStream, fs);
            //}


            //    byte[] image = new byte[Request.InputStream.Length];
            //    Request.InputStream.Read(image, 0, image.Length);

            //    //file.SaveAs("c:\a.jpeg");
            //    //var todoJson = new StreamReader(Request.InputStream).ReadToEnd();

            //if (string.IsNullOrEmpty(id)) id = "-1";
            //string decodedData = HttpUtility.UrlDecode(data);

            //// Save File as attachment
            //byte[] byteData = Convert.FromBase64String(decodedData.Replace(" ", "+"));

            //System.IO.File.WriteAllBytes("c:\\a.jpeg", byteData.ToArray());


            //// Save File as attachment
            //byte[] byteData1 = Convert.FromBase64String(data);

            //System.IO.File.WriteAllBytes("c:\b.jpeg", byteData1.ToArray());

            //SaveScaleAttachment(int.Parse(id), byteData);

            //// Save File as attachment
            //byte[] byteData = Convert.FromBase64String(decodedData.Replace(" ", "+"));
            //SaveScaleAttachment(int.Parse(id), image);
        }

        [HttpGet]
        public JsonResult _SaveCustomerImage(string scaleId, string documentRefId)
        {
            try
            {
                if (!string.IsNullOrEmpty(documentRefId))
                {
                    FilelHelper fileHelper = new FilelHelper();
                    string filePath = fileHelper.GetFilePathByFileRefId(documentRefId);
                    if (System.IO.File.Exists(filePath))
                    {
                        Byte[] photoBytes = fileHelper.GetBytesFromFile(filePath);
                        SaveScaleAttachment(Convert.ToInt32(scaleId), photoBytes, false);
                    }
                }
                return Json(new { Sucess = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        [HttpPost]
        public void _CameraZoom(string zoomFactor)
        {
            camera.Zoom(double.Parse(zoomFactor));
        }

        [HttpPost]
        public void _CameraPanTilt(string panTiltType, string panTiltFactor)
        {
            camera.PanTilt(panTiltType, double.Parse(panTiltFactor));
        }

        [HttpPost]
        public void _SaveSnapShot(string id)
        {
            //byte[] image = new byte[Request.InputStream.Length];
            //Request.InputStream.Read(image, 0, image.Length);

            ////SnapShot snapShot = camera.GetSnapShot();
            //SaveScaleAttachment(int.Parse(id), image);
        }

        public void _GetSnapShot()
        {
            //string newUrl = @"http://108.166.187.35:80/csflybynightinc";
            string newUrl = ConfigurationHelper.GetCameraSourceAddress();
            HttpRequestBase original = this.HttpContext.Request;
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(newUrl);




            newRequest.ContentType = original.ContentType;
            newRequest.Method = original.HttpMethod;
            newRequest.UserAgent = original.UserAgent;

            WebResponse newResponse = newRequest.GetResponse();
            Response.ContentType = newResponse.ContentType;
            //Response.Headers["Host"] = "localhost:3756";
            using (Stream newStream = newResponse.GetResponseStream())
            {
                int bufferLength = 10240;
                byte[] buffer = new byte[bufferLength];
                int bytes = 0;
                int total = 0;
                int bytesReceived = 0;

                while (true)
                {
                    // read next portion from stream
                    if ((bytes = newStream.Read(buffer, total, bufferLength - total)) == 0)
                        break;

                    total += bytes;
                    bytesReceived += bytes;

                }

                Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                MemoryStream result = new MemoryStream();

                bmp.Save(result, ImageFormat.Bmp);
                //SaveFile(result.ToArray());
                //bmp.Save("c:\abcd", ImageFormat.Bmp);

                bmp.Dispose();

                bmp = null;

                Response.Clear();
                Response.ContentType = "image/jpeg";
                byte[] getBuffer = result.ToArray();

                Response.BinaryWrite(getBuffer);

                //Response.End();
            }
        }

        public void _GetVideo()
        {
            string newUrl = @"http://108.166.187.35:80/csflybynightinc";
            HttpRequestBase original = this.HttpContext.Request;
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(newUrl);

            newRequest.ContentType = original.ContentType;
            newRequest.Method = original.HttpMethod;
            newRequest.UserAgent = original.UserAgent;

            WebResponse newResponse = newRequest.GetResponse();
            Response.InjectFrom(newResponse);

            //Response.ContentType = newResponse.ContentType;

            newResponse.GetResponseStream().CopyTo(Response.OutputStream);
        }

        public void _GetVideo2()
        {
            string newUrl = @"http://108.166.187.35:80/csflybynightinc";
            HttpRequestBase original = this.HttpContext.Request;
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(newUrl);

            newRequest.ContentType = original.ContentType;
            newRequest.Method = original.HttpMethod;
            newRequest.UserAgent = original.UserAgent;

            WebResponse newResponse = newRequest.GetResponse();
            Response.ContentType = newResponse.ContentType;
            //Response.Headers["Host"] = "localhost:3756";
            using (Stream newStream = newResponse.GetResponseStream())
            {
                int bufferLength = 10240;
                byte[] buffer = new byte[bufferLength];
                int bytes = 0;
                while ((bytes = newStream.Read(buffer, 0, bufferLength)) > 0)
                {
                    Response.OutputStream.Write(buffer, 0, bytes);
                    Response.Flush();
                }
            }

        }

        public void _OldGetVideo()
        {
            //string newUrl = @"http://www.camstreams.com/asx.asp?user=flybynightinc";
            string newUrl = @"http://www.camstreams.com/asx.asp";
            HttpRequestBase original = this.HttpContext.Request;
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(newUrl);

            newRequest.ContentType = original.ContentType;
            newRequest.Method = original.HttpMethod;
            newRequest.UserAgent = original.UserAgent;

            //byte[] originalStream = ReadToByteArray(original.InputStream, 1024);
            byte[] originalStream = Encoding.ASCII.GetBytes("user=flybynightinc");

            Stream reqStream = newRequest.GetRequestStream();
            WebResponse response = newRequest.GetResponse();

            reqStream.Write(originalStream, 0, originalStream.Length);
            reqStream.Close();

            newRequest.GetResponse();
        }

        public FileResult _GetImage()
        {
            SnapShot snapShot = camera.GetSnapShot();
            return new FileContentResult(snapShot.BinaryImage, "image/jpg");
        }

        private byte[] ReadToByteArray(Stream input, int bufferLength)
        {
            byte[] buffer = new byte[bufferLength];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        
        private void SaveScaleAttachment(int Id, byte[] data,bool updateParty=true )
        {
            Guid guid = SaveFile(data);

            ScaleAttachments attachments = new ScaleAttachments();

            attachments.Document_Name = "Customer-Image.jpeg";
            attachments.Document_RefId = guid;
            attachments.Document_Size = data.LongLength;
            attachments.Document_Title = "Customer-Image";
            attachments.Document_Type = "jpeg";
            attachments.Ref_Type = (int)EnumAttachmentRefType.Customer;

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
                ScaleAttachments scaleAttachment = ScaleLibrary.Add(attachments);

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

        private Guid SaveFile(byte[] data)
        {
            CameraFile cFile = new CameraFile(new MemoryStream(data), "image/jpeg", "Customer-Image.JPEG ");
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
