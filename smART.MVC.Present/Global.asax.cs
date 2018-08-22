using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using smART.MVC.Present.ViewEngines;
using smART.MVC.Present.Security;
using System.Web.Security;

namespace smART.MVC.Present {
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
      filters.Add(new LogonAttribute());
      filters.Add(new HandleErrorAttribute());
    }

    public static void RegisterRoutes(RouteCollection routes) {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
          "Default", // Route name
          "{controller}/{action}/{id}", // URL with parameters
          new {
            controller = "Home", action = "Index", id = UrlParameter.Optional
          } // Parameter defaults
      );

      routes.MapRoute(
      "Default2", // Route name
      "{controller}/{action}/{urltoken}/{id}", // URL with parameters
      new {
        controller = "Home", action = "Index", urltoken = UrlParameter.Optional, id = UrlParameter.Optional
      } // Parameter defaults
      );

    }

    protected void Application_Start() {
      AreaRegistration.RegisterAllAreas();

      System.Web.Mvc.ViewEngines.Engines.Add(new MasterViewEngine());
      System.Web.Mvc.ViewEngines.Engines.Add(new TransactionViewEngine());
      System.Web.Mvc.ViewEngines.Engines.Add(new AdministrationViewEngine());
      System.Web.Mvc.ViewEngines.Engines.Add(new ReportsViewEngine());
      System.Web.Mvc.ViewEngines.Engines.Add(new SearchViewEngine());
      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);

      ModelBinders.Binders.DefaultBinder = new ModelBinder.BaseEntityModelBinder();
    }

    private void Application_AuthenticateRequest(Object source, EventArgs e) {
      var application = (HttpApplication) source;
      var context = application.Context;

      // Get the authentication cookie
      string cookieName = FormsAuthentication.FormsCookieName;
      HttpCookie authCookie = context.Request.Cookies[cookieName];
      if (authCookie == null)
        return;

      var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

      context.User = SmartPrincipal.CreatePrincipalFromCookieData(authTicket.UserData);
    }

  }
}