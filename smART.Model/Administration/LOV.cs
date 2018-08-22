using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("LOV_Value"), Unique("LOV_Value,LOVType_ID,Active_Ind")]
    public class LOV : BaseEntity
    {
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string LOV_Value { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string LOV_Display_Value { get; set; }
                
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string LOV_Association { get; set; }

        public LOVType LOVType { get; set; }

        public bool LOV_Active { get; set; }

        public LOV Parent {
          get;
          set;
        }
    }
}
