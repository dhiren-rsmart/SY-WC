using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Purchase_Order_Items")]
    public class PurchaseOrderItem : PurchaseOrderChildEntity
    {
        //public int Item_ID { get; set; }
        public Item Item { get; set; }

        [Display(Name = "Item Override")]
        public string Item_Override { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Packaging_Type { get; set; }

        public decimal Ordered_Qty { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Ordered_Qty_UOM { get; set; }
               
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Price_Type { get; set; }

        public decimal Price { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Price_UOM { get; set; }

        public int No_Of_Containers { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Container_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Confirmed_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Expense_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Payment_Method { get; set; }

        public decimal Payment_Method_Amt { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Payment_Method_UOM { get; set; }


        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Material { get; set; }
    }
}
