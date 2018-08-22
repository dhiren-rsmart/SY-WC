using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel
{
    public class InvoiceItem : InvoiceChildEntity
    {
        [Display(Name = "Container#")]
        public string Container_No { get; set; }

        [Display(Name = "Seal#")]
        public string Seal_No { get; set; }

        [Display(Name = "Item Name")]
        public string Item_Name { get; set; }

        [Display(Name = "Net Weight#")]
        public decimal Net_Weight { get; set; }

        [Display(Name = "UOM (SO)")]
        public string UOM_SO { get; set; }

        [Display(Name = "Weight Per SO UOM")]
        [DataType("decimal(16 ,4")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000}")] 
        public decimal SO_Item_UOM_NetWeight { get; set; }

        public decimal Price { get; set; }

        public decimal Total{get;set;}


    }
}
