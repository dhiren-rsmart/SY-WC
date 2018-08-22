using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace smART.MVC.Present.Extensions {
  public static class EditorExtensions {
    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      if (validationString == null)
        return MvcHtmlString.Create(editorString.ToString());
      else
        return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }

    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object additionalViewData) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, additionalViewData);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      if (validationString == null)
        return MvcHtmlString.Create(editorString.ToString());
      else
        return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }

    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, templateName);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }

    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, object additionalViewData) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, templateName, additionalViewData);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }

    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, templateName, htmlFieldName);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }

    public static MvcHtmlString AuthorizedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName, object additionalViewData) {
      bool isVisible = true;
      bool isEditable = true;

      //TODO:: Check for access control

      MvcHtmlString editorString = System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, templateName, htmlFieldName, additionalViewData);
      MvcHtmlString validationString = MvcHtmlString.Empty;

      if (!isVisible)
        editorString = System.Web.Mvc.Html.InputExtensions.HiddenFor<TModel, TValue>(html, expression);
      else
        if (!isEditable)
          editorString = System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TValue>(html, expression, new {
            @readonly = "readonly"
          });
        else
          validationString = System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor<TModel, TValue>(html, expression, "*");

      return MvcHtmlString.Create(editorString.ToString() + validationString.ToString());
    }
  }
}