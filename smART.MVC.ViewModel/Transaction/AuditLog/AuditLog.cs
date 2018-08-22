// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel
{
    public class AuditLog : BaseEntity
    {
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        [Display(Name="Entity Name")]
        [HiddenInput(DisplayValue = false)]
        public string Entity_Name { get; set; }

        [Display(Name = "Entity ID")]
        [HiddenInput(DisplayValue = false)]
        public int Entity_ID { get; set; }

        [Display(Name = "Field Name")]
        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string Field_Name { get; set; }

        [Display(Name = "Old Value")]
        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string Old_Value { get; set; }

        [Display(Name = "New Value")]
        [StringLength(45, ErrorMessage = "Maximum legth is 45")]
        public string New_Value { get; set; }

        [Display(Name = "Action")]
        [HiddenInput(DisplayValue = false)]
        public string Action { get; set; }

    }
}
