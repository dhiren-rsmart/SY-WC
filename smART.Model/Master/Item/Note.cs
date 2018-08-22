using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("Notes_Appointment")]
    public class Note : PartyChildEntity
    {
        public DateTime? Date { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Notes_Desc { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Reminder { get; set; }
    }
}
