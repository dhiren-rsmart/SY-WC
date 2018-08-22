// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel {

  public class ScaleAttachments : AttachmentEntity<Scale> {
    public ScaleAttachments() {
      ControllerName = "ScaleAttachments";
    }

    [HiddenInput(DisplayValue = false)]
    public int Ref_Type {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public int Ref_ID {
      get;
      set;
    }
  }
}
