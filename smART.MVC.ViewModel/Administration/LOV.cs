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
    public class LOV : BaseEntity
    {
        [Required]
        [StringLength(45, ErrorMessage="Maximum length is 45")]        
        [DisplayName("LOV Value")]
        public string LOV_Value { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("LOV Display Value")]
        public string LOV_Display_Value { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Association")]
        [HiddenInput(DisplayValue = true)]
        public string LOV_Association { get; set; }

        [HiddenInput(DisplayValue = false)]
        public LOVType LOVType { get; set; }

        [DisplayName("Active")]
        [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= LOV_Active? \"checked='checked'\" : \"\" #> />")]
        public bool LOV_Active { get; set; }

       [HiddenInput(DisplayValue = false)]
       [DisplayName("Parent")]
        public LOV Parent {
          get;
          set;
        }

       [HiddenInput(DisplayValue = false)]
       public int Parent_Type_ID {
         get;
         set;
       }
        public LOV()
        {
            LOV_Active = true;          
        }

    }
}
