using VModel = smART.ViewModel;
using Model = smART.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AutoMapper;
using Telerik.Web.Mvc;
using System.Linq.Expressions;

namespace smART.Library {
  public class RoleFeatureLibrary : GenericLibrary<VModel.RoleFeature, Model.RoleFeature> {
    public RoleFeatureLibrary()
      : base() {
    }

    public RoleFeatureLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
      Mapper.CreateMap<VModel.Feature, Model.Feature>();
      Mapper.CreateMap<Model.Feature, VModel.Feature>();
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
      Mapper.CreateMap<VModel.Feature, Model.Feature>();
      Mapper.CreateMap<Model.Feature, VModel.Feature>();
    }

    public IEnumerable<VModel.RoleFeature> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.RoleFeature> modEnumeration = _repository.FindByPaging<Model.RoleFeature>(out totalRows, o => o.Role.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.RoleFeature> busEnumeration = Mapper.Map<IEnumerable<Model.RoleFeature>, IEnumerable<VModel.RoleFeature>>(modEnumeration);

      return busEnumeration;

    }

    public IEnumerable<VModel.RoleFeature> GetAllByParentID(
      int parentId,
      string[] includePredicate = null) {
      IEnumerable<Model.RoleFeature> modEnumeration = _repository.Find<Model.RoleFeature>(o => o.Role.ID == parentId, includePredicate);
      IEnumerable<VModel.RoleFeature> busEnumeration = Mapper.Map<IEnumerable<Model.RoleFeature>, IEnumerable<VModel.RoleFeature>>(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.RoleFeature> GetByFeature(VModel.Feature feature) {
      IEnumerable<Model.RoleFeature> dbEntities = _repository.GetQuery<Model.RoleFeature>().Include("Feature").Include("Role").Where(o => o.Feature.ID == feature.ID && o.Active_Ind == true);

      IEnumerable<VModel.RoleFeature> busEntities = Mapper.Map<IEnumerable<Model.RoleFeature>, IEnumerable<VModel.RoleFeature>>(dbEntities);

      return busEntities;

    }

    public override VModel.RoleFeature Add(VModel.RoleFeature addObject) {
      VModel.RoleFeature insertedObjectBusiness = addObject;
      try {
        Model.RoleFeature newModObject = Mapper.Map<VModel.RoleFeature, Model.RoleFeature>(addObject);
        newModObject.Role = _repository.GetQuery<Model.Role>().SingleOrDefault(o => o.ID == addObject.Role.ID);
        newModObject.Feature = _repository.GetQuery<Model.Feature>().SingleOrDefault(o => o.ID == addObject.Feature.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.RoleFeature insertedObject = _repository.Add<Model.RoleFeature>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.RoleFeature, VModel.RoleFeature>(insertedObject);
          Added(insertedObjectBusiness, newModObject, _dbContext);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
        if (rethrow)
          throw ex;
      }
      return insertedObjectBusiness;
      //return base.Add(addObject);
    }

    protected override void Modify(Expression<Func<Model.RoleFeature, bool>> predicate, VModel.RoleFeature modObject, string[] includePredicate = null) {
      try {
        Model.RoleFeature newModObject = Mapper.Map<VModel.RoleFeature, Model.RoleFeature>(modObject);

        newModObject.Role = _repository.GetQuery<Model.Role>().SingleOrDefault(o => o.ID == modObject.Role.ID);
        newModObject.Feature = _repository.GetQuery<Model.Feature>().SingleOrDefault(o => o.ID == modObject.Feature.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.RoleFeature>(predicate, newModObject, includePredicate);
          _repository.SaveChanges();
          Modified(modObject, newModObject, _dbContext);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    public override void Delete(string id, string[] includePredicate = null) {
      int roleFeatureId = Int32.Parse(id);
      Expression<Func<Model.RoleFeature, bool>> predicate = m => m.ID == roleFeatureId;
      VModel.RoleFeature roleFeature = GetByID(id,new string[] { "Role", "Feature" });
      Model.RoleFeature modifyRoleFeature = Mapper.Map<VModel.RoleFeature, Model.RoleFeature>(roleFeature);
      _repository.Modify<Model.RoleFeature>(predicate, modifyRoleFeature, includePredicate, false);
      _repository.SaveChanges();
    }

    public override System.Linq.Expressions.Expression<Func<Model.RoleFeature, bool>> UniqueEntityExp(Model.RoleFeature modelEntity, VModel.RoleFeature businessEntity) {
      return m => m.Role.ID == modelEntity.Role.ID
                  && m.Feature.ID == modelEntity.Feature.ID
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }
  }
}
