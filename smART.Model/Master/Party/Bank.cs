using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using smART.Common;

namespace smART.Model {
  [Table("M_Bank")]
  public class Bank : PartyChildEntity {
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Account_No { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Name { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Account_Name { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Routing_No { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Notes_Instruction { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Address1 { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Bank_Address2 { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string City { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string State { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [DefaultValue("USA")]
    public string Country { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Phone_No { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Fax_No { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Email { get; set; }

    public decimal Closing_Balance { get; set; }
  }
}
