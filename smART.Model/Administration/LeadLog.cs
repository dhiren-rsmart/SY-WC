using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{

    [Table("T_Lead_Log")]
    public class LeadLog : BaseEntity
    {

       [StringLength(20, ErrorMessage = "Maximum legth is 20")]
        [Required]
        public string Scale_Ticket_No
        {
            get;
            set;
        }


        [StringLength(100, ErrorMessage = "Maximum legth is 100")]
        public string Status
        {
            get;
            set;
        }

        [StringLength(1000, ErrorMessage = "Maximum legth is 1000")]
        public string Status_Remarks
        {
            get;
            set;
        }

    }
}
