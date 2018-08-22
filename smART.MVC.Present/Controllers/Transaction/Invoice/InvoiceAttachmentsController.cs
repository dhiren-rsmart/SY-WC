// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/04/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using System.IO;
using smART.Common;

namespace smART.MVC.Present.Controllers.Transaction
{
    [Feature(EnumFeatures.Transaction_InvoiceAttachment)]
    public class InvoiceAttachmentsController : AttachmentGridController<InvoiceAttachmentsLibrary, InvoiceAttachments, Invoice>
    {
        public InvoiceAttachmentsController() : base("InvoiceAttachments", new string[] { "Parent" }) { }

     }
}
