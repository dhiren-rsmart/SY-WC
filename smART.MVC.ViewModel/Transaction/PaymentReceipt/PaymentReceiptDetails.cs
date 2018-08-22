// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel {

  public class PaymentReceiptDetails : BaseEntity {
    [DisplayName("Ticket ID")]
    [HiddenInput(DisplayValue = false)]
    public Settlement Settlement { get; set; }

    [DisplayName("Expense ID")]
    [HiddenInput(DisplayValue = false)]
    public ExpensesRequest ExpenseRequest { get; set; }


    [DisplayName("Invoice ID")]
    [HiddenInput(DisplayValue = false)]
    public Invoice Invoice { get; set; }

    [DisplayName("Payment/Receipt ID")]
    [HiddenInput(DisplayValue = false)]
    public PaymentReceipt PaymentReceipt { get; set; }

    [DisplayName("Balance Amount")]
    public decimal Balance_Amount { get; set; }

    [DisplayName("100% Paid?")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Paid_In_Full' <#= Paid_In_Full? \"checked='checked'\" : \"\" #> />")]
    public bool Paid_In_Full { get; set; }

    [DisplayName("Apply Amount")]
    public decimal Apply_Amount { get; set; }

    [HiddenInput(DisplayValue = false)]
    public bool Is_Print { get; set; }

    //public string Booking_Ref_No {
    //  get {
    //    if (ExpenseRequest != null && ExpenseRequest.Dispatcher_Request_Ref != null &&  ExpenseRequest.Dispatcher_Request_Ref.Booking_Ref_No != null)
    //      return ExpenseRequest.Dispatcher_Request_Ref.Booking_Ref_No.Booking_Ref_No;
    //    else
    //      return "";
    //  }
    //}

    //public string Container_No {
    //  get {
    //    if (ExpenseRequest != null && ExpenseRequest.Dispatcher_Request_Ref != null && ExpenseRequest.Dispatcher_Request_Ref.Container != null)
    //      return ExpenseRequest.Dispatcher_Request_Ref.Container.Container_No;
    //    else
    //      return "";
    //  }
    //}

    public PaymentReceiptDetails() {
      //Settlement = new Settlement();
      //Invoice = new Invoice();
      //PaymentReceipt = new PaymentReceipt();
      //ExpenseRequest = new ExpensesRequest();
    }
  }
}
