// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    public abstract class BaseNotes:BaseEntity
    {
        [StringLength(2000, ErrorMessage = "Maximum length is 2000")]
        public string Notes { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Note_Type { get; set; }

        public bool Print { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]        
        public string Print_Area { get; set; }
    }
}
