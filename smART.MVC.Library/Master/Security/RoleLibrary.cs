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
  public class RoleLibrary : GenericLibrary<VModel.Role, Model.Role> {
    public RoleLibrary()
      : base() {
    }
    public RoleLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public virtual IEnumerable<VModel.RoleFeature> GetRoleFeatures(int roleID) {
      Model.Role role = _repository.GetQuery<Model.Role>().SingleOrDefault(m => m.ID == roleID);

      IEnumerable<Model.RoleFeature> modroleFeatures = (from roleFeatures in _repository.GetQuery<Model.RoleFeature>()
                                                        where roleFeatures.Role.ID == roleID
                                                        select roleFeatures).Include(m => m.Feature).Include(m => m.Role).AsEnumerable();

      IEnumerable<int> featureIDs = modroleFeatures.Select(m => m.Feature.ID);

      IEnumerable<Model.Feature> modFeatures = (from feature in _repository.GetQuery<Model.Feature>()
                                                where !featureIDs.Contains(feature.ID)
                                                select feature).AsEnumerable();

      List<Model.RoleFeature> unusedFeatures = new List<Model.RoleFeature>();
      foreach (Model.Feature feature in modFeatures) {
        Model.RoleFeature roleFeature = new Model.RoleFeature();
        roleFeature.Role = role;
        roleFeature.Feature = feature;
        roleFeature.ViewAccessInd = roleFeature.EditAccessInd = roleFeature.DeleteAccessInd = roleFeature.NewAccessInd = false;

        unusedFeatures.Add(roleFeature);
      }

      modroleFeatures = modroleFeatures.Union(unusedFeatures);

      IEnumerable<VModel.RoleFeature> busroleFeatures = Mapper.Map<IEnumerable<Model.RoleFeature>, IEnumerable<VModel.RoleFeature>>(modroleFeatures);

      return busroleFeatures;
    }

    public virtual bool DeleteRoleFeatures(int roleID, IEnumerable<VModel.RoleFeature> roleFeatures) {
      foreach (VModel.RoleFeature roleFeature in roleFeatures) {
        _repository.Delete<Model.RoleFeature>(m => m.Role.ID == roleID && m.Feature.ID == roleFeature.Feature.ID, true);
      }
      return true;
    }

    public virtual bool SetRoleFeatures(int roleID, IEnumerable<VModel.RoleFeature> roleFeatures) {
      bool retVal = true;
      try {
        Model.Role role = _repository.GetQuery<Model.Role>().SingleOrDefault(m => m.ID == roleID);

        foreach (VModel.RoleFeature roleFeature in roleFeatures) {
          bool isNew = false;
          Model.Feature feature = _repository.GetQuery<Model.Feature>().SingleOrDefault(m => m.ID == roleFeature.Feature.ID);
          Model.RoleFeature rFeature = null;
          rFeature = _repository.GetQuery<Model.RoleFeature>().SingleOrDefault(m => m.Role.ID == roleID && m.Feature.ID == feature.ID);

          if (rFeature == null) {
            rFeature = new Model.RoleFeature();
            rFeature.Role = role;
            rFeature.Feature = feature;
            isNew = true;
          }
          rFeature.ViewAccessInd = roleFeature.ViewAccessInd;
          rFeature.EditAccessInd = roleFeature.EditAccessInd;
          rFeature.DeleteAccessInd = roleFeature.DeleteAccessInd;
          rFeature.NewAccessInd = roleFeature.NewAccessInd;

          if (isNew)
            _repository.Add<Model.RoleFeature>(rFeature);
          else
            _repository.Modify<Model.RoleFeature>(m => m.Role.ID == roleID && m.Feature.ID == feature.ID, rFeature);
        }

        _repository.SaveChanges();
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
      return retVal;
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Feature, Model.Feature>();
      Mapper.CreateMap<Model.Feature, VModel.Feature>();
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
    }

    public override System.Linq.Expressions.Expression<Func<Model.Role, bool>> UniqueEntityExp(Model.Role modelEntity, VModel.Role businessEntity) {
      return m => m.Role_Name.Equals(modelEntity.Role_Name, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }

    //public override VModel.Role GetByID(string id, string[] includePredicate = null)
    //{
    //    VModel.Role role = base.GetByID(id, includePredicate);
    //    role.Features = GetRoleFeatures(role.ID);
    //    return role;
    //}
  }
}
