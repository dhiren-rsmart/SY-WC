using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("T_Dispatcher"), Unique("RequestCategory,RequestType,Container_No, Active_Ind")]
    public class DispatcherRequest : BaseEntity
    {
        public Party TruckingCompany { get; set; }

        public Contact Driver { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string RequestCategory { get; set; }

        public int Request_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string RequestType { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string RequestStatus { get; set; }

        public Party Party { get; set; }

        public Party Shipper { get; set; }

        public AddressBook Location { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Bin_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Order_No { get; set; }

        public SalesOrder Sales_Order_No { get; set; }

        public PurchaseOrder Purchase_Order_No { get; set; }
     
        public Booking Booking_Ref_No { get; set; }

        //[StringLength(45, ErrorMessage = "Maximum length is 45")]
        //public string Container_No { get; set; }

        public DateTime? Time { get; set; }

        public decimal? Amount_To_Be_Paid { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public Asset Asset { get; set; }

        public Container Container { get; set; }
    }
}
