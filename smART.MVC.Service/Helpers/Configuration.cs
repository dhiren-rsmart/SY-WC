using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace smART.MVC.Service
{
    public static class Configuration
    {
        public static string GetsmARTDBContextConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["smARTDBContext"].ConnectionString;
        }

        public static string GetsmARTDocPath()
        {
            return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTDocPath"].ToString());
        }
        public static string GetsmARTTempDocPath()
        {
            return Path.Combine(GetContentFolderPath(), System.Configuration.ConfigurationManager.AppSettings["smARTTempDocPath"].ToString());
        }

        public static string GetContentFolderPath()
        {
            // Set Main site folder path here
            //return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            return Path.Combine(@"C:\Rsmart\WebClient", "Content");
            //return Path.Combine(@"D:\Rsmart\Rsmart-SY-Project\smART.MVC.Present", "Content");            
        }

    }
}
