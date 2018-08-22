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
  public class PurchaseOrderLibrary : GenericLibrary<VModel.PurchaseOrder, Model.PurchaseOrder> {
    public PurchaseOrderLibrary()
      : base() {
    }
    public PurchaseOrderLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();
    }

    public IEnumerable<VModel.PurchaseOrder> GetOpenPOByPartyId(int partyId, string[] includePredicate) {
      IEnumerable<Model.PurchaseOrder> modPOs = _repository.Find<Model.PurchaseOrder>(
                                                                              o => o.Order_Status != "Closed" && o.Party.ID == partyId
                                                                               , includePredicate
                                                                             );
      IEnumerable<VModel.PurchaseOrder> busPOs = Map(modPOs);
      return busPOs;
    }

    public IEnumerable<VModel.PurchaseOrder> GetOpenPO(string[] includePredicate) {
      IEnumerable<Model.PurchaseOrder> modPOs = _repository.Find<Model.PurchaseOrder>(
                                                                              o => o.Order_Status != "Closed",
                                                                                  includePredicate
                                                                             );
      IEnumerable<VModel.PurchaseOrder> busPOs = Map(modPOs);
      return busPOs;
    }

    public IEnumerable<VModel.PurchaseOrder> GetOpenPOWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, int partyId = 0) {
      IEnumerable<Model.PurchaseOrder> modEnumeration;
      if (partyId > 0)
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Party.ID == partyId,
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrder>(out totalRows, o => o.Order_Status != "Closed",
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);

      IEnumerable<VModel.PurchaseOrder> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.PurchaseOrder> GetOpenBrokeragePOWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, int partyId = 0) {
      IEnumerable<Model.PurchaseOrder> modEnumeration;
      if (partyId > 0)
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Party.ID == partyId && o.Scale_Broker.ToLower() == "brokerage",
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrder>(out totalRows, o => o.Order_Status != "Closed" && o.Scale_Broker.ToLower() == "brokerage",
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);

      IEnumerable<VModel.PurchaseOrder> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public override VModel.PurchaseOrder Add(VModel.PurchaseOrder addObject) {
      VModel.PurchaseOrder insertedObjectBusiness = addObject;
      try {
        Model.PurchaseOrder newModObject = Mapper.Map<VModel.PurchaseOrder, Model.PurchaseOrder>(addObject);

        if (newModObject.Party != null)
          newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
        if (newModObject.Contact != null)
          newModObject.Contact = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == addObject.Contact.ID);
        if (newModObject.Price_List != null)
          newModObject.Price_List = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.Price_List.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {

          Model.PurchaseOrder insertedObject = _repository.Add<Model.PurchaseOrder>(newModObject);
          _repository.SaveChanges();


          insertedObjectBusiness = Mapper.Map<Model.PurchaseOrder, VModel.PurchaseOrder>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.PurchaseOrder, bool>> predicate, VModel.PurchaseOrder modObject, string[] includePredicate = null) {
      try {
        Model.PurchaseOrder newModObject = Mapper.Map<VModel.PurchaseOrder, Model.PurchaseOrder>(modObject);

        if (newModObject.Party != null)
          newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);
        if (newModObject.Contact != null)
          newModObject.Contact = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == modObject.Contact.ID);
        if (newModObject.Price_List != null)
          newModObject.Price_List = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == modObject.Price_List.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.PurchaseOrder>(predicate, newModObject, includePredicate);
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
