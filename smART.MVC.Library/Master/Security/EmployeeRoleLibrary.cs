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
  public class EmployeeRoleLibrary : GenericLibrary<VModel.EmployeeRole, Model.EmployeeRole> {
    public EmployeeRoleLibrary()
      : base() {
    }

    public EmployeeRoleLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
      Mapper.CreateMap<VModel.Employee, Model.Employee>();
      Mapper.CreateMap<Model.Employee, VModel.Employee>();
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
      Mapper.CreateMap<VModel.Employee, Model.Employee>();
      Mapper.CreateMap<Model.Employee, VModel.Employee>();
    }

    public IEnumerable<VModel.EmployeeRole> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.EmployeeRole> modEnumeration = _repository.FindByPaging<Model.EmployeeRole>(out totalRows, o => o.Employee.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.EmployeeRole> busEnumeration = Mapper.Map<IEnumerable<Model.EmployeeRole>, IEnumerable<VModel.EmployeeRole>>(modEnumeration);

      return busEnumeration;

    }

    public IEnumerable<VModel.EmployeeRole> GetAllByParentID(
      int parentId,
      string[] includePredicate = null) {
      IEnumerable<Model.EmployeeRole> modEnumeration = _repository.Find<Model.EmployeeRole>(o => o.Employee.ID == parentId, includePredicate);
      IEnumerable<VModel.EmployeeRole> busEnumeration = Mapper.Map<IEnumerable<Model.EmployeeRole>, IEnumerable<VModel.EmployeeRole>>(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.EmployeeRole> GetByROle(VModel.Role role) {
      IEnumerable<Model.EmployeeRole> dbEntities = _repository.GetQuery<Model.EmployeeRole>().Include("Role").Include("Employee").Where(o => o.Role.ID == role.ID && o.Active_Ind == true);

      IEnumerable<VModel.EmployeeRole> busEntities = Mapper.Map<IEnumerable<Model.EmployeeRole>, IEnumerable<VModel.EmployeeRole>>(dbEntities);

      return busEntities;

    }

    public override VModel.EmployeeRole Add(VModel.EmployeeRole addObject) {
      VModel.EmployeeRole insertedObjectBusiness = addObject;
      try {
        Model.EmployeeRole newModObject = Mapper.Map<VModel.EmployeeRole, Model.EmployeeRole>(addObject);
        newModObject.Role = _repository.GetQuery<Model.Role>().SingleOrDefault(o => o.ID == addObject.Role.ID);
        newModObject.Employee = _repository.GetQuery<Model.Employee>().SingleOrDefault(o => o.ID == addObject.Employee.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.EmployeeRole insertedObject = _repository.Add<Model.EmployeeRole>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.EmployeeRole, VModel.EmployeeRole>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.EmployeeRole, bool>> predicate, VModel.EmployeeRole modObject, string[] includePredicate = null) {
      try {
        Model.EmployeeRole newModObject = Mapper.Map<VModel.EmployeeRole, Model.EmployeeRole>(modObject);

        newModObject.Role = _repository.GetQuery<Model.Role>().SingleOrDefault(o => o.ID == modObject.Role.ID);
        newModObject.Employee = _repository.GetQuery<Model.Employee>().SingleOrDefault(o => o.ID == modObject.Employee.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.EmployeeRole>(predicate, newModObject, includePredicate);
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


  }
}
