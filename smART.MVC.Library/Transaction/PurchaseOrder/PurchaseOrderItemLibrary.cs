using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;
using System.Linq.Expressions;
using Telerik.Web.Mvc;



namespace smART.Library {
  public class PurchaseOrderItemLibrary : PurchaseOrderChildLibrary<VModel.PurchaseOrderItem, Model.PurchaseOrderItem> {
    public PurchaseOrderItemLibrary()
      : base() {
    }
    public PurchaseOrderItemLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();
      Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
      Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();
      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

    }

    public override VModel.PurchaseOrderItem Add(VModel.PurchaseOrderItem addObject) {
      VModel.PurchaseOrderItem insertedObjectBusiness = addObject;
      try {
        Model.PurchaseOrderItem newModObject = Mapper.Map<VModel.PurchaseOrderItem, Model.PurchaseOrderItem>(addObject);
        if (newModObject.PurchaseOrder != null)
          newModObject.PurchaseOrder = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == addObject.PurchaseOrder.ID);
        if (newModObject.Item != null)
          newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.PurchaseOrderItem insertedObject = _repository.Add<Model.PurchaseOrderItem>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.PurchaseOrderItem, VModel.PurchaseOrderItem>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.PurchaseOrderItem, bool>> predicate, VModel.PurchaseOrderItem modObject, string[] includePredicate = null) {
      try {
        Model.PurchaseOrderItem newModObject = Mapper.Map<VModel.PurchaseOrderItem, Model.PurchaseOrderItem>(modObject);
        if (newModObject.PurchaseOrder != null)
          newModObject.PurchaseOrder = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == modObject.PurchaseOrder.ID);
        if (newModObject.Item != null)
          newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.PurchaseOrderItem>(predicate, newModObject, includePredicate);
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

    public VModel.PurchaseOrderItem GetPOItemByItemCode(int purchaseOrderId, int itemId, string[] includePredicate = null) {
      IEnumerable<Model.PurchaseOrderItem> modEnumeration = _repository.Find<Model.PurchaseOrderItem>(o => o.PurchaseOrder.ID == purchaseOrderId && o.Item.ID == itemId, includePredicate);
      IEnumerable<VModel.PurchaseOrderItem> busEnumeration = Map(modEnumeration);
      return busEnumeration.FirstOrDefault();
    }

    public IEnumerable<VModel.PurchaseOrderItem> GetOpenPOItemsWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, int partyId = 0) {
      IEnumerable<Model.PurchaseOrderItem> modEnumeration;
      if (partyId > 0)
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrderItem>(out totalRows, o => o.PurchaseOrder.Order_Status != "Closed" && o.PurchaseOrder.Party.ID == partyId ,
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);
      else
        modEnumeration = _repository.FindByPaging<Model.PurchaseOrderItem>(out totalRows, o => o.PurchaseOrder.Order_Status != "Closed",
                                                                                        page, pageSize, sortColumn, sortType, includePredicate, filters);

      IEnumerable<VModel.PurchaseOrderItem> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

  }
}
