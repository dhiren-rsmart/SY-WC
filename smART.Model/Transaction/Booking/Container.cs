using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("T_Container_Ref"), Unique("Container_No,Booking_Ref_No, Active_Ind")]
    public class Container : BaseEntity
    {
        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Container_No { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Container_Size { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Chasis_No { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Seal1_No { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Seal2_No { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Origin { get; set; }

        [StringLength(50, ErrorMessage = "Maximum legth is 50")]
        public string Destination { get; set; }
        
        public Booking Booking { get; set; }

        public DateTime Date_In { get; set; }

        public DateTime? Date_Out { get; set; }

        public decimal Gross_Weight { get; set; }

        public decimal Tare_Weight { get; set; }

        public decimal Net_Weight { get; set; }

        public int Item_ID { get; set; }

        public string Status { get; set; }
        
        public bool Send_Mail { get; set; }

        public DateTime? Mail_Send_On { get; set; }

    }
}
