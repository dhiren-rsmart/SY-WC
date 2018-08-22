using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("Emp_Role_Int")]
    public class EmployeeRole: BaseEntity
    {
        public Employee Employee { get; set; }
        public Role Role { get; set; }
    }
}
