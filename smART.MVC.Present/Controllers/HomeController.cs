using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Exempt)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Recycle smART";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Help() {
          return View();
        }

    }
}
