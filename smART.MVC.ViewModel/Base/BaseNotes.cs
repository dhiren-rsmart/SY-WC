using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel
{
    public abstract class BaseNotes:BaseEntity
    {
       
        [StringLength(2000, ErrorMessage = "Maximum length is 2000")]
        [DisplayName("Notes")]
        [AllowHtml]      
        [ClientTemplateHtml("<div style='max-width:100%;max-height:100%px'><#= Notes #></div>")]
        public string Notes { get; set; }

        //[StringLength(45, ErrorMessage = "Maximum length is 45")]
        //[Required]
        [DisplayName("Notes Type")]
        [UIHint("LOVDropDownList")]
        public string Note_Type { get; set; }

        // [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= Print? \"checked='checked'\" : \"\" #> />")]
        //[DisplayName("Print")]
        //public bool Print { get; set; }

        ////[StringLength(10, ErrorMessage = "Maximum length is 10")]
        ////[Required]
        //[DisplayName("Print Area")]
        //[UIHint("LOVDropDownList")]
        //public string Print_Area { get; set; }

    }
}
