using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{

    [Table("T_Sales_Order")]
    public class SalesOrder : BaseEntity
    {
        public int Sales_Order_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_ConfirmedBy { get; set; }

        public DateTime? Due_Date { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Payment_Terms { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Shipping_Terms { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Delivery_Destination { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Ship_Via { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Location { get; set; }

        public Party Party { get; set; }

        public Contact Contact { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is 45")]
        public string Party_Order_Ref { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Status { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Closed_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Requested_By { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_Created_By { get; set; }

        public DateTime? Order_Date { get; set; }

        public DateTime? Order_Expired_By { get; set; }

        public DateTime? Delivery_Due_Date { get; set; }

        public int Qty_Variance { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Scale_Broker {
          get;
          set;
        }
        //public IEnumerable<SalesOrderItem> Items  { get; set; }
    }
}
