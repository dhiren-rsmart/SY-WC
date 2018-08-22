using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.ViewModel;
using smART.Library;

namespace smART.MVC.Service.Controllers {

  public class EmployeeController : BaseController<EmployeeLibrary, Employee> {
    public EmployeeController() {
    }
  }
}
