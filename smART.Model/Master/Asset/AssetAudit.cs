using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("T_Asset_Audit")]
  public class AssetAudit : BaseEntity {
   
    public Asset Asset { get; set; }

    public Party Party { get; set; }

    public AddressBook Location { get; set; }

    public DateTime? Date { get; set; }

    public bool Asset_Current_Location_Flg { get; set; }

    public DispatcherRequest Dispatcher_Request { get; set; }

  }
}
