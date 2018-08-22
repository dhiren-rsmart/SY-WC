using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("M_Feature")]
    public class Feature: BaseEntity
    {
        [Required]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string FeatureName { get; set; }
    }
}
