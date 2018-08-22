using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.ViewEngines
{
    public class TransactionViewEngine: RazorViewEngine
    {
        public static string[] NewViewFormats = new[] { "~/Views/Transaction/{1}/{0}.cshtml" };
        public TransactionViewEngine()
        {
            base.ViewLocationFormats = base.ViewLocationFormats.Union(NewViewFormats).ToArray();
            base.PartialViewLocationFormats = new string[] { "~/Views/Transaction/{1}/{0}.cshtml" }.Union(base.PartialViewLocationFormats).ToArray<string>();
        }
    }
}