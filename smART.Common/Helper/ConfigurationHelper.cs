using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace smART.Common
{
    public static class ConfigurationHelper
    {
        public static string GetsmARTDBContextConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["smARTDBContext"].ConnectionString;
        }
        public static string GetsmARTDocPath()
        {
           // return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTDocPath"].ToString());
            return Path.Combine(System.Configuration.ConfigurationManager.AppSettings["smARTDocPath"].ToString(), "Attachments");
        }


        public static string GetsmARTTempDocPath()
        {
            //return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTTempDocPath"].ToString());
            return Path.Combine(System.Configuration.ConfigurationManager.AppSettings["smARTDocPath"].ToString(), "Temp");
        }

        public static string GetsmARTDocUrl()
        {
            return Path.Combine(System.Configuration.ConfigurationManager.AppSettings["smARTDocUrl"].ToString());
        }


        public static int GetsmARTLookupGridPageSize()
        {
            int pageSize = 30;
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("smARTLookupGridPageSize"))
                pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smARTLookupGridPageSize"]);
            return pageSize;
        }

        public static int GetsmARTDetailGridPageSize()
        {
            int pageSize = 5;
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("smARTDetailGridPageSize"))
                pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smARTDetailGridPageSize"]);
            return pageSize;
        }

        public static string GetCameraSourceAddress()
        {
            string source = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("smARTCameraStreamSource"))
                source = System.Configuration.ConfigurationManager.AppSettings["smARTCameraStreamSource"];
            return source;
        }

        public static string GetCameraIPAddress()
        {
            string ipAddress = "192.168.1.1";
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("smARTCameraIPAddress"))
                ipAddress = System.Configuration.ConfigurationManager.AppSettings["smARTCameraIPAddress"];
            return ipAddress;
        }

        public static string GetsmARTReportServerUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTReportServerUrl"].ToString();
        }

        public static string GetsmARTScaleWeightComPort()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightComPort"].ToString();
        }

        public static string GetsmARTScaleWeightBaudRate()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightBaudRate"].ToString();
        }

        public static string GetsmARTScaleWeightDataBits()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightDataBits"].ToString();
        }

        public static string GetsmARTScaleWeightStopBits()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightStopBits"].ToString();
        }

        public static string GetsmARTScaleWeightTimeout()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightTimeout"].ToString();
        }

        public static string GetsmARTScaleWeightLogFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightLogFile"].ToString();
        }

        public static string GetsmARTScaleWeightIPAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightIPAddress"].ToString();
        }

        public static string GetsmARTScaleWeightPort()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightPort"].ToString();
        }

        public static string GetsmARTScaleWeightWaitTime()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightWaitTime"].ToString();
        }

        public static string GetsmARTScaleWeightCommand()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTScaleWeightCommand"].ToString();
        }

        public static string GetsmARTAttachmentImageTypes()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTAttachImageTypes"].ToString();
        }

        public static string GetsmARTXslPath()
        {
            return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTXSLPath"].ToString());
        }

        public static string GetsmARTSMTPServer()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTSMTPServer"].ToString();
        }

        public static string GetsmARTQBIntegrationBatchFilePath()
        {
            return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTQBIntegrationBatchFilePath"].ToString());
        }
        public static string GetsmARTPrintFilePath()
        {
            return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTPrintFilePath"].ToString());
        }
        public static string GetsmARTUtilitiesPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smARTUtilitiesPath"].ToString();
        }     

        public static string GetContentFolderPath()
        {
            //return Path.Combine(@"D:\Rsmart\Rsmart-SY-Project\smART.MVC.Present", "Content");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            //return Path.Combine(@"C:\Rsmart\WebClient", "Content");
        }


        public static string GetDeviceLogFilePath()
        {            
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeviceLogs");            
        }


        public static string GetClerk() {
          return System.Configuration.ConfigurationManager.AppSettings["Clerk"].ToString();
        }


        #region Leads Online
        
        public static string GetLeadsOnlineServiceUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ServiceUrl"].ToString();
        }

        public static string GetLeadsOnlineServiceUser()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ServiceUser"].ToString();
        }

        public static string GetLeadsOnlineServiceUserPwd()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ServiceUserPwd"].ToString();
        }

        public static int GetLeadsOnlineStoreId()
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreId"].ToString());
        }

        #endregion Leads Online


    }
}
