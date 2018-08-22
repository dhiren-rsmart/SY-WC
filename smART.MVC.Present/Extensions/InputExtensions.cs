using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using smART.Common;
using smART.MVC.Present.Security;

namespace smART.MVC.Present.Extensions
{
    public static class InputExtensions
    {
        public static MvcHtmlString AuthorizedButton(this HtmlHelper html, string Id, string value, string type = "button", object htmlAttributes = null, EnumFeatures feature= EnumFeatures.All, EnumActions action= EnumActions.All)
        {
            bool isVisible = true;
            bool isEditable = true;

            //TODO: Check for access control
            if (action != EnumActions.All)
            {
                isVisible = AccessControl.IsActionAccessible(html.ViewContext.HttpContext.User as SmartPrincipal, feature, action);
                //Visable then check readonly 
                // if Readonly then isEditable =false.
            }

            TagBuilder builder = new TagBuilder("input");
            builder.GenerateId(Id);
            builder.MergeAttribute("value", value);
            builder.MergeAttribute("type", type);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            if(!isVisible) builder.MergeAttribute("style", "display: none;");
            if(!isEditable) builder.MergeAttribute("disabled", "disabled");
           builder.MergeAttribute("background-image" ,"url('/Content/Images/weighing.png')");

            // Render tag
            return  MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}