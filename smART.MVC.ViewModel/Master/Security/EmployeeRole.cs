using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace smART.ViewModel
{
    public class EmployeeRole: BaseEntity
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Employee Employee { get; set; }

        [Required]
        [ClientTemplateHtml("<span><#= Role.Role_Description #></span>")]
        public Role Role { get; set; }
    }
}
