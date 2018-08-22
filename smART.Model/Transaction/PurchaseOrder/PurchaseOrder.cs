using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{

    [Table("T_Purchase_Order")]
    public class PurchaseOrder : BaseEntity
    {
        public int Purchase_Order_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Status { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Closed_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Requested_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Created_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Scale_Broker { get; set; }

        //[StringLength(45, ErrorMessage = "Maximum length is 45")]
        public PriceList Price_List { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Location { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Delivery_Destination { get; set; }
                
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Ship_Via { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Payment_Terms { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Shipping_Terms { get; set; }

        public DateTime? Order_Date { get; set; }

        public DateTime? Order_Expiry_Date { get; set; }

        public DateTime? Delivery_Due_Date { get; set; }

        public int Qty_Variance { get; set; }

        public Party Party { get; set; }

        public Contact Contact { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 45")]
        public string Party_Order_Ref { get; set; }

    }
}
