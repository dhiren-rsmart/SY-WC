using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("M_Role"),Unique("Role_Name,Active_Ind")]
    public class Role: BaseEntity
    {
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
    }
}
