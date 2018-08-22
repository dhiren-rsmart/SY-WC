// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using System.Web;

namespace smART.ViewModel
{
    public class Booking : BaseEntity
    {

        //=========================Booking=======================

        //Unique        
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        [DisplayName("Booking Number")]
        public string Booking_Ref_No { get; set; }

        [DisplayName("Sales Order#")]
        [UIHint("SalesOrderDropDownList")]
        public SalesOrder Sales_Order_No { get; set; }


        [DisplayName("Scale")]
        [UIHint("LOVDropDownList")]
        public string Scale_Name { get; set; }

        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        [DisplayName("Paper work to:")]
        public string Paper_Work_To { get; set; }

        [DisplayName("Forwarder/Party ID")]
        [UIHint("PartyDropDownList")]
        public Party Forwarder_Party_ID { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20.")]
        [DisplayName("Forwarder ref")]
        public string Forwarder_Ref_No { get; set; }

        [StringLength(45,ErrorMessage="Maximum length is 45.")]
        [DisplayName("Entered by")]
        //[UIHint("ContactDropDownList")]        
        //public Contact Entered_By { get; set; }
        public string Entered_By { get; set; }

        [DisplayName("Entered Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:ddd, MMM d, yyyy}")]
        public DateTime? Entered_Date { get; set; }
                
        [Integer]       
        [DisplayName("# of containers ordered")]
        public int No_Of_Containers { get; set; }

        //Display only based on formula
        [Integer]      
        [DisplayName("# of containers Assigned")]        
        public int  Containers_Assigned { get; set; }

        //Display only based on formula
        [Integer]        
        [DisplayName("# of Containers Due")]
        public int Containers_Due { get; set; }

               
        [DisplayName("Container Type")]
        [UIHint("LOVDropDownList")]
        public string Container_Type { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Invoice_Generated_Flag { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Invoice_Transfer_To_QB_Flag { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Amount_Received_Flag { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Invoice_Locked_Flag { get; set; }

        [DisplayName("Total Weight")]
        public decimal Total_Weight { get; set; }

        //=========================Shiping Details=======================

       
        [DisplayName("Place of receipt/Final Destination")]
        [UIHint("LOVDropDownList")]
        public string Final_Destination { get; set; }

        [DisplayName("Port of origin")]
        [UIHint("LOVDropDownList")]
        public string Port_Of_Origin { get; set; }

        [DisplayName("Destination Port")]
        [UIHint("LOVDropDownList")]
        public string Destination_Port { get; set; }

        [DisplayName("Shipping co.")]       
        [UIHint("PartyDropDownList")]
        public Party Shipping_Company { get; set; }
        
        [DisplayName("Vessel name")]
        [StringLength(18, ErrorMessage = "Maximum legth is 18")]
        public string Vessel_Name { get; set; }

        [DisplayName("Voyage#")]
        [StringLength(18, ErrorMessage = "Maximum legth is 18")]
        public string Voyage_No { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Pick up Date")]
        public DateTime? Pickup_Date { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Receive Date")]
        public DateTime? Receive_Date { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Cut off date")]
        public DateTime? Cutoff_Date { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Departure date")]
        public DateTime? Departure_Date { get; set; }

        //=========================Instructions=======================

        [DisplayName("Min. weight allowed")]
        public decimal Min_Weight { get; set; }

        [DisplayName("UOM")]
        [UIHint("LOVDropDownList")]
        public string Min_Weight_UOM { get; set; }

        [DisplayName("Max. weight allowed")]
        public decimal Max_Weight { get; set; }

        [DisplayName("UOM")]
        [UIHint("LOVDropDownList")]
        public string Max_Weight_UOM { get; set; }

        [DisplayName("No wooden pallet")]
        public bool Wooden_Pallet { get; set; }

        [DisplayName("Shipper")]
        [StringLength(500, ErrorMessage = "Maximum legth is 500")]        
        [DataType (DataType.MultilineText)]
         public string  Shipper { get; set; }

        [DisplayName("Notify")]
        [StringLength(500, ErrorMessage = "Maximum legth is 500")]
        [DataType(DataType.MultilineText )]
        public string Notify { get; set; }

        [DisplayName("Consignee")]
        [StringLength(500, ErrorMessage = "Maximum legth is 500")]
        [DataType(DataType.MultilineText)]
        public string Consignee { get; set; }

        [DisplayName("AES#")]
        [StringLength(50, ErrorMessage = "Maximum legth is 500")]
        public string AES_No { get; set; }

        [DisplayName("Shipping pick up location")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Shipping_Pickup_Location { get; set; }

        [DisplayName("Shipping return location")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Shipping_Return_Location { get; set; }

        [DisplayName("Rail pick up location")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Rail_Pickup_Location { get; set; }

        [DisplayName("Rail turn in location")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Rail_Tumin_Location { get; set; }

        //=========================Documentation=======================

        [UIHint("BlankDate")]
        [DisplayName("Original Docs sent to customer")]
        public DateTime? Doc_Set_Date { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Courier sent date")]
        public DateTime? Courier_Sent_Date { get; set; }

        [DisplayName("Courier Tracking #")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Courier_Tracking_No { get; set; }

        //=========================Documentation=======================

        [DisplayName("Booking Status")]
        [UIHint("LOVDropDownList")]
        public string Booking_Status { get; set; }

        [DisplayName("Booking Closed by")]
        [UIHint("ContactDropDownList")]
        public Contact Booking_Closed_By { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Booking Closed Date")]
        public DateTime? Booking_Closed_Date { get; set; }

        //=========================Inspection Info=======================

        [DisplayName("Agent Name")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Agent_Name { get; set; }

        [DisplayName("Agent Contact")]
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Agent_Contact { get; set; }

        [UIHint("BlankDate")]
        [DisplayName("Inspection Date")]
        public DateTime? Inspection_Date { get; set; }

        [DisplayName("Inspection Site")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Inspection_Site { get; set; }

        [DisplayName("Supplier Name")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Supplier_Name { get; set; }

        [DisplayName("Receiver Name")]
        [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        public string Reciver_Name { get; set; }

        public Booking()
            : base()
        {
            //Booking_Closed_By = new Contact();          
            //Shipping_Company = new Party();           
            //Forwarder_Party_ID = new Party();
            //Sales_Order_No = new SalesOrder();
            Entered_Date = DateTime.Now;
            Entered_By = HttpContext.Current!= null ? HttpContext.Current.User.Identity.Name:"";
            Booking_Ref_No = "0";
            
        }
    }
}
