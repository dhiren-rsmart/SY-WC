using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace smART.MVC.Library.BusinessRules
{
    public class BusinessRuleSection : ConfigurationSection
    {
        public static BusinessRuleSection _section = ConfigurationManager.GetSection("businessRuleBindings") as BusinessRuleSection;

        [ConfigurationProperty("businessRules", IsDefaultCollection = true)]
        public BusinessRuleElementCollection BusinessRules
        {
            get { return (BusinessRuleElementCollection)this["businessRules"]; }
            set { this["businessRules"] = value; }
        }

        public IList<BusinessRuleElement> GetBusinessRules()
        {
            List<BusinessRuleElement> result = new List<BusinessRuleElement>();
            
            foreach (BusinessRuleElement element in _section.BusinessRules)
            {
                result.Add(element);
            }

            return result;
        }

        public IList<BusinessRuleElement> GetBusinessRules(string className, string eventName)
        {
            List<BusinessRuleElement> result = (from r in GetBusinessRules()
                                               where (r.ClassName.ToLower().Trim().Equals(className.ToLower().Trim()) &&
                                                      r.EventName.ToLower().Trim().Equals(eventName.ToLower().Trim()))
                                               select r).Distinct().ToList();

            return result;
        }
    }

}
