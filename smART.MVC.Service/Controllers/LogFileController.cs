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


namespace smART.MVC.Service.Controllers
{

    public class LogFileController : ApiController
    {
        [HttpPost]
        public string SaveLogFile(string fileName, string deviceId)
        {
            string deviceFileName = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    FilelHelper fileHelper = new FilelHelper();
                    foreach (string file in httpRequest.Files)
                    {
                        string filePath = ConfigurationHelper.GetDeviceLogFilePath();
                        fileHelper.CreateDirectory(filePath);
                        var postedFile = httpRequest.Files[file];
                        deviceFileName = string.Format("{0}_{1}", deviceId, fileName);
                        if (File.Exists(Path.Combine(filePath, deviceFileName)))
                            fileHelper.DeleteFile(Path.Combine(filePath, deviceFileName));
                        postedFile.SaveAs(Path.Combine(filePath, deviceFileName));
                    }
                }
            }
            catch (Exception ex)
            {
                string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "SaveTicketImage", ex.Message, ex.StackTrace.ToString());
                smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
            }
            return deviceFileName;
        }
    }
}