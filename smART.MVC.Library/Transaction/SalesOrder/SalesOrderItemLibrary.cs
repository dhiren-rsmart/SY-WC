using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;
using System.Linq.Expressions;

namespace smART.Library {

  public class SalesOrderItemLibrary : SalesOrderChildLibrary<VModel.SalesOrderItem, Model.SalesOrderItem> {

    public SalesOrderItemLibrary()
      : base() {
    }
    public SalesOrderItemLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();
      Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
      Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();
      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();
    }

    public override VModel.SalesOrderItem Add(VModel.SalesOrderItem addObject) {
      VModel.SalesOrderItem insertedObjectBusiness = addObject;
      try {
        Model.SalesOrderItem newModObject = Mapper.Map<VModel.SalesOrderItem, Model.SalesOrderItem>(addObject);
        newModObject.SalesOrder = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == addObject.SalesOrder.ID);
        //Added By DB to resolve multiple items adding problem
        newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item.ID);
        //newModObject.Item_Override = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item_Override.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.SalesOrderItem insertedObject = _repository.Add<Model.SalesOrderItem>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.SalesOrderItem, VModel.SalesOrderItem>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.SalesOrderItem, bool>> predicate, VModel.SalesOrderItem modObject, string[] includePredicate = null) {
      try {
        Model.SalesOrderItem newModObject = Mapper.Map<VModel.SalesOrderItem, Model.SalesOrderItem>(modObject);

        newModObject.SalesOrder = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == modObject.SalesOrder.ID);
        newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item.ID);
        //newModObject.Item_Override = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item_Override.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.SalesOrderItem>(predicate, newModObject, includePredicate);
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

    protected bool IsSalesOrderItem(int itemId) {
      Model.SalesOrderItem newModObject = _repository.GetQuery<Model.SalesOrderItem>().SingleOrDefault(o => o.Item.ID == itemId);
      return newModObject != null && newModObject.Item.ID > 0;
    }
  }
}
