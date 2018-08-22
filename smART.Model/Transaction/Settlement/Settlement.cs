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
    [Table("T_Settlement")]
    public class Settlement:BaseEntity
    {
        public Scale Scale { get; set; }

        public decimal Amount { get; set; }

        public bool Ready_For_Payment { get; set; }

        public decimal Amount_Paid_Till_Date { get; set; }
    }
}
