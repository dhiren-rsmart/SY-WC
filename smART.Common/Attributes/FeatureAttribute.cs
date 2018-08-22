using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FeatureAttribute: Attribute
    {
        public EnumFeatures[] Features { get; set; }

        public FeatureAttribute(params EnumFeatures[] features)
        {
            this.Features = features;
        }
    }
}
