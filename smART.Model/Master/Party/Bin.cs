using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("M_Bin")]
    public class Bin : PartyChildEntity
    {
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Bin_Value { get; set; }
    }
}
