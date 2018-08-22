using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel.Web;

namespace smART.Integration.Scale.Service {

  public partial class WeightWindowsService : ServiceBase {


    public WeightWindowsService() {
      ServiceName = "WeightWindowsServiceWCF";
    }

    protected override void OnStart(string[] args) {      
    }

    protected override void OnStop() {

    }
  }
}
