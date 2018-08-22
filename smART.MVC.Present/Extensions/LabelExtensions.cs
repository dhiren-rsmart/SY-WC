﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;

namespace smART.MVC.Present.Extensions
{
    public static class LabelExtensions
    {
        public static MvcHtmlString AuthorizedLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return System.Web.Mvc.Html.LabelExtensions.LabelFor<TModel, TValue>(html, expression);
        }
        public static MvcHtmlString AuthorizedLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
        {
            return System.Web.Mvc.Html.LabelExtensions.LabelFor<TModel, TValue>(html, expression, labelText);
        }

        public static MvcHtmlString AuthorizedLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(labelText))
            {

                return MvcHtmlString.Empty;

            }

            TagBuilder tag = new TagBuilder("label");

            tag.MergeAttributes(htmlAttributes);

            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            tag.SetInnerText(labelText);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString MyLabel(this HtmlHelper htmlHelper,
          string labelText, IDictionary<string, object> htmlAttributes) {
          var attributes = htmlAttributes;
          return MvcHtmlString.Create(
              String.Format("<label for=\"{0}\" {1}>{0}</label",
              labelText, attributes));
        }

    }
}