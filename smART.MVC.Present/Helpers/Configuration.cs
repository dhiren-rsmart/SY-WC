using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace smART.Common
{
    public static class Configuration
    {
        public static string GetsmARTDBContextConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["smARTDBContext"].ConnectionString;
        }
        public static string GetsmARTDocPath()
        {
           return Path.Combine(GetContentFolderPath(),  System.Configuration.ConfigurationManager.AppSettings["smARTDocPath"].ToString());
        }
        public static string GetsmARTTempDocPath()
        {
             return Path.Combine(GetContentFolderPath(),  System.Configuration.ConfigurationManager.AppSettings["smARTTempDocPath"].ToString());
        }

        public static int GetsmARTLookupGridPageSize()
        {
            int pageSize = 30;
            if ( System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("smARTLookupGridPageSize"))
             pageSize = Convert.ToInt16(  System.Configuration.ConfigurationManager.AppSettings["smARTLookupGridPageSize"]);
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
        public static string GetsmARTReportServerUrl() {
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

        public static string GetsmARTAttachmentImageTypes() {
          return System.Configuration.ConfigurationManager.AppSettings["smARTAttachImageTypes"].ToString();
        }

        public static string GetsmARTXslPath() {
          return   Path.Combine(GetContentFolderPath(),  System.Configuration.ConfigurationManager.AppSettings["smARTXSLPath"].ToString());
        }

        public static string GetsmARTSMTPServer() {
          return System.Configuration.ConfigurationManager.AppSettings["smARTSMTPServer"].ToString();
        }

        public static string GetsmARTQBIntegrationBatchFilePath() {
          return   Path.Combine(GetContentFolderPath(),  System.Configuration.ConfigurationManager.AppSettings["smARTQBIntegrationBatchFilePath"].ToString());
        }
        public static string GetsmARTPrintFilePath() {
          return   Path.Combine(GetContentFolderPath(),  System.Configuration.ConfigurationManager.AppSettings["smARTPrintFilePath"].ToString());
        }
        public static string GetsmARTAdobeAppPath() {
          return  System.Configuration.ConfigurationManager.AppSettings["smARTAdobeAppPath"].ToString();
        }

        public static string GetsmARTOrgID() {
          return System.Configuration.ConfigurationManager.AppSettings["OrgID"].ToString();
        }

        public static string GetsmARTDeviceID() {
          return System.Configuration.ConfigurationManager.AppSettings["DeviceID"].ToString();
        }

        public static string GetContentFolderPath()
        {
            return Path.Combine(@"C:\Rsmart\WebClient", "Content");
        }
    }
}
