using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace smART.ViewModel
{
 //[Bind(Exclude = "Unique_ID,Active_Ind,Created_By,Updated_By,Created_Date,Last_Updated_Date,Site_Org_ID,IsReadOnly")]
   public class PurchaseOrderItem : PurchaseOrderChildEntity
    {
        [ScaffoldColumn(false)]
        [Display(Name = "Item")]
        //public int Item_ID { get; set; }
        public Item Item { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Item Override")]
        public Item Item_Override_Lookup { get; set; }

        [Display(Name = "Item Override")]
        public string Item_Override { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Material")]
        public string Material { get; set; }

        [UIHint("LOVDropDownList", "", "Packaging_Type")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Packaging")]
        public string Packaging_Type { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Quantity UOM")]
        public string Ordered_Qty_UOM { get; set; }

        [Display(Name = "Order Quantity")]
        public decimal Ordered_Qty { get; set; }

        [UIHint("LOVDropDownList")]
        [Display(Name = "Price Type")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Price_Type { get; set; }

        public decimal Price { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Price UOM")]
        public string Price_UOM { get; set; }

        [Display(Name = "# Of Containers")]
        public int No_Of_Containers { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Container Type")]
        public string Container_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Confirmed By")]
        [ScaffoldColumn(false)]
        public string Order_Confirmed_By { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Expense Type")]
        [ScaffoldColumn(false)]
        public string Expense_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Payment Method")]
        [ScaffoldColumn(false)]
        public string Payment_Method { get; set; }

        [Display(Name = "Amount")]
        [ScaffoldColumn(false)]
        public decimal Payment_Method_Amt { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Amount UOM")]
        public string Payment_Method_UOM { get; set; }

        [Display(Name = "Amount")]
        [ScaffoldColumn(false)]
        public decimal Payment_Method_Amt1 { get { return (Payment_Method == "PerMonth") ? Payment_Method_Amt : 0; } set { Payment_Method_Amt = (Payment_Method == "PerMonth") ? value : Payment_Method_Amt; } }

        [Display(Name = "Amount")]
        [ScaffoldColumn(false)]
        public decimal Payment_Method_Amt2 { get { return (Payment_Method == "Percentage") ? Payment_Method_Amt : 0; } set { Payment_Method_Amt = (Payment_Method == "Percentage") ? value : Payment_Method_Amt; } }

        [Display(Name = "Amount")]
        [ScaffoldColumn(false)]
        public decimal Payment_Method_Amt3 { get { return (Payment_Method == "FixedAmount") ? Payment_Method_Amt : 0; } set { Payment_Method_Amt = (Payment_Method == "FixedAmount") ? value : Payment_Method_Amt; } }

        [Display(Name = "Amount")]
        [ScaffoldColumn(false)]
        public decimal Payment_Method_Amt4 { get { return (Payment_Method == "FixedAmountPerScaleTicket") ? Payment_Method_Amt : 0; } set { Payment_Method_Amt = (Payment_Method == "FixedAmountPerScaleTicket") ? value : Payment_Method_Amt; } }

        [Display(Name = "Price List")]
        [ScaffoldColumn(false)]
        public decimal PriceListID { get; set; }

         public PurchaseOrderItem ()
            : base()
        {
            //Item = new Item();
            //Item.AllowRequiredFields(this.GetType(), false);
        }
    }
}
