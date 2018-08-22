using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.ViewEngines
{
    public class ReportsViewEngine: RazorViewEngine
    {
        public static string[] NewViewFormats = new[] { "~/Views/Reports/{1}/{0}.cshtml" };
        public ReportsViewEngine()
        {
            base.ViewLocationFormats = base.ViewLocationFormats.Union(NewViewFormats).ToArray();
            base.PartialViewLocationFormats = new string[] { "~/Views/Reports/{1}/{0}.cshtml" }.Union(base.PartialViewLocationFormats).ToArray<string>();
        }
    }
}