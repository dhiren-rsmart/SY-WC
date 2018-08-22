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
  public class LOVLibrary : GenericLibrary<VModel.LOV, Model.LOV> {
    public LOVLibrary()
      : base() {
    }

    public LOVLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      Mapper.CreateMap<VModel.LOVType, Model.LOVType>();
      Mapper.CreateMap<Model.LOVType, VModel.LOVType>();
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.LOVType, Model.LOVType>();
      Mapper.CreateMap<Model.LOVType, VModel.LOVType>();
    }

    public IEnumerable<VModel.LOV> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.LOV> modEnumeration = _repository.FindByPaging<Model.LOV>(out totalRows, o => o.LOVType.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.LOV> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }

    public IEnumerable<VModel.LOV> GetAllByParentID(
      int parentId,
      string[] includePredicate = null) {
      IEnumerable<Model.LOV> modEnumeration = _repository.Find<Model.LOV>(o => o.LOVType.ID == parentId && o.LOV_Active==true, includePredicate);
      IEnumerable<VModel.LOV> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.LOV> GetByLOVType(string LovType) {
      IEnumerable<Model.LOV> dbEntities = _repository.GetQuery<Model.LOV>().Include("LOVType").Where(o => o.LOVType.LOVType_Name == LovType && o.LOV_Active == true);
      IEnumerable<VModel.LOV> busEntities = Map(dbEntities);
      return busEntities;
    }

    public IEnumerable<VModel.LOV> GetByParentValue(string parentValue) {
      IEnumerable<Model.LOV> dbEntities = _repository.GetQuery<Model.LOV>().Include("Parent").Where(o => o.Parent.LOV_Value == parentValue && o.LOV_Active == true);
      IEnumerable<VModel.LOV> busEntities = Map(dbEntities);
      return busEntities;
    }

    public override VModel.LOV Add(VModel.LOV addObject) {
      VModel.LOV insertedObjectBusiness = addObject;
      try {
        Model.LOV newModObject = Mapper.Map<VModel.LOV, Model.LOV>(addObject);
        newModObject.LOVType = _repository.GetQuery<Model.LOVType>().SingleOrDefault(o => o.ID == addObject.LOVType.ID);
        newModObject.Parent = _repository.GetQuery<Model.LOV>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.LOV insertedObject = _repository.Add<Model.LOV>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.LOV, VModel.LOV>(insertedObject);
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
    }

    protected override void Modify(Expression<Func<Model.LOV, bool>> predicate, VModel.LOV modObject, string[] includePredicate = null) {
      try {
        Model.LOV newModObject = Mapper.Map<VModel.LOV, Model.LOV>(modObject);

        if (newModObject.LOVType != null)
          newModObject.LOVType = _repository.GetQuery<Model.LOVType>().SingleOrDefault(o => o.ID == modObject.LOVType.ID);

        if (newModObject.Parent != null)
          newModObject.Parent = _repository.GetQuery<Model.LOV>().SingleOrDefault(o => o.ID == modObject.Parent.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.LOV>(predicate, newModObject, includePredicate);
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




    public override System.Linq.Expressions.Expression<Func<Model.LOV, bool>> UniqueEntityExp(Model.LOV modelEntity, VModel.LOV businessEntity) {
      return m => m.LOV_Value.Equals(modelEntity.LOV_Value, StringComparison.InvariantCultureIgnoreCase)
                  && m.LOVType.ID == modelEntity.LOVType.ID
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }

    public VModel.LOV GetByValue(string lovValue) {
      try {
        return this.GetSingleByExpression(s => s.LOV_Value == lovValue && s.Active_Ind == true);
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }

    public VModel.LOV GetByValueAndParent(string lovValue, string parentLovValue) {
      try {
          return this.GetSingleByExpression(s => s.LOV_Value == lovValue && s.Parent.LOV_Value == parentLovValue && s.Active_Ind == true, new string[] { "Parent" });
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }


  }
}
