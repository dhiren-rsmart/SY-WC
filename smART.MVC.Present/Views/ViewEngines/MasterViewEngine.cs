using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.ViewEngines
{
    public class MasterViewEngine: RazorViewEngine
    {
        public static string[] NewViewFormats = new[] { "~/Views/Master/{1}/{0}.cshtml" };
        public MasterViewEngine()
        {
            base.ViewLocationFormats = base.ViewLocationFormats.Union(NewViewFormats).ToArray();
            base.PartialViewLocationFormats = new string[] { "~/Views/Master/{1}/{0}.cshtml" }.Union(base.PartialViewLocationFormats).ToArray<string>();
        }
    }
}