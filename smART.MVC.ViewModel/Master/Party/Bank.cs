using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DataAnnotationsExtensions;


namespace smART.ViewModel {
  public class Bank : PartyChildEntity {
    [Required]
    [DisplayName("Account #")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Account_No { get; set; }

    [Required]
    [DisplayName("Bank Name")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Name { get; set; }

    [Required]
    [DisplayName("Account Name")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Account_Name { get; set; }

    [DisplayName("Routing #")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Routing_No { get; set; }

    [DisplayName("Instruction")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Notes_Instruction { get; set; }

    [DisplayName("Address1")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Address1 { get; set; }

    [DisplayName("Address2")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Address2 { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string City { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string State { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Country { get; set; }

    [DisplayName("Phone")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Phone_No { get; set; }

    [DisplayName("Fax")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Fax_No { get; set; }

    [Email]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Email { get; set; }

    [Display(Name = "Closing Balance")]
    public decimal Closing_Balance { get; set; }

  }
}
