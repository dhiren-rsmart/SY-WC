using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model
{
    [Table("Role_Feature_Int"),Unique("ID,Active_Ind")]
    public class RoleFeature: BaseEntity
    {
        [Required]
        public Role Role { get; set; }

        [Required]
        public Feature Feature { get; set; }

        public bool ViewAccessInd { get; set; }
        public bool NewAccessInd { get; set; }
        public bool EditAccessInd { get; set; }
        public bool DeleteAccessInd { get; set; }
    }
}
