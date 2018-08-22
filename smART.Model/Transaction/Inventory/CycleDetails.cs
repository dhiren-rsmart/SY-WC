// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("T_Cycle_Details")]
  public class CycleDetails : BaseEntity {

    [Display(Name = "Date")]    
    public DateTime Date {
      get;
      set;
    }

    public Cycle Cycle {
      get;
      set;
    }

    public Item Item {
      get;
      set;
    }

    public SettlementDetails Purchase_ID {
      get;
      set;
    }

    public Decimal Purchase_Qty {
      get;
      set;
    }

    public Decimal Purchase_Cost {
      get;
      set;
    }

    public Decimal Purchase_Amount {
      get;
      set;
    }

    public Decimal Average_Cost {
      get;
      set;
    }

    [Display(Name = "Allow system to override")]
    public bool Override_OpCost {
      get;
      set;
    }
       
    public bool Allow_Triggering {
      get;
      set;
    }
  }
}
