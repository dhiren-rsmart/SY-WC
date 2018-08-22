// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/04/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using System.IO;
using smART.MVC.Present.Helpers;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Cash)]

  public class CashController : BaseFormController<CashLibrary, Cash> {
    #region /* Constructors */

    public CashController()
      : base("~/Views/Transaction/Cash/_List.cshtml", null, null) {
    }

    #endregion /* Constructors */

    protected override ActionResult Display(Cash entity) {
      CashLibrary lib = new CashLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      entity.Balance = lib.GetBalance();
      return View("New", entity);
    }
  }

}
