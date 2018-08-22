using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel {

  public class AssetAudit : BaseEntity, IListType {

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Asset")]
    [UIHint("PartyDropDownList")]
    public Asset Asset { get; set; }

    [DisplayName("Party Name")]
    [UIHint("PartyDropDownList")]
    public Party Party { get; set; }

    [DisplayName("Party Location")]
    [UIHint("PartyLocationDropDownList")]
    public AddressBook Location { get; set; }

    [DisplayName("Date")]
    public DateTime? Date { get; set; }

    [DisplayName("Currently at this location")]
    [ClientTemplateHtml("<input type='checkbox' disabled='disabled' name='Asset_Current_Location_Flg' <#= Asset_Current_Location_Flg? \"checked='checked'\" : \"\" #> />")]
    public bool Asset_Current_Location_Flg { get; set; }

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Dispatcher ID")]
    [UIHint("PartyDropDownList")]
    public DispatcherRequest Dispatcher_Request { get; set; }

    public AssetAudit() {
      Asset = new Asset();
      Party = new Party();
      Location = new AddressBook();
      Dispatcher_Request = new DispatcherRequest();
    }


    #region IListType Members

    [HiddenInput(DisplayValue = false)]
    public string ListText {
      get { return ID.ToString(); }
    }

    [HiddenInput(DisplayValue = false)]
    public string ListValue {
      get { return ID.ToString(); }
    }

    #endregion
  }

  public class AssetAuditLookup : BaseEntity {

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Asset")]
    [UIHint("PartyDropDownList")]
    public Asset Asset { get; set; }

    [DisplayName("Party Name")]
    [UIHint("PartyDropDownList")]
    public Party Party { get; set; }

    [DisplayName("Party Location")]
    [UIHint("PartyLocationDropDownList")]
    public AddressBook Location { get; set; }

    public AssetAuditLookup() {
      Asset = new Asset();
      Party = new Party();
      Location = new AddressBook();
    }
  }

}
