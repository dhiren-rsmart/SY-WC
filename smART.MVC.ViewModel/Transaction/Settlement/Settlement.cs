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
    public class Settlement:BaseEntity
    {

        [DisplayName("Scale#")]
        [HiddenInput(DisplayValue = false)]
        public Scale Scale { get; set; }

        [DisplayName("Amount")]
        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        public decimal Amount { get; set; }

        [DisplayName("Ready For Payment")]
        public bool Ready_For_Payment { get; set; }

        [DisplayName("NetWeight")]
        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        public decimal Actual_Net_Weight { get; set; }

        [DisplayName("Amount Paid")]
        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        public decimal Amount_Paid_Till_Date { get; set; }

        [DisplayName("Balance Amount")]
        [DataType("decimal(18 ,2")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")] 
        public decimal Balance_Amount { get { return Amount - Amount_Paid_Till_Date; } }


        public Settlement()
            : base()
        {
            //Scale = new Scale();


        }
    }
}
