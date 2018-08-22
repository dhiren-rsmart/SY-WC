using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace smART.ViewModel
{
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

        public List<Role> AvailableRoles { get; set; }
        public List<Role> EmployeeRoles { get; set; }

        public int[] AvailableRolesSelected { get; set; }
        public int[] EmployeeRolesSelected { get; set; }
       
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Email")]
        public string Email { get; set; }
      
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Email Password")]
        public string Email_Password { get; set; }
    }
}
