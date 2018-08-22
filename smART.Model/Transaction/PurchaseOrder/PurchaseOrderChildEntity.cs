using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Model
{
    public abstract class PurchaseOrderChildEntity : BaseEntity
    {
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
