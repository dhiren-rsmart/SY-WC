using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;

namespace smART.Model {

  [Table("M_Focus_Area")]
  public class FocusArea : BaseEntity {

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Focus_Area {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Sub_Focus_Area {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string View_Name {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximum length is 100")]
    public string OrderBy_Clause {
      get;
      set;
    }

    [StringLength(10, ErrorMessage = "Maximum length is 10")]
     public string Focus_Area_Type {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
     public string Report_Name {
      get;
      set;
    }

    [StringLength(30, ErrorMessage = "Maximum length is 30")]
    public string DataSet_Name {
      get;
      set;
    }

    [StringLength(300, ErrorMessage = "Maximum length is 300")]
    public string Parameters {
      get;
      set;
    }
  }
}
