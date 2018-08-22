using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel
{
    public class Invoice_BookingLookupEntity : Booking
    {
        public int SalesOrderID { get; set; }
        public string  PartyName { get; set; }
    }
}
