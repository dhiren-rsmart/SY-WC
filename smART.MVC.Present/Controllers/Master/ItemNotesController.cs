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
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers.Master
{
    [Feature(EnumFeatures.Master_ItemNotes)]
    public class ItemNotesController: NotesGridController<ItemNotesLibrary, ItemNotes, Item>
    {
        public ItemNotesController() : base("ItemNotes", new string[] { "Parent" }) { }
    }
  
}