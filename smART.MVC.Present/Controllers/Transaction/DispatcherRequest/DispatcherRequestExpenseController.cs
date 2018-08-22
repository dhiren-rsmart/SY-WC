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
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Transaction_DispatcherRequestExpense)]
    public class DispatcherRequestExpenseController : ExpenseGridController<DispatcherRequestExpenseLibrary, ExpensesRequest, DispatcherRequest>
    {
        #region Constructor

        public DispatcherRequestExpenseController() : base("DispatcherRequestExpense", null) { }

        #endregion Constructor
    }
}