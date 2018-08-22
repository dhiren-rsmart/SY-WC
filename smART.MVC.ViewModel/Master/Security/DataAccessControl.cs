﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel
{    
    public class DataAccessControl : BaseEntity
    {        
        public Feature Feature { get; set; }        
             
        public Role Role { get; set; }
    }
        
    public class DataAccessControlFilter:BaseEntity
    {
        public DataAccessControl DataAccessControl { get; set; }

        [Required]
        // View/Table
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string FilterFieldType { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string FilterFieldName { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string FilterFieldValue { get; set; }

    }
}
