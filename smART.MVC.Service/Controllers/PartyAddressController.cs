using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.ViewModel;
using smART.Library;

using smART.ViewModel;

namespace smART.MVC.Service.Controllers {
  public class PartyAddressController : BaseController<AddressBookLibrary, AddressBook> {

    public PartyAddressController() {
    }

    // GET api/values/5
    [HttpGet]
    public IEnumerable<AddressBook> GetByParent(int id) {
      try {
        AddressBookLibrary lib = new AddressBookLibrary(base.ConString);
        return lib.GetAllByParentID(id, new string[] { "Party" });
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

  }
}
