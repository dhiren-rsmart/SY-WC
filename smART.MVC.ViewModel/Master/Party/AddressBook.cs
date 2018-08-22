using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace smART.ViewModel {
  public class AddressBook : PartyChildEntity {
    //[Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Address1 { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Address2 { get; set; }

    //[Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string City { get; set; }

    //[Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string State { get; set; }

    //[Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Country { get; set; }

    //[Required]
    [DisplayName("Zip")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Zip_Code { get; set; }


    [UIHint("LOVDropDownList")]
    [DisplayName("Address Type")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Address_Type { get; set; }

    [DisplayName("Is Primary Address")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Primary_Flag' <#= Primary_Flag? \"checked='checked'\" : \"\" #> />")]
    public bool Primary_Flag { get; set; }

    public string FullAddress {
      get { 
            return Address1 + 
                  (!string .IsNullOrEmpty(Address2) ?   ", " + Address2 : "" ) +
                  (!string.IsNullOrEmpty(City) ? ", " + City : "");
           }
    }

  }
}
