using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.ViewModel
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Property)]
    public class ClientTemplateHtmlAttribute : Attribute
    {
        public string TemplateHtml { get; set; }

        public ClientTemplateHtmlAttribute(string templateHtml)
        {
            this.TemplateHtml = templateHtml;
        }
    }
}
