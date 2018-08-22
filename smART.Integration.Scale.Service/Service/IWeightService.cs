using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceModel.Web;

namespace smART.Integration.Scale.Service
{
    [ServiceContract]
    public interface IWeightService
    {
        [OperationContract]
        [WebInvoke(Method="POST", UriTemplate="GetWeight", ResponseFormat=WebMessageFormat.Json)]
        WeightData GetWeight(string scaleIdentifier);

        [OperationContract]
        [WebGet(UriTemplate = "GetWeightJSON", ResponseFormat = WebMessageFormat.Json)]
        WeightData GetWeightJSON();
    }
}
