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
    public class LOVType : BaseEntity
    {
        [Required]
        [StringLength(45, ErrorMessage="Maximum length is 45")]
        [DisplayName("LOV Type")]
        public string LOVType_Name { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Description")]
        public string LOVType_Description { get; set; }

        [DisplayName("Parent Type")]
        [HiddenInput(DisplayValue = false)]
        public LOVType ParentType {
          get;
          set;
        }
    }
}
