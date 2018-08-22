using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Model
{
    public abstract class InvoiceChildEntity : BaseEntity
    {
        public Invoice Invoice { get; set; }
    }
}
