using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("M_Employee"), Unique("Emp_Name, User_ID, Active_Ind")]
    public class Employee : BaseEntity
    {
        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string User_ID { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Password { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Emp_Name { get; set; }

        public string SSN { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public DateTime? Date_Of_Join { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Email { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Email_Password { get; set; }
    }
}
