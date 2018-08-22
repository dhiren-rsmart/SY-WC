using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;


namespace smART.ViewModel {
  public class Contact : PartyChildEntity {
    [Required]
    [DisplayName("First Name")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string First_Name { get; set; }

    [DisplayName("Last Name")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Last_Name { get; set; }

    [Email]
    [StringLength(100, ErrorMessage = "Maximum length is 100")]
    public string Email { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Mobile { get; set; }

    [DisplayName("Work Phone")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Work_Phone { get; set; }

    [DisplayName("Home Phone")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Home_Phone { get; set; }

    [DisplayName("Fax")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Fax_No { get; set; }

    [UIHint("LOVDropDownList")]
    [DisplayName("Role")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Role { get; set; }

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Name")]
    public override string ListText {
      get {
        return First_Name + " " + Last_Name;
      }
    }

    [HiddenInput(DisplayValue = false)]
    public override string ListValue {
      get {
        return ID.ToString();
      }
    }

    [DisplayName("Party Name")]
    public string ContactParty { get { if (Party != null) return Party.ListText; else return string.Empty; } }

    [DisplayName("Receive Emails")]
    public bool Receive_Emails { get; set; }
  }
}
