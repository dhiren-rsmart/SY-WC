using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace smART.Model
{
    [Table("M_DeviceSettings")]
    public class DeviceSettings : BaseEntity
    {
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Device_ID
        {
            get;
            set;
        }

        public int MaxTicket_ID { get; set; }
    }
}
