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

  public class ScaleDetailsLibrary : GenericLibrary<VModel.ScaleDetails, Model.ScaleDetails>, IParentChildLibrary<VModel.ScaleDetails> {
    public ScaleDetailsLibrary()
      : base() {
    }
    public ScaleDetailsLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();

    }


    public override VModel.ScaleDetails Add(VModel.ScaleDetails addObject) {
      VModel.ScaleDetails insertedObjectBusiness = addObject;
      try {
        Model.ScaleDetails newModObject = Mapper.Map<VModel.ScaleDetails, Model.ScaleDetails>(addObject);
        if (newModObject.Scale != null)
          newModObject.Scale = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Scale.ID);
        if (newModObject.Item_Received != null)
          newModObject.Item_Received = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item_Received.ID);
        if (newModObject.Apply_To_Item != null)
          newModObject.Apply_To_Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Apply_To_Item.ID);
        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.ScaleDetails insertedObject = _repository.Add<Model.ScaleDetails>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.ScaleDetails, VModel.ScaleDetails>(insertedObject);

          newModObject.Old_Net_Weight = insertedObject.NetWeight;

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



    protected override void Modify(Expression<Func<Model.ScaleDetails, bool>> predicate, VModel.ScaleDetails modObject, string[] includePredicate = null) {
      try {
        Model.ScaleDetails newModObject = Mapper.Map<VModel.ScaleDetails, Model.ScaleDetails>(modObject);
        if (newModObject.Scale != null)
          newModObject.Scale = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == modObject.Scale.ID);
        if (newModObject.Item_Received != null)
          newModObject.Item_Received = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item_Received.ID);
        if (newModObject.Apply_To_Item != null)
          newModObject.Apply_To_Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Apply_To_Item.ID);

        decimal oldNetWeight = 0;
        Model.ScaleDetails oldModObject = _repository.GetQuery<Model.ScaleDetails>().SingleOrDefault(o => o.ID == modObject.ID);
        if (oldModObject != null) {
          oldNetWeight = oldModObject.NetWeight;
        }

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.ScaleDetails>(predicate, newModObject, includePredicate);
          _repository.SaveChanges();
          newModObject.Old_Net_Weight = oldNetWeight;
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



    public IEnumerable<VModel.ScaleDetails> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.ScaleDetails> modEnumeration = _repository.FindByPaging<Model.ScaleDetails>(out totalRows, o => o.Scale.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.ScaleDetails> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.ScaleDetails> GetAllByParentID(int parentId, string[] includePredicate = null) {
      IEnumerable<Model.ScaleDetails> modEnumeration = _repository.Find<Model.ScaleDetails>(o => o.Scale.ID == parentId, includePredicate);
      IEnumerable<VModel.ScaleDetails> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }


    public IEnumerable<VModel.ScaleDetails> GetAllByPartyIdWithPagging(
       int id,
      out int totalRows,     
      int page,
      int pageSize,
      string sortColumn,
      string sortType,
      string[] includePredicate = null,
      IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.ScaleDetails> modEnumeration = _repository.FindByPaging<Model.ScaleDetails>(out totalRows, o => o.Scale.Party_ID.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.ScaleDetails> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

  }
}

