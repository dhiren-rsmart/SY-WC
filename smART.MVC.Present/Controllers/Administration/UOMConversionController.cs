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
    [Feature(EnumFeatures.Master_LOV)]
    public class UOMConversionController : BaseFormController<UOMConversionLibrary, UOMConversion>
    {
        #region Constructor

        public UOMConversionController() : base("~/Views/Administration/UOMConversion/_List.cshtml", null) { }

        #endregion Constructor

        #region Public Methods

        [HttpGet]
        public string _ConvertUOM(string sourceUOM, string targetUOM)
        {
            string convFactor = "1";
            if (!string.IsNullOrWhiteSpace(sourceUOM) && sourceUOM != targetUOM)
            {
                UOMConversion result = Library.GetByUOM(sourceUOM, targetUOM);
                if (result != null)
                {
                    convFactor = result.Factor.ToString();
                }
            }
            return convFactor;
        }

        #endregion Public Methods
    }
}
