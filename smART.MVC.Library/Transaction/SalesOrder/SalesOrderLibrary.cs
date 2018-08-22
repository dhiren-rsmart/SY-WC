using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;
using System.Linq.Expressions;

namespace smART.Library {
  public class SalesOrderLibrary : GenericLibrary<VModel.SalesOrder, Model.SalesOrder> {
    public SalesOrderLibrary()
      : base() {
    }
    public SalesOrderLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {

      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();
    }

    public IEnumerable<VModel.SalesOrder> GetOpenSO(string[] includePredicate) {
      IEnumerable<Model.SalesOrder> modSOs = _repository.Find<Model.SalesOrder>(
                                                                              o => o.Order_Status != "Closed",
                                                                                  includePredicate
                                                                             );
      IEnumerable<VModel.SalesOrder> busSOs = Map(modSOs);
      return busSOs;
    }

    public override VModel.SalesOrder Add(VModel.SalesOrder addObject) {
      VModel.SalesOrder insertedObjectBusiness = addObject;
      try {
        Model.SalesOrder newModObject = Mapper.Map<VModel.SalesOrder, Model.SalesOrder>(addObject);
        if (newModObject.Party != null)
          newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
        if (newModObject.Contact != null)
          newModObject.Contact = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == addObject.Contact.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {

          Model.SalesOrder insertedObject = _repository.Add<Model.SalesOrder>(newModObject);
          _repository.SaveChanges();


          insertedObjectBusiness = Mapper.Map<Model.SalesOrder, VModel.SalesOrder>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.SalesOrder, bool>> predicate, VModel.SalesOrder modObject, string[] includePredicate = null) {
      try {
        Model.SalesOrder newModObject = Mapper.Map<VModel.SalesOrder, Model.SalesOrder>(modObject);
        if (newModObject.Party != null)
          newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);
        if (newModObject.Contact != null)
          newModObject.Contact = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == modObject.Contact.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.SalesOrder>(predicate, newModObject, includePredicate);
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

    public IEnumerable<VModel.SalesOrder> GetOpenSOWithPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, int partyId = 0) {
      IEnumerable<Model.SalesOrder> modEnumeration;
      if (partyId > 0)
        modEnumeration = _repository.FindByPaging<Model.SalesOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Party.ID == partyId,
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.SalesOrder>(out totalRows, o => o.Order_Status != "Closed",
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.SalesOrder> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }

    public IEnumerable<VModel.SalesOrder> GetOpenBrokerageSOWithPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, int partyId = 0) {
      IEnumerable<Model.SalesOrder> modEnumeration;
      if (partyId > 0)
        modEnumeration = _repository.FindByPaging<Model.SalesOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Party.ID == partyId && o.Scale_Broker.ToLower() == "brokerage",
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.SalesOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Scale_Broker.ToLower() == "brokerage",
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.SalesOrder> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }
  }
}
