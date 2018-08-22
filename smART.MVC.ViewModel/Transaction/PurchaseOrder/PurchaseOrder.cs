using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace smART.ViewModel
{
    public class PurchaseOrder : FormatedBaseEntity
    {
        public int Purchase_Order_No { get; set; }
                
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Type")]
        public string Order_Type { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Status")]
        public string Order_Status { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Closed By")]
        public string Order_Closed_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Requested By")]
        public string Order_Requested_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Order Created By")]
        public string Order_Created_By { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Scale/Broker")]
        public string Scale_Broker { get; set; }

        [UIHint("PartyDropDownList")]
        //[StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Price List")]
        public PriceList Price_List { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Party Location")]
        public string Party_Location { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Delivery Destination")]
        public string Delivery_Destination { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Ship Via")]
        public string Ship_Via { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Payment Terms")]
        public string Payment_Terms { get; set; }

        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Shipping Terms")]
        public string Shipping_Terms { get; set; }

        [UIHint("BlankDate")]
        [Display(Name = "Order Date")]
        public DateTime? Order_Date { get; set; }

        [UIHint("BlankDate")]
        [Display(Name = "Order Expiry Date")]
        public DateTime? Order_Expiry_Date { get; set; }

        [UIHint("BlankDate")]
        [Display(Name = "Delivery Due Date")]
        public DateTime? Delivery_Due_Date { get; set; }

        [Display(Name = "Quantity Variance %")]
        public int Qty_Variance { get; set; }

        [UIHint("PartyDropDownList")]
        public Party Party { get; set; }

        [UIHint("ContactDropDownList")]
        public Contact Contact { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 45")]
        [Display(Name = "Party Order Ref")]
        public string Party_Order_Ref { get; set; }

        public PurchaseOrder()
            : base()
        {
            //Party = new Party();
            //Contact = new Contact();
            //Price_List = new PriceList();
            Updated_By = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "";
            Order_Created_By = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "";
            Order_Date = DateTime.Now;   
        }
    }
}
