using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("M_Contact"), Unique("First_Name, Last_Name,Party_ID, Active_Ind")]
    public class Contact : PartyChildEntity
    {
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string First_Name { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Last_Name { get; set; }

        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Email { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Mobile { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Work_Phone { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Home_Phone { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Fax_No { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Role { get; set; }
        
        public bool Receive_Emails { get; set; }
    }
}
