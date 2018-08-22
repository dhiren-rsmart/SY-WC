using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smART.MVC.Present.Security
{
    public class LogonAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //skip authorization if this is an ajax call (i.e. action name starts with underscore)
            if (filterContext.ActionDescriptor.ActionName.StartsWith("_"))
                return;

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
            {
                return;
            }

            if (!AccessControl.IsControllerActionAccessible(filterContext.HttpContext.User, filterContext.Controller, filterContext.ActionDescriptor))
                filterContext.Result = new  ViewResult() { ViewName = "~/Views/Common/AccessDenied.cshtml" };
                //filterContext.Result = new  ContentResult() { Content = "Access denied", ContentType = "text/html" };
            else
                base.OnAuthorization(filterContext);
        }
    }
}