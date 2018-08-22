using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel {
  public class RoleFeature : BaseEntity {
    [Required]
    [HiddenInput(DisplayValue = false)]
    public Role Role { get; set; }

    [Required]
    [ClientTemplateHtml("<span><#= Feature.FeatureName #></span>")]
    public Feature Feature { get; set; }

    [DisplayName("View")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= ViewAccessInd? \"checked='checked'\" : \"\" #> />")]
    public bool ViewAccessInd { get; set; }

    [DisplayName("New")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= NewAccessInd? \"checked='checked'\" : \"\" #> />")]
    public bool NewAccessInd { get; set; }

    [DisplayName("Edit")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= EditAccessInd? \"checked='checked'\" : \"\" #> />")]
    public bool EditAccessInd { get; set; }

    [DisplayName("Delete")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Print' <#= DeleteAccessInd? \"checked='checked'\" : \"\" #> />")]
    public bool DeleteAccessInd { get; set; }

    public RoleFeature() {
      Role = new Role();
      Feature = new Feature();
    }
  }
}
