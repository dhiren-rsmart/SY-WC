using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present.Controllers.Master
{
    [Feature(EnumFeatures.Master_Bin)]
    public class BinController : PartyChildGridController<BinLibrary, Bin>
    {
        public BinController() : base("Bin", new string[] { "Party" }) { }

    }
}
