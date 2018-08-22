using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.MVC.ViewModel;
using System.Web.Security;
using smART.ViewModel;
using smART.MVC.Present.Security;
using smART.MVC.Present.Helpers;
using smART.Library;
using smART.Common;

namespace smART.MVC.Present.Controllers {

  [AllowAnonymous]
  public class AccountController : Controller {
    //
    // GET: /Account/LogOn
    [Logon]
    public ActionResult LogOn() {
      return View();
    }

    //
    // POST: /Account/LogOn

    [HttpPost()]
    public ActionResult LogIn(LogOnModel model) {
      if (ModelState.IsValid) {
        if (Membership.ValidateUser(model.UserName, model.Password)) {
          EmployeeHelper employeeHelper = new EmployeeHelper();
          Employee employee = employeeHelper.GetEmployeeByUsername(model.UserName);

          if (employee != null) {
            //IEnumerable<Feature> features = employeeHelper.GetFeaturesForEmployee(employee.ID);
            //IEnumerable<RoleFeature> roleFeatures = employeeHelper.GetRoleFeaturesForEmployee(employee.ID);
            SmartPrincipal principal = new SmartPrincipal() {
              Name = model.UserName,
              ID = employee.ID
            };

            string userData = SmartPrincipal.GetCookieUserData(principal);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            return Content("Logged In Ok");
          }
          //HttpCookie cookie = FormsAuthentication.GetAuthCookie(model.UserName, false);
          //cookie.Expires = DateTime.Now.AddMinutes(20);
          //Request.Cookies.Add(cookie);
          //Request.Cookies.Add(new HttpCookie("Barney", "Rubble"));
          
          //return Content("Logged In Ok");
        }
      }
      return new HttpUnauthorizedResult();

    }


    [HttpPost]
    public ActionResult LogOn(LogOnModel model, string returnUrl) {
      if (ModelState.IsValid) {
        if (Membership.ValidateUser(model.UserName, model.Password)) {
          EmployeeHelper employeeHelper = new EmployeeHelper();
          Employee employee = employeeHelper.GetEmployeeByUsername(model.UserName);
          DeviceSettingLibrary deviceLib = new DeviceSettingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());

          if (employee != null) {

              Session["Site_Org_ID"] = employee.Site_Org_ID.ToString();
              Session["Unique_ID"] =  deviceLib.GetBySiteIdAndDeviceId(employee.Site_Org_ID.Value, "1").Unique_ID;

            //IEnumerable<Feature> features = employeeHelper.GetFeaturesForEmployee(employee.ID);
            //IEnumerable<RoleFeature> roleFeatures = employeeHelper.GetRoleFeaturesForEmployee(employee.ID);
            SmartPrincipal principal = new SmartPrincipal() {
              Name = model.UserName,
              ID = employee.ID
            };

            string userData = SmartPrincipal.GetCookieUserData(principal);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
              return Redirect(returnUrl);
            }
            else {
              return RedirectToAction("Index", "Home");
            }

          }
          else {
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
          }
        }
        else {
          ModelState.AddModelError("", "The user name or password provided is incorrect.");
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }
    //
    // GET: /Account/LogOff

    public ActionResult LogOff() {
      FormsAuthentication.SignOut();
      return RedirectToAction("Index", "Home");
    }

  }
}
