using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace smART.MVC.Library.BusinessRules
{
    public class BusinessRuleElement: ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("className", IsRequired = true)]
        public string ClassName
        {
            get { return (string)this["className"]; }
            set { this["className"] = value; }
        }

        [ConfigurationProperty("eventName", IsRequired = true)]
        public string EventName
        {
            get { return (string)this["eventName"]; }
            set { this["eventName"] = value; }
        }

        [ConfigurationProperty("methodName", IsRequired = true)]
        public string MethodName
        {
            get { return (string)this["methodName"]; }
            set { this["methodName"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }
    }
}
