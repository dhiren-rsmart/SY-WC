using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("M_Asset")]
  public class Asset : BaseEntity {

    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_No { get; set; }

    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_Type { get; set; }

    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_Sub_Type { get; set; }

    public DateTime? Purchase_Date { get; set; }

    public decimal Purchase_Aamount { get; set; }

    public decimal Current_Value { get; set; }

    public bool Active { get; set; }

  }
}
