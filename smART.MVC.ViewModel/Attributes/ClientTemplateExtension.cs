using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace smART.ViewModel
{
    public static class ClientTemplateExtension
    {
        public static string TemplateHtml<T>(this T obj) where T : Type
        {
            var attr = obj.GetCustomAttributes(typeof(ClientTemplateHtmlAttribute), true);
            if (attr != null && attr.Length > 0)
                return ((ClientTemplateHtmlAttribute)attr[0]).TemplateHtml;

            return string.Empty;
        }
        public static string TemplateHtmlForProperty<T>(this T obj) where T : PropertyInfo
        {
            var attr = obj.GetCustomAttributes(typeof(ClientTemplateHtmlAttribute), true);
            if (attr != null && attr.Length > 0)
                return ((ClientTemplateHtmlAttribute)attr[0]).TemplateHtml;
            return string.Empty;
        }
    }
}
