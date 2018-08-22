using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace smART.ViewModel
{
    public class DeviceSettings : BaseEntity
    {
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        [DisplayName("Device Unique ID")]
        public string Device_ID
        {
            get;
            set;
        }

        [DisplayName("Maximum Ticket ID")]
        public int MaxTicket_ID { get; set; }
    }
}
