using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;

namespace smART.Library
{
    public class BinLibrary : PartyChildLibrary<VModel.Bin, Model.Bin>
    {
        public BinLibrary() : base() { }
        public BinLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    }
}
