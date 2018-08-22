using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace smART.ViewModel {

  public class CycleDetails : BaseEntity {

    [Display(Name = "Date")]
    [HiddenInput(DisplayValue = false)]
    public DateTime Date {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Cycle Cycle {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Item Item {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public SettlementDetails Purchase_ID {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Decimal Purchase_Qty {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Decimal Purchase_Cost {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Decimal Purchase_Amount {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Decimal Average_Cost {
      get;
      set;
    }

    public CycleDetails() {
      Date = DateTime.Now;
    }

    [Display(Name = "Allow system to override")]
    public bool Override_OpCost {
      get;
      set;
    }

    [HiddenInput (DisplayValue=false)]
    public bool Allow_Triggering {
      get;
      set;
    }

  }
}
