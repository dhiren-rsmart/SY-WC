// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel {
  
  public class QBLog : BaseEntity {

    [DisplayName("Posting Date")]
    public DateTime? Posting_Date {
      get;
      set;
    }

    [DisplayName("Account No")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Account_No {
      get;
      set;
    }

    [DisplayName("Account Name")]
    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    public string Account_Name {
      get;
      set;
    }

    [Required]
    [DisplayName("Debit Amount")]
    public Decimal Debit_Amt {
      get;
      set;
    }

    [Required]
    [DisplayName("Crdit Amount")]
    public Decimal Credit_Amt {
      get;
      set;
    }

    [StringLength(200, ErrorMessage = "Maximum legth is 200")]
    [DisplayName("Remarks")]
    public string Remarks {
      get;
      set;
    }

    [Required]
    [DisplayName("Source Type")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Source_Type {
      get;
      set;
    }

    [Required]
    [DisplayName("Source ID")]
    public int Source_ID {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    [DisplayName("QB Ref No.")]
    public string QB_Ref_No {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    [DisplayName("RS Ref No.")]
    public string RS_Ref_No {
      get;
      set;
    }

    
    [Required]
    public int Parent_ID {
      get;
      set;
    }

    [Required]
    public int Group {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    [DisplayName("Status")]
    public string Status {
      get;
      set;
    }

    [StringLength(1000, ErrorMessage = "Maximum legth is 1000")]
    [DisplayName("Status Remarks")]
    public string Status_Remarks {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    public string Issues {
      get;
      set;
    }

    [StringLength(200, ErrorMessage = "Maximum legth is 200")]
    [DisplayName("Name")]
    public string Name {
      get;
      set;
    }

  }
}
