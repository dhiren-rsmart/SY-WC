using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library
{
    public class ItemLibrary : GenericLibrary<VModel.Item, Model.Item>
    {
        public ItemLibrary() : base() { }
        public ItemLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override System.Linq.Expressions.Expression<Func<Model.Item, bool>> UniqueEntityExp(Model.Item modelEntity, VModel.Item businessEntity) {
          return m => m.Short_Name.Equals(modelEntity.Short_Name, StringComparison.InvariantCultureIgnoreCase)
                      && m.Item_Category .Equals(modelEntity.Item_Category, StringComparison.InvariantCultureIgnoreCase)   
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }

        public IEnumerable<VModel.Item> GetActiveItems() {
          return base.GetByExpression(i => i.IsActive == true && i.Active_Ind == true);
        }
    }
}
