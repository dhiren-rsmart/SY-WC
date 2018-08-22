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

namespace smART.ViewModel
{

    public class Scale : BaseEntity
    {
        //=========================Scale============================================

        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        [DisplayName("Ticket No #")]
        public string Scale_Ticket_No
        {
            get;
            set;
        }

        [DisplayName("Ticket Type")]
        [UIHint("LOVDropDownList")]
        public string Ticket_Type
        {
            get;
            set;
        }


        [DisplayName("Ticket Status")]
        [UIHint("LOVDropDownList")]
        public string Ticket_Status
        {
            get;
            set;
        }

        [DisplayName("Vehicle Type")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Vehicle_Type
        {
            get;
            set;
        }

        [DisplayName("Truck #")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Truck_No
        {
            get;
            set;
        }

        [DisplayName("Vehicle Plate #")]
        [StringLength(25, ErrorMessage = "Maximum legth is 25")]
        public string Vehicle_Plate_No
        {
            get;
            set;
        }

        [DisplayName("Trailer Chasis#")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Trailer_Chasis_No
        {
            get;
            set;
        }

        [DisplayName("Dispatch#")]
        [UIHint("DispatcherDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public DispatcherRequest Dispatch_Request_No
        {
            get;
            set;
        }

        [DisplayName("Other Info")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Other_Details
        {
            get;
            set;
        }

        [DisplayName("Driver Name")]
        [StringLength(90, ErrorMessage = "Maximum legth is 90")]
        public string Driver_Name
        {
            get;
            set;
        }

        [DisplayName("Driver Name")]
        [HiddenInput(DisplayValue = false)]
        public Contact Contact
        {
            get;
            set;
        }


        [DisplayName("Gross Weight")]
        public decimal Gross_Weight
        {
            get;
            set;
        }

        [DisplayName("Tare Weight")]
        public decimal Tare_Weight
        {
            get;
            set;
        }

        [DisplayName("Net Weight")]
        public decimal Net_Weight
        {
            get;
            set;
        }

        [DisplayName("Settlement Diff.")]
        public decimal Settlement_Diff_NetWeight
        {
            get;
            set;
        }

        //============================Rec Ticket Additional Info===========================
        [DisplayName("Party")]
        [UIHint("PartyDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public Party Party_ID
        {
            get;
            set;
        }

        public string Party_Name
        {
            get
            {
                return Party_ID != null ? Party_ID.Party_Name : "";
            }
        }

        [DisplayName("PO#")]
        [UIHint("DispatcherDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public PurchaseOrder Purchase_Order
        {
            get;
            set;
        }

        //[DisplayName("PO Item#")]
        //[UIHint("PurchaseOrderItemDropDownList")]
        //[HiddenInput(DisplayValue = false)]
        //public PurchaseOrderItem Purchase_Order_Item {
        //  get;
        //  set;
        //}


        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        [DisplayName("Supplier Scale Ticket #")]
        public string Supplier_Scale_Ticket_No
        {
            get;
            set;
        }

        //[DisplayName("Bin#")]
        //[StringLength(20, ErrorMessage = "Maximum legth is 25")]
        //public string Asset_ID {
        //  get;
        //  set;
        //}

        [DisplayName("Bin#")]
        [UIHint("AssetDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public Asset Asset
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public AssetAuditLookup AssetAuditLookup
        {
            get;
            set;
        }

        [DisplayName("Location")]
        [UIHint("AddressDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public AddressBook Party_Address
        {
            get;
            set;
        }

        [DisplayName("Net Weight PO UOM")]
        //[HiddenInput(DisplayValue = false)]
        public decimal Net_Weight_POUOM
        {
            get;
            set;
        }

        //============================Shipping Ticket Details ===========================

        [DisplayName("Container#")]
        [UIHint("ContainerDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public Container Container_No
        {
            get;
            set;
        }
        public string ContainerNo
        {
            get
            {
                return Container_No != null ? Container_No.Container_No : "";
            }
        }


        [DisplayName("Seal#")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Seal_No
        {
            get;
            set;
        }


        //[DisplayName("Gross Weight")]
        //public decimal Shiping_Gross_Weight { get { return Gross_Weight; } }

        //[DisplayName("Tare Weight")]
        //public decimal Shiping_Tare_Weight { get { return Tare_Weight; } }

        //[DisplayName("Net Weight")]
        //public decimal Shiping_Net_Weight { get { return Net_Weight; } }

        //[DisplayName("Diff Net Weight")]
        //public decimal Shiping_Diff_Net_Weight { get { return Diff_Net_Weight; } }


        [DisplayName("Sttled")]
        [HiddenInput(DisplayValue = false)]
        public bool Ticket_Settled
        {
            get;
            set;
        }

        //[DisplayName("Shipping Item")]
        //[UIHint("ItemDropDownList")]
        //[HiddenInput(DisplayValue = false)]
        //public Item Shipping_Item_ID { get; set; }

        [DisplayName("Scale Reading")]
        [HiddenInput(DisplayValue = false)]
        public Decimal Scale_Reading
        {
            get;
            set;
        }

        [DisplayName("Net Weight SO UOM")]
        //[HiddenInput(DisplayValue = false)]
        public decimal Net_Weight_SOUOM
        {
            get;
            set;
        }
        //============================Local Sales===========================
        [DisplayName("Party")]
        [UIHint("PartyDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public Party Local_Sales_AND_Trading_Party
        {
            get;
            set;
        }

        [DisplayName("SO#")]
        [UIHint("SalesOrderDropDownList")]
        [HiddenInput(DisplayValue = false)]
        public SalesOrder Sales_Order
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public Invoice Invoice
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayName("Send Email")]
        public bool Send_Mail
        {
            get;
            set;
        }

        [DisplayName("Send On")]
        [HiddenInput(DisplayValue = false)]
        public DateTime? Mail_Send_On
        {
            get;
            set;
        }


        //============================Brokerage===========================   
        [HiddenInput(DisplayValue = false)]
        public Booking Booking
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public Party Brokerage_Party
        {
            get;
            set;
        }
        [HiddenInput(DisplayValue = false)]
        public PurchaseOrder Brokerage_Purchase_Order
        {
            get;
            set;
        }


        // =================Common==================================
        [HiddenInput(DisplayValue = false)]
        [DisplayName("Party Name")]
        public Party Scale_Type_Party
        {
            get
            {
                if ((Ticket_Type == "Receiving Ticket" || Ticket_Type == "Brokerage") && Party_ID != null)
                {
                    return Party_ID;
                }
                else if (Ticket_Type == "Shipping Ticket" && Container_No != null && Container_No.Booking != null && Container_No.Booking.Sales_Order_No != null && Container_No.Booking.Sales_Order_No.Party != null)
                {
                    return Container_No.Booking.Sales_Order_No.Party;
                }
                else if ((Ticket_Type == "Local Sales" || Ticket_Type == "Trading") && Local_Sales_AND_Trading_Party != null)
                {
                    return Local_Sales_AND_Trading_Party;
                }
                else
                    return null;
            }
        }

        // =================================QScale ==================

        [HiddenInput(DisplayValue = false)]
        [DisplayName("Make")]
        [UIHint("LOVDropDownList")]
        public string Make
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayName("Model")]
        [UIHint("LOVDropDownList")]
        public string Model
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayName("Color")]
        [UIHint("LOVDropDownList")]
        public string Color
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayName("Year")]
        [UIHint("LOVDropDownList")]
        public string Vehicle_Year {
          get;
          set;
        }

        [HiddenInput(DisplayValue = false)]
        public decimal Item_GW
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public decimal Item_TW
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public decimal Item_Adjestment
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DataType("decimal(16 ,4")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Item_Price
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Item_Amount
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public bool QScale
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [StringLength(100, ErrorMessage = "Maximum legth is 100")]
        [DisplayName("State")]
        [UIHint("LOVDropDownList")]
        public string State
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public PriceList PriceList
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        [StringLength(100, ErrorMessage = "Maximum legth is 100")]
        [DisplayName("Car Plate - State")]
        [UIHint("LOVDropDownList")]
        public string Plate_State
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public string License_No
        {
            get;
            set;
        }

        [HiddenInput(DisplayValue = false)]
        public bool Payment { get; set; }

        [HiddenInput(DisplayValue = false)]
        public AddressBook PrimaryAddress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Lead { get; set; }

        public Scale()
            : base()
        {
            //Dispatch_Request_No = new DispatcherRequest();
            //Party_ID = new Party();
            //Purchase_Order = new PurchaseOrder();
            //Container_No = new Container();
            //Contact = new Contact();
            //Party_Address = new AddressBook();
            //Local_Sales_AND_Trading_Party = new Party();
            //Sales_Order = new SalesOrder();
            //Invoice = new Invoice();
            //Asset = new Asset();
        }
    }
}
