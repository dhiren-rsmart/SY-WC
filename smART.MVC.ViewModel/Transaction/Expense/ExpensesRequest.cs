using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel {

  public class ExpensesRequest : BaseEntity {

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [DisplayName("Expense Type")]
    public string EXPENSE_TYPE { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(50, ErrorMessage = "Maximum length is 45")]
    [DisplayName("Expense Sub Type")]
    public string EXPENSE_Sub_TYPE { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [DisplayName("Paid By")]
    public string Paid_By { get; set; }

    [DisplayName("Approved Amount Charge")]
    [DataType("decimal(16 ,4")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000}")]
    public double Amount_Paid { get; set; }

    [DisplayName("Paid to Party")]
    public Party Paid_Party_To { get; set; }

    [DisplayName("Amount paid Till Date")]
    [DataType("decimal(16 ,4")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000}")]
    public Double Amount_Paid_Till_Date { get; set; }

    ////[ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Approved' <#= Approved? \"checked='checked'\" : \"\" #> />")]
    //[DisplayName("Approved")]
    ////[HiddenInput(DisplayValue = false)]
    //public Boolean Approved { get; set; }

    [DisplayName("Paid (Y/N)")]
    [HiddenInput(DisplayValue = false)]
    public Boolean Paid_YN { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(100, ErrorMessage = "Maximum length is 100")]
    [DisplayName("Comments")]
    [HiddenInput(DisplayValue = false)]
    public string Comments { get; set; }

    [DisplayName("Transaction ID")]
    //[HiddenInput(DisplayValue = false)]
    public int Reference_ID { get; set; } // ? to ask 

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [DisplayName("Transaction Name")]
    //[HiddenInput(DisplayValue = false)]
    public string Reference_Table { get; set; } //? to ask

    [HiddenInput(DisplayValue = false)]
    public BaseEntity Reference { get; set; } // ? to ask 

    [UIHint("LOVDropDownList")]
    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    [DisplayName("Expense Status")]
    [HiddenInput(DisplayValue = false)]
    public string Expense_Status { get; set; }

    [DisplayName("Balance Amount")]
    [DataType("decimal(18 ,2")]
    [HiddenInput(DisplayValue = false)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
    public double Balance_Amount { get { return Amount_Paid - Amount_Paid_Till_Date; } }

    [HiddenInput(DisplayValue = false)]
    public PaymentReceipt Payment { get; set; }

    [HiddenInput(DisplayValue = false)]
    public Invoice Invoice { get; set; }

    [HiddenInput(DisplayValue = false)]
    public DispatcherRequest Dispatcher_Request_Ref { get; set; }

    [HiddenInput(DisplayValue = false)]
    public Scale Scale_Ref { get; set; }

    // =====For display only=======
    [HiddenInput(DisplayValue = false)]
    [DisplayName("Scale#")]
    public string Scale_ID {
      get { return Scale_Ref != null ? Scale_Ref.ID.ToString() : ""; }
    }

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Booking#")]
    public string Booking_No {
      get { return Dispatcher_Request_Ref != null && Dispatcher_Request_Ref.Booking_Ref_No != null ? Dispatcher_Request_Ref.Booking_Ref_No.Booking_Ref_No : ""; }
    }

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Container#")]
    public string Container_No {
      get { return Dispatcher_Request_Ref != null && Dispatcher_Request_Ref.Container != null ? Dispatcher_Request_Ref.Container.Container_No : ""; }
    }

    public ExpensesRequest()
      : base() {
      //Paid_Party_To = new Party();
      //Payment = new PaymentReceipt();
      //Invoice = new Invoice();     
      //Dispatcher_Request_Ref = new DispatcherRequest();
      //Scale_Ref = new Scale();           
    }
  }
}
