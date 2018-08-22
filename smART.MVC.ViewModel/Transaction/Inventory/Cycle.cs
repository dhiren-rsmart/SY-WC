using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace smART.ViewModel {

  public class Cycle : BaseEntity {

    [Display(Name = "Start Date")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM d, yyyy}")]
    public DateTime Start_Date {
      get;
      set;
    }

    [Display(Name = "End Date")]
   [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: MMM d, yyyy}")]
    public DateTime End_Date {
      get;
      set;
    }

    public Cycle() {
      Start_Date = DateTime.Now;
      End_Date = DateTime.Now;
    }
  }
}
