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
    [Table("M_Item_Attachment")]
    public class ItemAttachment :AttachmentEntity<Item>
    {

    }
}