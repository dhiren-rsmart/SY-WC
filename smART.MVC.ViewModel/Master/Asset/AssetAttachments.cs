// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel {

  public class AssetAttachments : AttachmentEntity<Asset> {

    public AssetAttachments() {
      ControllerName = "AssetAttachments";
    }

  }
}
