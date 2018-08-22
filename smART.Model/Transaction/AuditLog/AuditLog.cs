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
    [Table("T_Audit_Log")]
    public class AuditLog : BaseEntity
    {
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Entity_Name { get; set; }

        public int Entity_ID { get; set; }
               
        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string Field_Name { get; set; }
              

        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string Old_Value { get; set; }

        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string New_Value { get; set; }

        [StringLength(10, ErrorMessage = "Maximum legth is 10")]
        public string Action { get; set; }
    }
}
