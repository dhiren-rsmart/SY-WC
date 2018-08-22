// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel
{
    public class SalesOrderAttachments : AttachmentEntity<SalesOrder>
    {
        //[Required]
        //[StringLength(45, ErrorMessage = "Maximum length is 45")]
        //[Display(Name = "Title", Order = 1, Description = "Title")]
        //[DataType(System.ComponentModel.DataAnnotations.DataType.Url)]
        //[ClientTemplateHtml("<a target='blank' href='/../SalesOrderAttachments/OpenDocument?id=<#= Document_RefId #>'><#= Document_Title #></a>")]
        //public override string Document_Title { get; set; }
        public SalesOrderAttachments()
        {
            ControllerName = "SalesOrderAttachments";        
        }
    }
}
