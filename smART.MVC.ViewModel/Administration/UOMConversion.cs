using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using System.Web.Mvc;

namespace smART.ViewModel
{     
    public class UOMConversion :  BaseEntity
    {
        [Required]
        [StringLength(4, ErrorMessage = "Maximum length is 4")]
        [DisplayName("Conversion UOM")]
        [UIHint("LOVDropDownList")]
         public string Conversion_UOM { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Conversion UOM Desc")]
        public string Conversion_UOM_Desc { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "Maximum length is 4")]
        [DisplayName("Base UOM")]
        [UIHint("LOVDropDownList")]
        public string Base_UOM { get; set; }

        [DisplayName("Base UOM Desc")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Base_UOM_Desc { get; set; }

        [Required]
        public double Factor { get; set; }

        [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Is_Base_UOM' <#= Is_Base_UOM ? \"checked='checked'\" : \"\" #> />")]
        [DisplayName("Base UOM")]
        public bool Is_Base_UOM { get; set; }
      
    }
}
