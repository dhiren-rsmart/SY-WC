using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using smART.Common;

namespace smART.Model {
  [Table("M_Item"), Unique("Short_Name, Active_Ind")]
  public class Item : BaseEntity {
    [Required]
    [StringLength(25, ErrorMessage = "Maximum length is 45")]
    public string Item_Category {
      get;
      set;
    }

    [Required]
    [StringLength(25, ErrorMessage = "Maximum length is 45")]
    public string Item_Group {
      get;
      set;
    }

    [StringLength(25, ErrorMessage = "Maximum length is 25")]
    public string Short_Name {
      get;
      set;
    }

    [StringLength(100, ErrorMessage = "Maximium lenght is 100")]
    public string Long_Name {
      get;
      set;
    }

    [DefaultValue(false)]
    public bool Priced {
      get;
      set;
    }

    [DefaultValue(false)]
    public bool Regulated_Item {
      get;
      set;
    }

    [DefaultValue(false)]
    public bool Require_VIN {
      get;
      set;
    }

    public Decimal Opening_Balance {
      get;
      set;
    }

    public Decimal Current_Balance {
      get;
      set;
    }

    [DisplayName("Active")]
    public bool IsActive {
      get;
      set;
    }

  }
}
