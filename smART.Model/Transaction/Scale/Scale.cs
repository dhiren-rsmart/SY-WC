// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {
  [Table("T_Scale")]
  public class Scale : BaseEntity {
    //=========================Scale============================================

    [StringLength(20, ErrorMessage = "Maximum legth is 20")]
    public string Scale_Ticket_No {
      get;
      set;
    }

    public string Ticket_Type {
      get;
      set;
    }

    public string Ticket_Status {
      get;
      set;
    }

    [StringLength(20, ErrorMessage = "Maximum legth is 20")]
    public string Vehicle_Type {
      get;
      set;
    }


    [StringLength(20, ErrorMessage = "Maximum legth is 20")]
    public string Truck_No {
      get;
      set;
    }

    [StringLength(25, ErrorMessage = "Maximum legth is 25")]
    public string Vehicle_Plate_No {
      get;
      set;
    }

    [StringLength(20, ErrorMessage = "Maximum legth is 25")]
    public string Trailer_Chasis_No {
      get;
      set;
    }

    public DispatcherRequest Dispatch_Request_No {
      get;
      set;
    }

    [StringLength(20, ErrorMessage = "Maximum legth is 25")]
    public string Other_Details {
      get;
      set;
    }

    [StringLength(90, ErrorMessage = "Maximum legth is 90")]
    public string Driver_Name {
      get;
      set;
    }

    public decimal Gross_Weight {
      get;
      set;
    }

    public decimal Tare_Weight {
      get;
      set;
    }

    public decimal Net_Weight {
      get;
      set;
    }

    public decimal Settlement_Diff_NetWeight {
      get;
      set;
    }

    public decimal Scale_Reading {
      get;
      set;
    }

    //============================Rec Ticket Additional Info===========================

    public Party Party_ID {
      get;
      set;
    }

    public PurchaseOrder Purchase_Order {
      get;
      set;
    }

    [StringLength(20, ErrorMessage = "Maximum legth is 20")]
    public string Supplier_Scale_Ticket_No {
      get;
      set;
    }

    public Asset Asset {
      get;
      set;
    }

    public AddressBook Party_Address {
      get;
      set;
    }

    //============================Shipping Ticket Details ===========================

    public Container Container_No {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Seal_No {
      get;
      set;
    }

    //Add by dhirendra mail ref
    public bool Ticket_Settled {
      get;
      set;
    }

    //============================Local Sales===========================                
    public SalesOrder Sales_Order {
      get;
      set;
    }
    public Party Local_Sales_AND_Trading_Party {
      get;
      set;
    }
    public Invoice Invoice {
      get;
      set;
    }

    ////============================Brokerage===========================                
    public Booking Booking {
      get;
      set;
    }

    // =======================Email ==========================
    public bool Send_Mail {
      get;
      set;
    }
    public DateTime? Mail_Send_On {
      get;
      set;
    }


    // ===============================QScale ==================

    [StringLength(100, ErrorMessage = "Maximum legth is 45")]
    public string Make {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 45")]
    public string Model {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 45")]
    public string Color {
      get;
      set;
    }

    [StringLength(10, ErrorMessage = "Maximum legth is 10")]
    public string Vehicle_Year {
      get;
      set;
    }

    public bool QScale {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    public string State {
      get;
      set;
    }

    public PriceList PriceList {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    public string Plate_State {
      get;
      set;
    }

    public bool Lead {
      get;
      set;
    }

  }
}
