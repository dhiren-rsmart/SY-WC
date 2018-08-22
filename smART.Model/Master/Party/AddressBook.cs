using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("M_Address")]
    public class AddressBook : PartyChildEntity
    {
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Address1 { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Address2 { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string City { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string State { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Country { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Zip_Code { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Address_Type { get; set; }

        public bool Primary_Flag { get; set; }
    }
}
