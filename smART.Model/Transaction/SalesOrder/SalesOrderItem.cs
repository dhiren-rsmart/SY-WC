using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Sales_Order_Items")]
    public class SalesOrderItem : SalesOrderChildEntity
    {
        //public int Item_ID { get; set; }
        public Item Item { get; set; }

        //public Item Item_Override { get; set; }
        public string Item_Override { get; set; }

        public decimal Item_Qty { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Item_UOM { get; set; }

        public decimal Brokerage { get; set; }

        public decimal Commission { get; set; }

        

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Packaging_Type { get; set; }

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
    }
}
