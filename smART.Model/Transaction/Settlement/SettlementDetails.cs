// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Settlement_Details")]
    public class SettlementDetails:BaseEntity
    {
        public Settlement Settlement_ID { get; set; }

        public ScaleDetails Scale_Details_ID { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public PriceList Price_List_ID { get; set; }
                
        public string Item_UOM { get; set; }
                
        [DataType("decimal(16 ,4")] 
        public decimal Item_UOM_NetWeight { get; set; }

    }
}
