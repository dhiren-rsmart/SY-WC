using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Invoice_Items")]
    public class InvoiceItem : InvoiceChildEntity
    {
        public int Container_No { get; set; }

        public int Seal_No { get; set; }

        public string Item { get; set; }

        public decimal Net_Weight { get; set; }

        public string UOM_SO { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        [DataType("decimal(16 ,4")]
        public decimal SO_Item_UOM_NetWeight { get; set; }
    }
}
