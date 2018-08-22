using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using smART.Common;
using System.Linq.Expressions;

namespace smART.Model {
  [Table("M_LOV"), Unique("LOVType_Name,Active_Ind")]
  public class LOVType : BaseEntity {
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string LOVType_Name {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string LOVType_Description {
      get;
      set;
    }

   public  LOVType ParentType {
      get;
      set;
    }
  }
}
