using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace smART.MVC.Present.Extensions
{
    public static class HtmlExtensions
    {
        // Extension method
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controller,  object routeValues, string imagePath, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, controller, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }
    }

    public static class ModelStateHelper {
      public static IEnumerable Errors(this ModelStateDictionary modelState) {
        if (!modelState.IsValid) {
          return modelState.ToDictionary(kvp => kvp.Key,
              kvp => kvp.Value.Errors
                              .Select(e => e.ErrorMessage).ToArray())
                              .Where(m => m.Value.Count() > 0);
        }
        return null;
      }
    }

}