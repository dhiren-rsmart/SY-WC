using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
     [Table("M_UOM_Conversion"), Unique("Conversion_UOM,Base_UOM, Active_Ind")]
    public class UOMConversion : BaseEntity
    {
        [StringLength(4, ErrorMessage = "Maximum length is 4")]
         public string Conversion_UOM { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Conversion_UOM_Desc { get; set; }

        [StringLength(4, ErrorMessage = "Maximum length is 4")]
        public string Base_UOM { get; set; }


        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Base_UOM_Desc { get; set; }
                 
        public double Factor { get; set; }
                
        public bool Is_Base_UOM { get; set; }
      
    }
}
