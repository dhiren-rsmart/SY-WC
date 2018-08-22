using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model {

  [Table("T_Invoice"), Unique("Booking_Ref_ID, Active_Ind")]
  public class Invoice : BaseEntity {
    public int Invoice_No { get; set; }
    public DateTime? Trans_Date { get; set; }
    public string Trans_Type { get; set; }
    public decimal Total_Amt { get; set; }
    public decimal Advance_Amt { get; set; }
    public decimal Tax_Amt { get; set; }
    public decimal Expences_Amt { get; set; }
    public decimal Discount { get; set; }
    public string Discount_Type { get; set; }
    public decimal Net_Amt { get; set; }
    public Booking Booking { get; set; }
    public decimal Amount_Paid_Till_Date { get; set; }
    public bool Is_Print { get; set; }
    public bool Is_Sent_QuickBooks { get; set; }
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Invoice_Status { get; set; }
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Invoice_Type { get; set; }
    public SalesOrder Sales_Order_No { get; set; }
    public bool QB {get;set;}
  }
}
