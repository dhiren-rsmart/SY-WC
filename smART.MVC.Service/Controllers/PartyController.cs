using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.ViewModel;
using smART.Library;

using smART.ViewModel;

namespace smART.MVC.Service.Controllers
{
    public class PartyController : BaseController<PartyLibrary, Party> {

      public PartyController() {
    }

    // GET api/values/5
    [HttpGet]
      public IEnumerable<Party> GetOrgParties() {
      try {
        PartyLibrary lib = new PartyLibrary(base.ConString);
        return lib.GetOrgParties();
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    }
}
