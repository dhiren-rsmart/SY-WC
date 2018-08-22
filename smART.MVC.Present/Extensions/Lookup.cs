using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Telerik.Web.Mvc;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Telerik.Web.Mvc.UI;
using System.Text;
using System.Linq.Expressions;
using smART.MVC.Present.Helpers;
using smART.Common;

namespace smART.MVC.Present.Extensions {
  public static class Lookup {
    public static MvcHtmlString LookupHtml<TModel>(this HtmlHelper<TModel> html, string idFieldName, string textFieldName, string actionName, string controllerName, string jsonActionName, string jsonControllerName) where TModel : class {
      ViewDataDictionary<TModel> ViewData = html.ViewData;

      StringBuilder sbFields = new StringBuilder();
      UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

      foreach (ModelMetadata m in ViewData.ModelMetadata.Properties) {
        sbFields.Append(html.Hidden(m.PropertyName, typeof(TModel).GetProperty(m.PropertyName).GetValue(ViewData.Model, null), new { @id = string.Format("HiddenID_{0}{1}", ViewData.ModelMetadata.PropertyName, m.PropertyName) }).ToHtmlString());
      }
      sbFields.Append(string.Format("<span id=\"LabelID_{0}\">{1}</span>", ViewData.ModelMetadata.PropertyName, typeof(TModel).GetProperty(textFieldName).GetValue(ViewData.Model, null) ?? "[Display Text]"));
      sbFields.Append(string.Format("<input type=\"button\" value=\"...\" class=\"t-button\" onclick=\"lookupOpenWindow('#SearchWindow_{0}')\" />", ViewData.ModelMetadata.PropertyName));

      sbFields.Append(
          html.Telerik().Window().BuildWindow("SearchWindow_" + ViewData.ModelMetadata.PropertyName)
              .Modal(true)
              .Content(
                  html.Telerik().Grid<TModel>()
                      .Name("SearchWindowGrid_" + ViewData.ModelMetadata.PropertyName)
                       .Selectable()
                      .Columns(columns => {
                        foreach (ModelMetadata m in ViewData.ModelMetadata.Properties) {
                          if (m.ShowForDisplay)
                            columns.Bound(m.PropertyName);
                        }
                        columns.Bound(idFieldName)
                            .ClientTemplate("<a href='#' onClick='onMasterSelect(<#= " + idFieldName + " #>, \"#HiddenID_" + ViewData.ModelMetadata.PropertyName + "\", \"#LabelID_" + ViewData.ModelMetadata.PropertyName + "\", \"" + urlHelper.Action(jsonActionName, jsonControllerName) + "\", \"#SearchWindow_" + ViewData.ModelMetadata.PropertyName + "\")'>Select</a>").Title("Select");
                      })
                      .EnableCustomBinding(true)
                      .Pageable(paging => paging.PageSize(ConfigurationHelper.GetsmARTLookupGridPageSize())
                      .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                      .Total(100))
                      .DataKeys(keys => keys.Add(idFieldName))
                      .DataBinding(binding => binding.Ajax().Select(actionName, controllerName))
                      .ClientEvents(events => events.OnLoad("SetDefaultFilterToContains")).ToHtmlString()
                  )
              .Visible(false).ToHtmlString()
          );

      MvcHtmlString htmlString = MvcHtmlString.Create(sbFields.ToString());

      return htmlString;
    }

    public static MvcHtmlString LookupHtml<TModel, TValue>(this HtmlHelper<TModel> html,
        Expression<Func<TModel, TValue>> expression,
        string controlName, string idFieldName, string textFieldName,
        string actionName, string controllerName, object routeValues,
        string jsonActionName, string jsonControllerName,
        string[] columns = null, string onChange = null, bool isVisible = true, Dictionary<string, string> columnNames = null)
      where TModel : class
      where TValue : class {
      ViewDataDictionary<TModel> ViewData = html.ViewData;
      ModelMetadata modelMetaData = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, ViewData);

      StringBuilder sbFields = new StringBuilder();
      UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

      sbFields.Append(html.Hidden(controlName, new { @id = controlName }).ToHtmlString());
      foreach (ModelMetadata m in modelMetaData.Properties) {

        if (m.PropertyName != "Created_Date" && m.PropertyName != "Last_Updated_Date") {
          sbFields.Append(html.Hidden(string.Format("{0}.{1}", modelMetaData.PropertyName, m.PropertyName), modelMetaData.Model == null ? "" : typeof(TValue).GetProperty(m.PropertyName).GetValue(modelMetaData.Model, null), new { @id = string.Format("HiddenID_{0}{1}", modelMetaData.PropertyName, m.PropertyName) }).ToHtmlString());
        }
      }
      String displayMode = (isVisible) ? "inline" : "none";
      sbFields.Append(html.TextBox(string.Format("LabelID_{0}", modelMetaData.PropertyName), modelMetaData.Model == null ? "" : typeof(TValue).GetProperty(textFieldName).GetValue(modelMetaData.Model, null), new { @readonly = "readonly", style = "display:" + displayMode + ";" }));
      sbFields.Append(string.Format("<input type=\"button\" value=\"...\" class=\"t-button\" onclick=\"lookupOpenWindow('#SearchWindow_{0}','#SearchWindowGrid_{0}')\" />", modelMetaData.PropertyName));

      sbFields.Append(
          html.Telerik().Window().BuildWindow("SearchWindow_" + modelMetaData.PropertyName)
              .Modal(true).Resizable().Draggable(true)
              .Content(
                  html.Telerik().Grid<TValue>()
                      .Name("SearchWindowGrid_" + modelMetaData.PropertyName)
                       .Selectable()
                      .Columns(cols => {
                        if (columns == null) {
                          foreach (ModelMetadata m in modelMetaData.Properties) {
                            if (m.ShowForDisplay) {
                              if (columnNames != null && columnNames.Count > 0 && columnNames.ContainsKey(m.PropertyName))
                                cols.Bound(m.PropertyName).Filterable(true).Title(columnNames[m.PropertyName]);
                              else
                                cols.Bound(m.PropertyName).Filterable(true);
                            }
                          }

                        }
                        else {
                          foreach (string column in columns) {
                            if (columnNames != null && columnNames.Count > 0 && columnNames.ContainsKey(column))
                              cols.Bound(column).Filterable(true).Title(columnNames[column]);
                            else
                              cols.Bound(column).Filterable(true);
                          }
                        }
                        cols.Bound(idFieldName).Filterable(false)
                            .ClientTemplate("<a href='#' onClick='onLookupMasterSelect(<#= " + idFieldName + " #>, \"#HiddenID_" + modelMetaData.PropertyName + "\", \"#LabelID_" + modelMetaData.PropertyName + "\", \"" + urlHelper.Action(jsonActionName, jsonControllerName) + "\", \"#SearchWindow_" + modelMetaData.PropertyName + "\", \"" + textFieldName + "\", \"" + (onChange ?? "") + "\")'>Select</a>").Title("Select");
                      })
                      .DataKeys(keys => keys.Add(idFieldName))
                      .DataBinding(binding => binding.Ajax().Select(actionName, controllerName, routeValues))
                      .Pageable(paging => paging.PageSize(ConfigurationHelper.GetsmARTLookupGridPageSize()).Enabled(true).Total(100))
                      .ClientEvents(events => events.OnDataBinding("LookupGrid_onDataBinding").OnLoad("SetDefaultFilterToContains"))
                      //.ToolBar(commands => commands.Custom().HtmlAttributes(new { id = "refresh", OnClientClick = string.Format("refreshlookupWindow('SearchWindowGrid_{0}',{1},{2},{3})",modelMetaData.PropertyName,controllerName,actionName,routeValues) }).Text("Refresh"))
                     .Filterable()
                     .ToHtmlString()
                  )
              .ClientEvents(events => events.OnClose("closeWindow"))
              .Visible(false).ToHtmlString()

          );

      MvcHtmlString htmlString = MvcHtmlString.Create(sbFields.ToString());

      return htmlString;
    }


    //public static MvcHtmlString SearchButtonHtml(this HtmlHelper html, string controllerName, string actionName, object routeValues,
    //  string windowTitle,
    //  string windowName, 
    //  string buttonText,
    //  bool isVisible = true) {
    //  StringBuilder sbFields = new StringBuilder();
    //  UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);   
    //  String displayMode = (isVisible) ? "inline" : "none";
    //  sbFields.Append(string.Format("<input type=\"button\" value=\"{0}\" class=\"t-button\" onclick=\"OpenSearchWindow('#SearchWindow_{1}','{2}','{3}',{4})\" />", buttonText, windowName, controllerName ,actionName , routeValues));
    //  sbFields.Append(
    //      html.Telerik().Window().BuildWindow("SearchWindow_" + windowName)
    //          .Modal(true).Resizable().Draggable(true)             
    //          .Width(950)
    //          .Height(440)
    //          .Title(windowTitle)  
    //          .Buttons(buttons => buttons.Maximize().Close())
    //          .ClientEvents(events => events.OnClose("closeWindow"))
    //          .Visible(false).ToHtmlString()
    //      );

    //  MvcHtmlString htmlString = MvcHtmlString.Create(sbFields.ToString());
    //  return htmlString;
    //}
  }
}