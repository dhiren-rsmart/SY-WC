using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model {

  [Table("T_QB_Log")]
  public class QBLog : BaseEntity {

    public DateTime? Posting_Date {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Account_No {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum legth is 100")]
    public string Account_Name {
      get;
      set;
    }

    [Required]
    public Decimal Debit_Amt {
      get;
      set;
    }

    [Required]
    public Decimal Credit_Amt {
      get;
      set;
    }

    [StringLength(200, ErrorMessage = "Maximum legth is 200")]
    public string Remarks {
      get;
      set;
    }

    [Required]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Source_Type {
      get;
      set;
    }

    [Required]
    public int Source_ID {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string QB_Ref_No {
      get;
      set;
    }

    [StringLength(50, ErrorMessage = "Maximum legth is 50")]    
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
    public string Status {
      get;
      set;
    }

    [StringLength(1000, ErrorMessage = "Maximum legth is 1000")]
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
    public string Name {
      get;
      set;
    }
  }
}
