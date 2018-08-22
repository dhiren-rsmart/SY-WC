using VModel = smART.ViewModel;
using Model = smART.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AutoMapper;


namespace smART.Library
{
    public class FeatureLibrary : GenericLibrary<VModel.Feature, Model.Feature>
    {
        public FeatureLibrary() : base() { }
        public FeatureLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    }
}
