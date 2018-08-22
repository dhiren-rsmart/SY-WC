using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Model
{
    public abstract class SalesOrderChildEntity : BaseEntity
    {
        public SalesOrder SalesOrder { get; set; }
    }
}
