// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Scale_Details")]
    public class ScaleDetails : BaseEntity
    {   
       
        public Scale Scale { get; set; }

        public Item Item_Received { get; set; }

        public Item Apply_To_Item { get; set; }

        [Required(AllowEmptyStrings=true)] 
        public Decimal Split_Value { get; set; }

        [Required(AllowEmptyStrings=true )]
        public Decimal GrossWeight { get; set; }

        [Required(AllowEmptyStrings=true) ]
        public Decimal TareWeight { get; set; }

        [Required(AllowEmptyStrings=true )]
        public Decimal Contamination_Weight { get; set; }

        [Required(AllowEmptyStrings=true )]
        public Decimal NetWeight { get; set; }

        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string Supplier_Item { get; set; }

        [Required(AllowEmptyStrings=true )]
        public Decimal Supplier_Net_Weight { get; set; }

        [Required(AllowEmptyStrings = true)]
        public Decimal Settlement_Diff_NetWeight { get; set; }

        [Required(AllowEmptyStrings = true)]
        public Decimal Old_Net_Weight{get;set;}

        [Required(AllowEmptyStrings = true)]
        [DataType("decimal(16 ,4")]
        public Decimal Rate {
          get;
          set;
        }

         [StringLength(100, ErrorMessage = "Maximum legth is 100")]
        public string Notes {
          get;
          set;
        }

    }
}
