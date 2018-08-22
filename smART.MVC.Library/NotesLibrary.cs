using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

namespace smART.Library {
  public abstract class NotesLibrary<TEntityBusiness, TEntityModel, TParentEntityBusiness, TParentEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, IParentChildLibrary<TEntityBusiness>
    where TEntityBusiness : VModel.NotesEntity<TParentEntityBusiness>, new()
    where TEntityModel : Model.NotesEntity<TParentEntityModel>, new()
    where TParentEntityBusiness : VModel.BaseEntity, new()
    where TParentEntityModel : Model.BaseEntity, new() {
    public NotesLibrary() : base() {
    }
    public NotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) {
    }

    protected override void Initialize(DbContext dbContext) {
      base.Initialize(dbContext);

      Mapper.CreateMap<TEntityModel, TEntityBusiness>();
      Mapper.CreateMap<TEntityBusiness, TEntityModel>();

      Mapper.CreateMap<TParentEntityModel, TParentEntityBusiness>();
      Mapper.CreateMap<TParentEntityBusiness, TParentEntityModel>();
    }

    public IEnumerable<TEntityBusiness> GetAllByPagingByParentID(
        out int totalRows,
        int id,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) {
      IEnumerable<TEntityModel> modEnumeration = _repository.FindByPaging<TEntityModel>(out totalRows, o => o.Parent.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }

    public IEnumerable<TEntityBusiness> GetAllByParentID(
        int parentId,
        string[] includePredicate = null) {
      IEnumerable<TEntityModel> modEnumeration = _repository.Find<TEntityModel>(o => o.Parent.ID == parentId, includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public override TEntityBusiness Add(TEntityBusiness addObject) {
      TEntityBusiness insertedObjectBusiness = addObject;
      try {
        TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(addObject);
        newModObject.Parent = _repository.GetQuery<TParentEntityModel>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          TEntityModel insertedObject = _repository.Add<TEntityModel>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<TEntityModel, TEntityBusiness>(insertedObject);
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


  }
}
