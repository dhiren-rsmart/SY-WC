using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Model
{
    public abstract class PartyChildEntity: BaseEntity
    {
        public Party Party { get; set; }
    }
}
