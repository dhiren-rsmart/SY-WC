using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace smART.ViewModel
{
    public class Bin : PartyChildEntity
    {
        [Required]
        [DisplayName("Bin #")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Bin_Value { get; set; }
    }
}
