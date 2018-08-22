using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.Mvc.UI.Fluent;
using System.Reflection;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using smART.ViewModel;
using smART.Common;
using smART.MVC.Present.Security;
using System.Linq.Expressions;
using System.Web.Routing;

namespace smART.MVC.Present.Extensions {

  public static class TelerikExtensions {
    //Created By DB.
    public static GridBuilder<T> BuildGrid<T>(
        this GridBuilder<T> gridbuilder,
        string gridName,
        string ajaxController,
        string indexID, string[] hiddenColumns = null, bool allowInsert = true, bool allowEdit = true, bool allowDelete = true, EnumFeatures feature = EnumFeatures.Exempt) where T : class {
      bool isEditable = true;
      bool isVisible = true;

      allowEdit &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Edit);
      allowInsert &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Add);
      allowDelete &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Delete);

      GridBuilder<T> retVal = gridbuilder.Name(gridName).DataKeys(k => k.Add("ID"))
                              .Scrollable(sc => sc.Height("*"))
                              .Resizable(rs => rs.Columns(true))
                             .EnableCustomBinding(true)
                             .Pageable(paging => paging.PageSize(ConfigurationHelper.GetsmARTDetailGridPageSize())
                                 .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                                 .Total(100))
                             .DataBinding(bindings => bindings.Ajax()
                                 .Select("_Index", ajaxController, new { id = indexID })
                                 .Insert("_Insert", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
                                 .Update("_Update", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
                                 .Delete("_Delete", ajaxController, new { MasterID = indexID, isNew = (indexID.Equals("0") ? true : false) }));


      if (isEditable && isVisible) {
        if (allowInsert) {
          retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
        }
        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true)));
      }
      else
        retVal.Editable(editing => editing.Enabled(false));

      Type typeT = typeof(T);
      PropertyInfo[] properties = typeT.GetProperties();

      //// Add keys
      ////IList<Telerik.Web.Mvc.UI.IGridDataKey> keys = new List<Telerik.Web.Mvc.UI.IGridDataKey>();
      //GridDataKeyFactory<T> dataKeys = new GridDataKeyFactory<T>(retVal.DataKeys (),false);
      //foreach (PropertyInfo property in properties)
      //  if (IsAttributePresent<KeyAttribute>(property))
      //    dataKeys.Add(property.Name);

      // Add Columns
      GridColumnFactory<T> columns = new GridColumnFactory<T>(retVal);
      foreach (PropertyInfo property in properties) {
        bool IsColumnVisible = !IsAttributePresent<HiddenInputAttribute>(property);
        if (hiddenColumns != null && hiddenColumns.Contains(property.Name)) {
          continue;
        }
        GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name).Visible(IsColumnVisible);

        //Set numeric column to right align.
        if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(double)) {

          var dispFormatAttribute = (DisplayFormatAttribute)property.GetCustomAttributes(false)
                         .Where(a => a.GetType() == typeof(DisplayFormatAttribute)).FirstOrDefault();
          string dispFormat = "{0:0.00}";
          if (dispFormatAttribute != null) {
            dispFormat = dispFormatAttribute.DataFormatString;
          }
          gridBoundBuilder.HtmlAttributes(new { style = "text-align: right;" }).Format(dispFormat);
        }
        if (IsAttributePresent<ClientTemplateHtmlAttribute>(property)) {
          gridBoundBuilder.ClientTemplate(property.TemplateHtmlForProperty());
        }
      }

      //Added By Dharmendra ason 26Dec2011
      //if (hiddenColumns!=null)
      //{
      //    foreach (PropertyInfo property in properties)
      //        if (hiddenColumns.Contains(property.Name))
      //        {
      //            GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name);
      //            gridBoundBuilder.Column.Hidden = true;
      //        }
      //}


      if (isEditable && isVisible)
        columns.Command(commands => {
          if (allowEdit) {
            commands.Edit().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          }
          if (allowDelete) {
            commands.Delete().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          }

        }
        );

      return retVal;
    }


    public static GridBuilder<T> BuildGrid<T>(
        this GridBuilder<T> gridbuilder,
        string gridName,
        string ajaxController,
        Action<GridDataKeyFactory<T>> dataKeys,
        Action<GridColumnFactory<T>> columns = null) where T : class {
      bool isEditable = true;
      bool isVisible = true;

      GridBuilder<T> retVal =
                  gridbuilder.Name(gridName)
                      .DataKeys(dataKeys)
                      .EnableCustomBinding(true)
                      .Pageable(paging => paging.PageSize(20)
                          .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                          .Total(100))
                      .DataBinding(bindings => bindings.Ajax()
                          .Select("_Index", ajaxController)
                          .Insert("_Insert", ajaxController)
                          .Update("_Update", ajaxController)
                          .Delete("_Delete", ajaxController));

      if (isEditable && isVisible)
        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
      else
        retVal.Editable(editing => editing.Enabled(false));

      if (columns != null)
        retVal = retVal.Columns(columns);

      return retVal;
    }

    #region /* Commented the following method as this is a duplicate of the first method in this class */

    //public static GridBuilder<T> BuildGrid<T>(
    //    this GridBuilder<T> gridbuilder,
    //    string gridName,
    //    string ajaxController,
    //    string indexID, EnumFeatures feature = EnumFeatures.Exempt) where T : class
    //{
    //    bool isEditable = true;
    //    bool isVisible = true;

    //    bool allowEdit = true;
    //    bool allowInsert = true;
    //    bool allowDelete = true;

    //    isEditable = (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Edit);
    //    allowEdit &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Edit);
    //    allowInsert &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Add);
    //    allowDelete &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Delete);


    //    GridBuilder<T> retVal = gridbuilder.Name(gridName)
    //                        .EnableCustomBinding(true)
    //                        .Pageable(paging => paging.PageSize(20)
    //                            .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
    //                            .Total(100))
    //                        .DataBinding(bindings => bindings.Ajax()
    //                            .Select("_Index", ajaxController, new { id = indexID, isNew = (indexID.Equals("0") ? true : false) })
    //                            .Insert("_Insert", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
    //                            .Update("_Update", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
    //                            .Delete("_Delete", ajaxController, new { MasterID = indexID, isNew = (indexID.Equals("0") ? true : false) }));

    //    if (isEditable && isVisible)
    //        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
    //    else
    //        retVal.Editable(editing => editing.Enabled(false));

    //    Type typeT = typeof(T);
    //    PropertyInfo[] properties = typeT.GetProperties();

    //    // Add keys
    //    GridDataKeyFactory<T> dataKeys = new GridDataKeyFactory<T>(retVal);
    //    foreach (PropertyInfo property in properties)
    //        if (IsAttributePresent<KeyAttribute>(property))
    //            dataKeys.Add(property.Name);

    //    // Add Columns
    //    GridColumnFactory<T> columns = new GridColumnFactory<T>(retVal);
    //    foreach (PropertyInfo property in properties)
    //    {
    //        bool IsColumnVisible = !IsAttributePresent<HiddenInputAttribute>(property);
    //        GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name).Visible(IsColumnVisible);
    //        if (IsAttributePresent<ClientTemplateHtmlAttribute>(property))
    //        {
    //            gridBoundBuilder.ClientTemplate(property.TemplateHtmlForProperty());
    //        }

    //    }

    //    if (isEditable && isVisible)
    //        columns.Command(commands =>
    //        {
    //            commands.Edit().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
    //            commands.Delete().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
    //        }
    //        );

    //    return retVal;
    //}
    #endregion

    public static GridBuilder<T> BuildGrid<T>(
        this GridBuilder<T> gridbuilder,
        string gridName,
        string ajaxController
        ) where T : class {
      bool isEditable = true;
      bool isVisible = true;

      GridBuilder<T> retVal = gridbuilder.Name(gridName).DataKeys(k => k.Add("ID"))
                          .EnableCustomBinding(true)
                          .Pageable(paging => paging.PageSize(20)
                              .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                              .Total(100))
                          .DataBinding(bindings => bindings.Ajax()
                              .Select("_Index", ajaxController)
                              .Insert("_Insert", ajaxController)
                              .Update("_Update", ajaxController)
                              .Delete("_Delete", ajaxController));

      if (isEditable && isVisible)
        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
      else
        retVal.Editable(editing => editing.Enabled(false));

      Type typeT = typeof(T);
      PropertyInfo[] properties = typeT.GetProperties();

      //// Add keys
      //GridDataKeyFactory<T> dataKeys = new GridDataKeyFactory<T>(retVal,false);
      //foreach (PropertyInfo property in properties)
      //  if (IsAttributePresent<KeyAttribute>(property))
      //    dataKeys.Add(property.Name);

      // Add Columns
      GridColumnFactory<T> columns = new GridColumnFactory<T>(retVal);
      foreach (PropertyInfo property in properties) {
        bool IsColumnVisible = !IsAttributePresent<HiddenInputAttribute>(property);
        GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name).Visible(IsColumnVisible);

        //Set numeric column to right align.
        if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(double))
          gridBoundBuilder.HtmlAttributes(new { style = "text-align: right;" }).Format("{0:0.00}");

        if (IsAttributePresent<ClientTemplateHtmlAttribute>(property)) {
          gridBoundBuilder.ClientTemplate(property.TemplateHtmlForProperty());
        }

      }

      if (isEditable && isVisible)
        columns.Command(commands => {
          commands.Edit().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          commands.Delete().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
        }
        );

      return retVal;
    }

    public static GridBuilder<T> BuildGrid<T>(
        this GridBuilder<T> gridbuilder,
        string gridName,
        string ajaxController,
        bool useClientTemplateHtml
        ) where T : class {
      bool isEditable = true;
      bool isVisible = true;

      GridBuilder<T> retVal = gridbuilder.Name(gridName).DataKeys(k => k.Add("ID"))
                           .Scrollable(sc => sc.Height("*"))
                           .Resizable(rs => rs.Columns(true))
                          .EnableCustomBinding(true)
                          .Pageable(paging => paging.PageSize(ConfigurationHelper.GetsmARTDetailGridPageSize())
                              .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                              .Total(100))
                          .DataBinding(bindings => bindings.Ajax()
                              .Select("_Index", ajaxController)
                              .Insert("_Insert", ajaxController)
                              .Update("_Update", ajaxController)
                              .Delete("_Delete", ajaxController));

      if (isEditable && isVisible)
        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
      else
        retVal.Editable(editing => editing.Enabled(false));

      Type typeT = typeof(T);
      PropertyInfo[] properties = typeT.GetProperties();

      //// Add keys
      //GridDataKeyFactory<T> dataKeys = new GridDataKeyFactory<T>(retVal);
      //foreach (PropertyInfo property in properties)
      //  if (IsAttributePresent<KeyAttribute>(property))
      //    dataKeys.Add(property.Name);

      // Add Columns

      if (!useClientTemplateHtml) {
        GridColumnFactory<T> columns = new GridColumnFactory<T>(retVal);
        foreach (PropertyInfo property in properties) {
          bool IsColumnVisible = !IsAttributePresent<HiddenInputAttribute>(property);
          GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name).Visible(IsColumnVisible);

          //Set numeric column to right align.
          if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(double))
            gridBoundBuilder.HtmlAttributes(new { style = "text-align: right;" }).Format("{0:0.00}");

          if (IsAttributePresent<ClientTemplateHtmlAttribute>(property)) {
            gridBoundBuilder.ClientTemplate(property.TemplateHtmlForProperty());
          }

        }

        if (isEditable && isVisible)
          columns.Command(commands => {
            commands.Edit().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
            commands.Delete().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          }
      );
      }
      else {
        Type type = typeof(T);
        retVal.ClientRowTemplate(type.TemplateHtml()).Columns(columns => columns.AutoGenerate(false));
      }

      return retVal;
    }

    //Created By SK.
    public static GridBuilder<T> BuildGrid<T>(
        this GridBuilder<T> gridbuilder,
        string gridName,
        string ajaxController,
        string action,
        object routedValue,
        string indexID,
        string[] hiddenColumns = null, bool allowInsert = true, bool allowEdit = true, bool allowDelete = true, EnumFeatures feature = EnumFeatures.Exempt) where T : class {
      bool isEditable = true;
      bool isVisible = true;

      allowEdit &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Edit);
      allowInsert &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Add);
      allowDelete &= (feature == EnumFeatures.Exempt) ? true : AccessControl.IsActionAccessible(HttpContext.Current.User, feature, EnumActions.Delete);

      GridBuilder<T> retVal = gridbuilder.Name(gridName).DataKeys(k => k.Add("ID"))
                           .Scrollable(sc => sc.Height("*"))
                           .Resizable(rs => rs.Columns(true))
                           .EnableCustomBinding(true)
                           .Pageable(paging => paging.PageSize(ConfigurationHelper.GetsmARTDetailGridPageSize())
                           .Style(Telerik.Web.Mvc.UI.GridPagerStyles.NextPreviousAndNumeric)
                           .Total(100))
                           .DataBinding(bindings => bindings.Ajax()
                           .Select(action, ajaxController, routedValue)
                           .Insert("_Insert", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
                           .Update("_Update", ajaxController, new { isNew = (indexID.Equals("0") ? true : false) })
                           .Delete("_Delete", ajaxController, new { MasterID = indexID, isNew = (indexID.Equals("0") ? true : false) })
                            );


      if (isEditable && isVisible) {
        if (allowInsert) {
          retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true))).ToolBar(commands => commands.Insert());
        }
        retVal.Editable(editing => editing.Enabled(isEditable).Mode(Telerik.Web.Mvc.UI.GridEditMode.PopUp).Window(w => w.Modal(true)));
      }
      else
        retVal.Editable(editing => editing.Enabled(false));

      Type typeT = typeof(T);
      PropertyInfo[] properties = typeT.GetProperties();

      //// Add keys      
      //GridDataKeyFactory<T> dataKeys = new GridDataKeyFactory<T>(retVal);
      //foreach (PropertyInfo property in properties)
      //  if (IsAttributePresent<KeyAttribute>(property))
      //    dataKeys.Add(property.Name);

      // Add Columns
      GridColumnFactory<T> columns = new GridColumnFactory<T>(retVal);
      foreach (PropertyInfo property in properties) {
        bool IsColumnVisible = !IsAttributePresent<HiddenInputAttribute>(property);
        if (hiddenColumns != null && hiddenColumns.Contains(property.Name)) {
          continue;
        }
        GridBoundColumnBuilder<T> gridBoundBuilder = columns.Bound(property.Name).Visible(IsColumnVisible);

        //Set numeric column to right align.
        if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(double))
          gridBoundBuilder.HtmlAttributes(new { style = "text-align: right;" }).Format("{0:0.00}");

        if (IsAttributePresent<ClientTemplateHtmlAttribute>(property)) {
          gridBoundBuilder.ClientTemplate(property.TemplateHtmlForProperty());
        }
      }


      if (isEditable && isVisible)
        columns.Command(commands => {
          if (allowEdit) {
            commands.Edit().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          }
          if (allowDelete) {
            commands.Delete().ButtonType(Telerik.Web.Mvc.UI.GridButtonType.Image);
          }

        }
        );

      return retVal;
    }


    public static SelectList MakeSelection(this SelectList list, object selection) {
      return new SelectList(list.Items, list.DataValueField, list.DataTextField, selection);
    }

    private static bool IsAttributePresent<T>(PropertyInfo property) where T : class {
      foreach (T cAttribute in property.GetCustomAttributes(typeof(T), false))
        if (cAttribute != null)
          return true;

      return false;
    }

    public static WindowBuilder BuildWindow(this WindowBuilder windowbuilder, string name) {
      WindowBuilder retVal = windowbuilder.Name(name)
                                          .Width(950)
                                          .Height(440)
                                          .Buttons(buttons => buttons.Maximize().Close())
                                          .Visible(false);
      return retVal;
    }

    #region Grid Template Column

    public static GridBoundColumnBuilder<T> BoundActionLink<T, TProperty>(this GridColumnFactory<T> factory, Expression<Func<T, TProperty>> linkText
                , string action, string controller, Expression<Func<T, object>> routeValues)
                where T : class {
      if (string.IsNullOrEmpty(controller))
        controller = factory.Container.ViewContext.Controller.GetType().Name.Replace("Controller", "");

      var urlHelper = new UrlHelper(factory.Container.ViewContext.RequestContext);


      if (!(routeValues.Body is NewExpression))
        throw new ArgumentException("routeValues.Body must be a NewExpression");

      RouteValueDictionary routeValueDictionary = ExtractClientTemplateRouteValues(((NewExpression)routeValues.Body));

      var link = urlHelper.Action(action, controller, routeValueDictionary);

      string lintTextString = string.Format("<#= {0} #>", ((MemberExpression)linkText.Body).Member.Name);
      var clientTemplate = string.Format(@"<a href=""{0}"">{1}</a>", link, lintTextString);

      return factory.Bound(linkText).Template(x => {
        var actionUrl = urlHelper.Action(action, controller, routeValues.Compile().Invoke(x));
        return string.Format(@"<a href=""{0}"">{1}</a>", actionUrl, linkText.Compile().Invoke(x));
      }).ClientTemplate(clientTemplate);

    }

    private static RouteValueDictionary ExtractClientTemplateRouteValues(NewExpression newExpression) {
      RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

      for (int index = 0; index < newExpression.Arguments.Count; index++) {
        var argument = newExpression.Arguments[index];
        var member = newExpression.Members[index];

        object value;

        switch (argument.NodeType) {
          case ExpressionType.Constant:
            value = ((ConstantExpression)argument).Value;
            break;

          case ExpressionType.MemberAccess:
            MemberExpression memberExpression = (MemberExpression)argument;

            if (memberExpression.Expression is ParameterExpression)
              value = string.Format("<#= {0} #>", memberExpression.Member.Name);
            else
              value = GetValue(memberExpression);

            break;

          default:
            throw new smART.Common.InvalidOperationException("Unknown expression type!");
        }

        routeValueDictionary.Add(member.Name, value);
      }
      return routeValueDictionary;
    }

    private static object GetValue(MemberExpression member) {
      var objectMember = Expression.Convert(member, typeof(object));
      var getterLambda = Expression.Lambda<Func<object>>(objectMember);
      return getterLambda.Compile().Invoke();
    }


    #endregion Grid Temlate Column

  }
}