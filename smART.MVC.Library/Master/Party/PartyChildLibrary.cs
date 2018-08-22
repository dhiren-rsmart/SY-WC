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
    public abstract class PartyChildLibrary<TEntityBusiness, TEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, IParentChildLibrary<TEntityBusiness>
        where TEntityBusiness : VModel.PartyChildEntity, new()
        where TEntityModel : Model.PartyChildEntity, new()
    {
        public PartyChildLibrary() : base() { }
        public PartyChildLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public IEnumerable<TEntityBusiness> GetAllByPagingByParentID(
            out int totalRows,
            int id,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.FindByPaging<TEntityModel>(out totalRows, o => o.Party.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;

        }

        public IEnumerable<TEntityBusiness> GetAllByParentID(
            int partyId, 
            string[] includePredicate = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.Find<TEntityModel>(o => o.Party.ID == partyId, includePredicate);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public override TEntityBusiness Add(TEntityBusiness addObject)
        {
            TEntityBusiness insertedObjectBusiness = addObject;
            try {
              TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(addObject);
              newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
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
