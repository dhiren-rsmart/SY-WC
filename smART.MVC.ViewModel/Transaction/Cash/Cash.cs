using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web;

namespace smART.ViewModel {

  public class Cash : BaseEntity {

    [UIHint("BlankDate")]
    [Display(Name = "Date and Time")]
    public DateTime? Date {
      get;
      set;
    }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Transaction Type")]
    public string Transaction_Type {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Payment")]
    public PaymentReceipt Payment {
      get;
      set;
    }

    [Display(Name = "Amount Paid")]
    public Decimal Amount_Paid {
      get;
      set;
    }

    [Display(Name = "Amount Received")]
    public Decimal Amount_Received {
      get;
      set;
    }

    [Display(Name = "Current Balance")]
    public Decimal Balance {
      get;
      set;
    }

    [Display(Name = "Amount")]
    public Decimal Amount {
      get;
      set;
    }

    public Cash() {
      Date = DateTime.Now;
      Created_By = HttpContext.Current.User.Identity.Name;
    }
  }
}
