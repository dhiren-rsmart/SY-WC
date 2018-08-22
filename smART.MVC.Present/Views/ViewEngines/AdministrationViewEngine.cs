using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.ViewEngines
{
    public class AdministrationViewEngine: RazorViewEngine
    {
        public static string[] NewViewFormats = new[] { "~/Views/Administration/{1}/{0}.cshtml" };
        public AdministrationViewEngine()
        {
            base.ViewLocationFormats = base.ViewLocationFormats.Union(NewViewFormats).ToArray();
            base.PartialViewLocationFormats = new string[] { "~/Views/Administration/{1}/{0}.cshtml" }.Union(base.PartialViewLocationFormats).ToArray<string>();
        }
    }
}