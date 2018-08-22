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

  public class PaymentReceipt : BaseEntity {
    [DisplayName("Party Name")]
    [HiddenInput(DisplayValue = false)]
    public Party Party { get; set; }

    [StringLength(8, ErrorMessage = "Maximum legth is 50")]
    [DisplayName("Payment Type")]
    [UIHint("LOVDropDownList")]
    public string Payment_Receipt_Type { get; set; }

    [StringLength(8, ErrorMessage = "Maximum legth is 8")]
    [HiddenInput(DisplayValue = false)]
    public string Transaction_Type { get; set; }

    [DisplayName("Transaction Mode")]
    [UIHint("LOVDropDownList")]
    [StringLength(15, ErrorMessage = "Maximum legth is 15")]
    public string Transaction_Mode { get; set; }

    [UIHint("BlankDate")]
    [DisplayName("Transaction Date")]
    public DateTime Transaction_Date { get; set; }

    [DisplayName("Transaction Status")]
    [UIHint("LOVDropDownList")]
    [StringLength(30, ErrorMessage = "Maximum length is 30")]
    public string Transaction_Status { get; set; }

    [StringLength(30, ErrorMessage = "Maximum length is 30")]
    [DisplayName("Override Name")]
    public string Override_Name { get; set; }

    [DisplayName("Total Amount Due")]
    public decimal Total_Amount_Due { get; set; }

    [DisplayName("Cash Amount to Apply")]
    public decimal Cash_Amount { get; set; }

    [DisplayName("Bank Amount to Apply")]
    public decimal Bank_Amount { get; set; }

    [DisplayName("Amount to be Paid")]
    public decimal Total_Amount_Paid { get { return Cash_Amount + Bank_Amount; } }

    [DisplayName("Amount Applied")]
    public decimal Applied_Amount { get; set; }

    [DisplayName("Amount to be Applied")]
    public decimal Applied_Amount_To_Be { get { return (Total_Amount_Paid - Applied_Amount); } }

    [DisplayName("Expenses")]
    public decimal Expenses_Amt { get; set; }

    [Display(Name = "Net Payable")]
    public decimal Net_Amt { get; set; }

    [DisplayName("Bank Name")]
    [HiddenInput(DisplayValue = false)]
    public Bank Account_Name { get; set; }
       
    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    [DisplayName("Check# or Wire Transfer#")]
    public string Check_Wire_Transfer { get; set; }

    [DisplayName("Cash Drawer")]
    [UIHint("LOVDropDownList")]
    [StringLength(2, ErrorMessage = "Maximum length is 2")]
    public string Cash_Drawer { get; set; }

    [DisplayName("Check Print Count")]
    [HiddenInput(DisplayValue = false)]
    public int Check_Print_Count {
      get;
      set;
    }

    [DisplayName("Booking#")]
    [HiddenInput(DisplayValue = false)]
    public Booking Booking { get; set; }

    [DisplayName("Location")]
    [UIHint("AddressDropDownList")]
    [HiddenInput(DisplayValue = false)]
    public AddressBook Party_Address {get;set;}

    public PaymentReceipt() {
      //Party = new Party();
      //Account_Name = new Bank();
      Applied_Amount = 0;
      Transaction_Date = DateTime.Now;
    }
  }
}
