// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {
  [Table("T_Payment_Receipt")]
  public class PaymentReceipt : BaseEntity {
    public Party Party {
      get;
      set;
    }


    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Payment_Receipt_Type {
      get;
      set;
    }

    [StringLength(8, ErrorMessage = "Maximum legth is 8")]
    public string Transaction_Type {
      get;
      set;
    }

    [StringLength(15, ErrorMessage = "Maximum legth is 15")]
    public string Transaction_Mode {
      get;
      set;
    }

    public DateTime Transaction_Date {
      get;
      set;
    }

    [StringLength(30, ErrorMessage = "Maximum length is 30")]
    public string Transaction_Status {
      get;
      set;
    }

    [StringLength(30, ErrorMessage = "Maximum length is 30")]
    public string Override_Name {
      get;
      set;
    }

    public decimal Cash_Amount {
      get;
      set;
    }

    public decimal Bank_Amount {
      get;
      set;
    }

    public decimal Expenses_Amt {
      get;
      set;
    }

    public decimal Net_Amt {
      get;
      set;
    }

    public Bank Account_Name {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Check_Wire_Transfer {
      get;
      set;
    }

    [StringLength(2, ErrorMessage = "Maximum length is 2")]
    public string Cash_Drawer {
      get;
      set;
    }

    public bool Is_Print {
      get;
      set;
    }

    public int Check_Print_Count {
      get;
      set;
    }

    public Booking Booking { get; set; }

    public AddressBook Party_Address { get; set; }
  }
}
