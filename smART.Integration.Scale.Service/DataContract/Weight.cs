using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Integration.Scale.Service
{
    [DataContract]
    public class WeightData
    {
        [DataMember]
        public string Weight { get; set; }
        
    }
}
