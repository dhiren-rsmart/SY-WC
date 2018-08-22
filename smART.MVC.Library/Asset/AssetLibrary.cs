using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library {

  public class AssetLibrary : GenericLibrary<VModel.Asset, Model.Asset> {

    public AssetLibrary() : base() { }
    public AssetLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    public override System.Linq.Expressions.Expression<Func<Model.Asset, bool>> UniqueEntityExp(Model.Asset modelEntity, VModel.Asset businessEntity) {
      return m => m.Asset_Type.Equals(modelEntity.Asset_Type, StringComparison.InvariantCultureIgnoreCase)
                  && m.Asset_No.Equals(modelEntity.Asset_No, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }

  }
}
