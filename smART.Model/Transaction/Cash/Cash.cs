using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("T_Cash")]
  public class Cash : BaseEntity {

    public DateTime? Date {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Transaction_Type {
      get;
      set;
    }

    public PaymentReceipt Payment {
      get;
      set;
    }

    public Decimal Amount_Paid {
      get;
      set;
    }

    public Decimal Amount_Received {
      get;
      set;
    }

    public Decimal Balance {
      get;
      set;
    }

  }
}
