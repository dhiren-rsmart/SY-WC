// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("T_Cycle")]
  public class Cycle : BaseEntity {

    [Display(Name = "Start")]    
    public DateTime Start_Date {
      get;
      set;
    }

    [Display(Name = "End")]    
    public DateTime End_Date {
      get;
      set;
    }
  }
}
