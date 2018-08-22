using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace smART.ViewModel
{
    public class Note : PartyChildEntity
    {
        [Required]
        public DateTime? Date { get; set; }

        [Required]
        [DisplayName("Description")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Notes_Desc { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Reminder { get; set; }
    }
}
