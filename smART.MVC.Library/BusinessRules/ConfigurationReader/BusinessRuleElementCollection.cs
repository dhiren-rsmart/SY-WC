using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace smART.MVC.Library.BusinessRules
{
    public class BusinessRuleElementCollection: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BusinessRuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BusinessRuleElement)element).Key;
        }
    }
}
