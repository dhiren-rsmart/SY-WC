// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

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
    public class SettlementDetails:BaseEntity
    {
        [DisplayName("Settlement")]
        [HiddenInput(DisplayValue = false)]
        public Settlement Settlement_ID { get; set; }

        [DisplayName("Scale Details")]
        [HiddenInput(DisplayValue = false)]
        public ScaleDetails Scale_Details_ID { get; set; }

        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        [DisplayName("Settlement Rate")]
        public decimal Rate { get; set; }

        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        [DisplayName("Amount Payable")]        
        public decimal Amount { get; set; }

        [DisplayName("Price List")]
        [HiddenInput(DisplayValue = false)]
        public PriceList Price_List_ID { get; set; }

        [DisplayName("NetWeight(LBS)")]
        public decimal Actual_Net_Weight { get; set; }

        [DisplayName("UOM")]
        public string Item_UOM { get; set; }

        [UIHint("LOVDropDownList")]       
        [DisplayName("NetWeight(LBS)")]
        public string Item_UOM_LOV { get { return Item_UOM; } set { Item_UOM = value; } }
        
        //[DisplayFormat(DataFormatString = "{0:0.##}")]  
        [DisplayName("NetWeight in per UOM")]
        [DataType("decimal(16 ,4")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000}")] 
        public decimal Item_UOM_NetWeight { get; set; }

        [DisplayName("Item UOM Conv Fact")]
        public decimal Item_UOM_Conv_Fact { get; set; }

        public SettlementDetails()
            : base()
        {
            //Settlement_ID = new Settlement();
            //Scale_Details_ID = new ScaleDetails();
            //Price_List_ID = new PriceList();

        }
    }
}
