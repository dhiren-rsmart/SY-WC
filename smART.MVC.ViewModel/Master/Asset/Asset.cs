using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DataAnnotationsExtensions;

namespace smART.ViewModel {

  public class Asset : BaseEntity {
   
    [DisplayName("Asset#")]
    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_No { get; set; }

    [DisplayName("Asset Type")]
    [UIHint("LOVDropDownList")]
    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_Type { get; set; }

    [DisplayName("Asset Sub Type")]
    [UIHint("LOVDropDownList")]
    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Asset_Sub_Type { get; set; }

    [DisplayName("Asset purchase date")]
    public DateTime? Purchase_Date { get; set; }

    [DisplayName("Purchase amount")]
    public decimal Purchase_Aamount { get; set; }

    [DisplayName("Current value")]
    public decimal Current_Value { get; set; }

    [DisplayName("Active")]
    public bool Active { get; set; }

    public Asset() { }
  }
}
