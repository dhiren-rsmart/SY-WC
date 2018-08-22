using VModel = smART.ViewModel;
using Model = smART.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AutoMapper;

namespace smART.Library
{
    public class UOMConversionLibrary : GenericLibrary<VModel.UOMConversion, Model.UOMConversion>
    {
        public UOMConversionLibrary() : base() { }
        public UOMConversionLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public VModel.UOMConversion GetByUOM(string converUOM,string baseUOM)
        {
            IEnumerable<Model.UOMConversion> modEnt = from entities in _repository.GetQuery<Model.UOMConversion>()
                                                      where entities.Conversion_UOM.Equals(converUOM, StringComparison.OrdinalIgnoreCase)
                                                            && entities.Base_UOM .Equals(baseUOM,StringComparison.OrdinalIgnoreCase)
                                                            && entities.Is_Base_UOM==true  
                                                      select entities;
            IEnumerable<VModel.UOMConversion> busEnt = Map(modEnt);
            return busEnt.FirstOrDefault();
        }

        public override System.Linq.Expressions.Expression<Func<Model.UOMConversion, bool>> UniqueEntityExp(Model.UOMConversion modelEntity, VModel.UOMConversion businessEntity) {
          return m => m.Conversion_UOM.Equals(modelEntity.Conversion_UOM,StringComparison.InvariantCultureIgnoreCase)
                      && m.Base_UOM.Equals(modelEntity.Base_UOM,StringComparison.InvariantCultureIgnoreCase)
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }

    }
}
