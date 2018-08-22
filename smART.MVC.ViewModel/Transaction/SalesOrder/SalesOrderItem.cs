using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace smART.ViewModel {
  public class SalesOrderItem : SalesOrderChildEntity {
    [ScaffoldColumn(false)]
    [Display(Name = "Item")]
    //public int Item_ID { get; set; }
    public Item Item { get; set; }

    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Item Override")]
    public Item Item_Override_Lookup { get; set; }
 
    [Display(Name = "Item Override")]
    public string Item_Override { get; set; }

    //public Party Party { get; set; }

    [UIHint("LOVDropDownList", "", "Packaging_Type")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Packaging")]
    public string Packaging_Type { get; set; }

    [UIHint("LOVDropDownList")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Quantity UOM")]
    public string Item_UOM { get; set; }

    [Display(Name = "Order Quantity")]
    public decimal Item_Qty { get; set; }

    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Display(AutoGenerateField = false, AutoGenerateFilter = false)]
    [ScaffoldColumn(false)]
    public decimal Brokerage { get; set; }

    [ScaffoldColumn(false)]
    public decimal Commission { get; set; }

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
  
    public SalesOrderItem()
      : base() {
      //Item = new Item();
      //Item_Override = new Item();
      //Item.AllowRequiredFields(this.GetType(), false);
    }
  }
}
