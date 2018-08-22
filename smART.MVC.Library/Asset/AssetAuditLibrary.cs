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

  public class AssetAuditLibrary : GenericLibrary<VModel.AssetAudit, Model.AssetAudit>, IParentChildLibrary<VModel.AssetAudit> {

    public AssetAuditLibrary()
      : base() {
    }
    public AssetAuditLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Asset, Model.Asset>();
      Mapper.CreateMap<Model.Asset, VModel.Asset>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();

      Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

    }

    public override VModel.AssetAudit Add(VModel.AssetAudit addObject) {
      VModel.AssetAudit insertedObjectBusiness = addObject;
      try {
        Model.AssetAudit newModObject = Mapper.Map<VModel.AssetAudit, Model.AssetAudit>(addObject);

        newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == addObject.Asset.ID);
        newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
        newModObject.Location = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == addObject.Location.ID);
        newModObject.Dispatcher_Request = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == addObject.Dispatcher_Request.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.AssetAudit insertedObject = _repository.Add<Model.AssetAudit>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.AssetAudit, VModel.AssetAudit>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.AssetAudit, bool>> predicate, VModel.AssetAudit modObject, string[] includePredicate = null) {
      try {
        Model.AssetAudit newModObject = Mapper.Map<VModel.AssetAudit, Model.AssetAudit>(modObject);
        newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == modObject.Asset.ID);
        newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);
        newModObject.Location = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == modObject.Location.ID);
        newModObject.Dispatcher_Request = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == modObject.Dispatcher_Request.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.AssetAudit>(predicate, newModObject, includePredicate);
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

    public IEnumerable<VModel.AssetAudit> GetAllByPagingByParentID(
     out int totalRows,
     int id,
     int page,
     int pageSize,
     string sortColumn,
     string sortType,
     string[] includePredicate = null,
     IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.AssetAudit> modEnumeration = _repository.FindByPaging<Model.AssetAudit>(out totalRows, o => o.Asset.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.AssetAudit> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);

      return busEnumeration;
    }
   
    public IEnumerable<VModel.AssetAudit> GetAllByParentID(
        int parentId,
        string[] includePredicate = null) {
      IEnumerable<Model.AssetAudit> modEnumeration = _repository.Find<Model.AssetAudit>(o => o.Asset.ID == parentId, includePredicate);
      IEnumerable<VModel.AssetAudit> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);
      return busEnumeration;
    }

    public IEnumerable<VModel.AssetAuditLookup> GetDispatcherReqTypeAssetsByPaging(
      string requestType,
       out int totalRows,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.AssetAudit> modEnumeration = null;
      totalRows = 0;
      if (!string.IsNullOrEmpty(requestType) && requestType.ToLower() == "drop off only")
        modEnumeration = _repository.FindByPaging<Model.AssetAudit>(out totalRows, o => o.Location.Party.Party_Type == "Organization" && o.Asset_Current_Location_Flg == true, page, pageSize, sortColumn, sortType, includePredicate, filters);
      else if (!string.IsNullOrEmpty(requestType) && requestType.ToLower() == "pickup only")
        modEnumeration = _repository.FindByPaging<Model.AssetAudit>(out totalRows, o => o.Location.Party.Party_Type != "Organization" && o.Asset_Current_Location_Flg == true, page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.AssetAudit>(out totalRows, o => o.Asset_Current_Location_Flg == true, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.AssetAudit> busEnumeration = Map(modEnumeration);

      GotMultiple(busEnumeration, modEnumeration, _dbContext);
      List<ViewModel.AssetAuditLookup> assetAuditLookupList = new List<VModel.AssetAuditLookup>();
      foreach (var item in busEnumeration) {
        ViewModel.AssetAuditLookup assetAuditLookup = new ViewModel.AssetAuditLookup();
        assetAuditLookup.ID = item.ID;
        assetAuditLookup.Party = item.Party;
        assetAuditLookup.Location = item.Location;
        assetAuditLookup.Asset = item.Asset;
        assetAuditLookupList.Add(assetAuditLookup);
      }
      return assetAuditLookupList;
    }

    public override System.Linq.Expressions.Expression<Func<Model.AssetAudit, bool>> UniqueEntityExp(Model.AssetAudit modelEntity, VModel.AssetAudit businessEntity) {
      return null;
    }


  }
}

