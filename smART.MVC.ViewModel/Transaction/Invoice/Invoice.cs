using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace smART.ViewModel {
  public class Invoice : BaseEntity {

    public int Invoice_No { get; set; }

    [UIHint("BlankDate")]
    [Display(Name = "Invoice Date")]
    public DateTime? Trans_Date { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Order Type")]
    public string Trans_Type { get; set; }

    [Display(Name = "Invoice Gross Amount")]
    public decimal Total_Amt { get; set; }

    [Display(Name = "Applicable Discounts")]
    public decimal Discount { get; set; }

    [Display(Name = "Advance Received")]
    public decimal Advance_Amt { get; set; }

    [Display(Name = "Tax")]
    public decimal Tax_Amt { get; set; }

    [Display(Name = "Expences")]
    public decimal Expences_Amt { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Discount Type")]
    public string Discount_Type { get; set; }

    [Display(Name = "Net Invoice amount")]
    //[HiddenInput(DisplayValue = false)]
    public decimal Net_Amt { get; set; }

    public Booking Booking { get; set; }

    [Display(Name = "Net Invoice amount")]
    //[HiddenInput(DisplayValue = false)]
    public decimal Amount_Paid_Till_Date { get; set; }

    [HiddenInput(DisplayValue = false)]
    public bool Is_Print { get; set; }

    [HiddenInput(DisplayValue = false)]
    public bool Is_Sent_QuickBooks { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Invoice Status")]
    public string Invoice_Status { get; set; }

    [DisplayName("Balance Amount")]
    public decimal Balance_Amount { get { return Net_Amt - Amount_Paid_Till_Date; } }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Invoice Type")]
    public string Invoice_Type { get; set; }

    public SalesOrder Sales_Order_No { get; set; }

    [Display(Name = "Party Name")]
    public string Party_Name {
      get {
        if (Invoice_Type == "Local Sales")
          return (Sales_Order_No != null && Sales_Order_No.Party != null ? Sales_Order_No.Party.Party_Name : "");
        else
          return (Booking != null && Booking.Sales_Order_No != null && Booking.Sales_Order_No.Party != null ? Booking.Sales_Order_No.Party.Party_Name : "");
      }
    }

    [Display(Name = "SO#")]
    private string _invoice_SO;
    public string Invoice_SO {
      get {
        if (Invoice_Type == "Local Sales")
          _invoice_SO = (Sales_Order_No != null ? Sales_Order_No.ID.ToString() : "");
        else
          _invoice_SO = (Booking != null && Booking.Sales_Order_No != null ? Booking.Sales_Order_No.ID.ToString() : "");
        return _invoice_SO;
      }
      set { _invoice_SO = value; }
    }

    [HiddenInput(DisplayValue = false)]
    public bool QB {
      get;
      set;
    }

    public Invoice()
      : base() {
      //Booking = new Booking();
      //Sales_Order_No = new SalesOrder();
      Trans_Date = DateTime.Now;
    }

  }
}
