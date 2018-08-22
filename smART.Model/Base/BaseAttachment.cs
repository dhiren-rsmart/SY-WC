// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace smART.Model
{
    public abstract class BaseAttachment : BaseEntity
    {
        //[NotMapped]
        //public bool SelctChk { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Document_Title { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Document_Name { get; set; }
                
        public Guid Document_RefId { get; set; }

        [StringLength(10, ErrorMessage = "Maximum length is 10")]
        public string Document_Type { get; set; }

        public long Document_Size { get; set; }


        [StringLength(256, ErrorMessage = "Maximum length is 256")]
        public string Document_Path { get; set; }

        [StringLength(25, ErrorMessage = "Maximum length is 25")]
        public string Mime_Type { get; set; }
            
    }
}
