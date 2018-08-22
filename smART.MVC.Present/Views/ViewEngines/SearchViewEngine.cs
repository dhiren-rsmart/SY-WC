using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.ViewEngines {

  public class SearchViewEngine : RazorViewEngine {

    public static string[] NewViewFormats = new[] { "~/Views/Search/{1}/{0}.cshtml" };

    public SearchViewEngine() {
      base.ViewLocationFormats = base.ViewLocationFormats.Union(NewViewFormats).ToArray();
      base.PartialViewLocationFormats = new string[] { "~/Views/Search/{1}/{0}.cshtml" }.Union(base.PartialViewLocationFormats).ToArray<string>();
    }

  }
}