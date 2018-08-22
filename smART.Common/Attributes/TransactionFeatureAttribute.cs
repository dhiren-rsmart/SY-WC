using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionFeatureAttribute : Attribute
    {
        public EnumFeatures[] Features { get; set; }

        public TransactionFeatureAttribute(params EnumFeatures[] features)
        {
            this.Features = features;
        }
    }
}
