using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.Mvc.UI;
using smART.Common;
using smART.MVC.Present.Security;
using Telerik.Web.Mvc;

namespace smART.MVC.Present.Extensions
{
    public class ActionDetails
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Content { get; set; }
        public EnumFeatures Feature { get; set; }
    }

    public static class TelerikMenuItemExtensions
    {
        public static MenuItemBuilder AddSeparator(this MenuItemBuilder item)
        {
            return item.Text("&nbsp;&nbsp;").Encoded(false).Enabled(false);
        }

        public static MenuItemBuilder ActionIf(this MenuItemBuilder item, ActionDetails[] actionDetails)
        {
            SmartPrincipal user = item.ViewContext.HttpContext.User as SmartPrincipal;

            if ((actionDetails != null) && (actionDetails.Length > 0))
            {
                foreach (ActionDetails actionDetail in actionDetails)
                {
                    if (user.IsInFeature(actionDetail.Feature))
                        return item.Action(actionDetail.Action, actionDetail.Controller);
                }
            }

            item.Visible(false);
            return item;
        }

        public static NavigationItemBuilder<TItem, TBuilder> ActionIf<TItem, TBuilder>(this NavigationItemBuilder<TItem, TBuilder> item, ActionDetails[] actionDetails)
            where TItem: NavigationItem<TItem>
            where TBuilder: NavigationItemBuilder<TItem, TBuilder>, IHideObjectMembers
        {
            SmartPrincipal user = item.ViewContext.HttpContext.User as SmartPrincipal;

            if ((actionDetails != null) && (actionDetails.Length > 0))
            {
                foreach (ActionDetails actionDetail in actionDetails)
                {
                    if (user.IsInFeature(actionDetail.Feature))
                        return item.Action(actionDetail.Action, actionDetail.Controller);
                }
            }

            item.Visible(true);
            return item;
        }

        public static NavigationItemBuilder<TItem, TBuilder> ContentIf<TItem, TBuilder>(this NavigationItemBuilder<TItem, TBuilder> item, ActionDetails actionDetail)
            where TItem : NavigationItem<TItem>
            where TBuilder : NavigationItemBuilder<TItem, TBuilder>, IHideObjectMembers
        {
            SmartPrincipal user = item.ViewContext.HttpContext.User as SmartPrincipal;

            if ((actionDetail != null))
            {
                    if (user.IsInFeature(actionDetail.Feature))
                        return item.Content(actionDetail.Content);
            }

            item.Visible(false);
            return item;
        }
    }
}