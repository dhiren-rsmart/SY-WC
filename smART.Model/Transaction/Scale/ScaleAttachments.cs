// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace smART.Model
{
    [Table("T_Scale_Attachments")]
    public class ScaleAttachments : AttachmentEntity<Scale>
    {
      public int Ref_Type {
        get;
        set;
      }

      public int Ref_ID {
        get;
        set;
      }
    }
}
