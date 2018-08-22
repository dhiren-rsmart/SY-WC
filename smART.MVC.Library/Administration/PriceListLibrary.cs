using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;

namespace smART.Library
{
    public class PriceListLibrary : GenericLibrary<VModel.PriceList, Model.PriceList>
    {
        public PriceListLibrary() : base() { }
        public PriceListLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Party, Model.Party>();
            Mapper.CreateMap<Model.Party, VModel.Party>();

            Mapper.CreateMap<VModel.Contact, Model.Contact>();
            Mapper.CreateMap<Model.Contact, VModel.Contact>();
        }

        public override System.Linq.Expressions.Expression<Func<Model.PriceList, bool>> UniqueEntityExp(Model.PriceList modelEntity, VModel.PriceList businessEntity) {
          return m => m.PriceList_Name.Equals(modelEntity.PriceList_Name, StringComparison.InvariantCultureIgnoreCase)               
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }

        public IEnumerable<VModel.PriceList> GetActivePriceList() {

          return base.GetByExpression(i => i.Active == true && i.Active_Ind == true 
              &&  i.Effective_Date_From.HasValue && i.Effective_Date_To.HasValue &&  i.Effective_Date_From <= DateTime.Now && i.Effective_Date_To.Value >= DateTime.Now);
                
        }

    }
}
