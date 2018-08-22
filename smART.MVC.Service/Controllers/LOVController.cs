using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;

namespace smART.MVC.Service.Controllers {

  public class LOVController : BaseController<LOVTypeLibrary, LOVType> {

    public LOVController() {
    }

    // GET api/values/5
    [HttpGet]
    public LOVType GetByType(string lovType) {
      try {
        LOVTypeLibrary lib = new LOVTypeLibrary(base.ConString);
        return lib.GetByLOVType(lovType);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }
  }
}
