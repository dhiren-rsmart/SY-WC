using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel
{
    public class Role: BaseEntity
    {
        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Role_Name { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Role_Description { get; set; }

        public IEnumerable<RoleFeature> Features { get; set; }
    }
}
