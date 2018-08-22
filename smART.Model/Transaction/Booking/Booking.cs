// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("T_Booking_Ref"), Unique("Booking_Ref_No, Active_Ind")]
    public class Booking : BaseEntity
    {
        //=========================Booking=======================

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Booking_Ref_No { get; set; }

        public SalesOrder Sales_Order_No { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Scale_Name { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Paper_Work_To { get; set; }

        public Party Forwarder_Party_ID { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Forwarder_Ref_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Entered_By { get; set; }

        public DateTime? Entered_Date { get; set; }

        public int No_Of_Containers { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Container_Type { get; set; }

        public bool Invoice_Generated_Flag { get; set; }

        public bool Invoice_Transfer_To_QB_Flag { get; set; }

        public bool Amount_Received_Flag { get; set; }

        public bool Invoice_Locked_Flag { get; set; }

        public decimal Total_Weight { get; set; }

        //=========================Shiping Details=======================

        //[StringLength(18, ErrorMessage = "Maximum length is 18")]
        public string Final_Destination { get; set; }

        //[StringLength(18, ErrorMessage = "Maximum length is 18")]
        public string Port_Of_Origin { get; set; }

        //[StringLength(18, ErrorMessage = "Maximum length is 18")]
        public string Destination_Port { get; set; }

       
        public Party Shipping_Company { get; set; }

        [StringLength(18, ErrorMessage = "Maximum length is 18")]
        public string Vessel_Name { get; set; }

        [StringLength(18, ErrorMessage = "Maximum length is 18")]
        public string Voyage_No { get; set; }

        public DateTime? Pickup_Date { get; set; }

        public DateTime? Receive_Date { get; set; }

        public DateTime? Cutoff_Date { get; set; }

        public DateTime? Departure_Date { get; set; }

        //=========================Instructions=======================

        public decimal Min_Weight { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Min_Weight_UOM { get; set; }
        
        public decimal Max_Weight { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Max_Weight_UOM { get; set; }

        public bool Wooden_Pallet { get; set; }

        [StringLength(500, ErrorMessage = "Maximum length is 500")]
        public string  Shipper { get; set; }

        [StringLength(500, ErrorMessage = "Maximum length is 500")]
        public string Notify { get; set; }

        [StringLength(500, ErrorMessage = "Maximum length is 500")]
        public string Consignee { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 500")]
        public string AES_No { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Shipping_Pickup_Location { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Shipping_Return_Location { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Rail_Pickup_Location { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Rail_Tumin_Location { get; set; }

        //=========================Documentation=======================

        public DateTime? Doc_Set_Date { get; set; }

        public DateTime? Courier_Sent_Date { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string  Courier_Tracking_No { get; set; }

        //=========================Documentation=======================

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Booking_Status { get; set; }

       
        public Contact Booking_Closed_By { get; set; }

        public DateTime? Booking_Closed_Date { get; set; }

        //=========================Inspection Info=======================

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Agent_Name { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Agent_Contact { get; set; }
               
        public DateTime? Inspection_Date { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Inspection_Site { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Supplier_Name { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string Reciver_Name { get; set; }


    }
}
