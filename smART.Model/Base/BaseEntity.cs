using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq.Expressions;

namespace smART.Model {

  public abstract class BaseEntity {
    [Key]
    public int ID {
      get;
      set;
    }

    public int? Unique_ID {
      get;
      set;
    }

    [DefaultValue(true)]
    public bool Active_Ind {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum Length is 45")]
    public string Created_By {
      get;
      set;
    }

    [StringLength(45, ErrorMessage = "Maximum Length is 45")]
    public string Updated_By {
      get;
      set;
    }

    public DateTime? Created_Date {
      get;
      set;
    }

    public DateTime? Last_Updated_Date {
      get;
      set;
    }
    public int? Site_Org_ID {
      get;
      set;
    }
  }
}
