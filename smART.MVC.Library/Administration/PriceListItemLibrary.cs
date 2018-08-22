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
  public class PriceListItemLibrary : PriceListChildLibrary<VModel.PriceListItem, Model.PriceListItem> {
    public PriceListItemLibrary()
      : base() {
    }
    public PriceListItemLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();

      Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
      Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();
    }

    public override VModel.PriceListItem Add(VModel.PriceListItem addObject) {
      VModel.PriceListItem insertedObjectBusiness = addObject;
      try {
        Model.PriceListItem newModObject = Mapper.Map<VModel.PriceListItem, Model.PriceListItem>(addObject);
        newModObject.PriceList = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.PriceList.ID);

        //Added By DB to resolve multiple items adding problem
        newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.PriceListItem insertedObject = _repository.Add<Model.PriceListItem>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.PriceListItem, VModel.PriceListItem>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.PriceListItem, bool>> predicate, VModel.PriceListItem modObject, string[] includePredicate = null) {
      try {
        Model.PriceListItem newModObject = Mapper.Map<VModel.PriceListItem, Model.PriceListItem>(modObject);

        if (modObject.PriceList != null)
          newModObject.PriceList = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == modObject.PriceList.ID);

        if (modObject.Item != null)
          newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.PriceListItem>(predicate, newModObject, includePredicate);
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

    public VModel.PriceListItem GetItemByPriceListAndItemId(int priceListId, int itemId) {
      return base.GetSingleByExpression(i => i.PriceList.ID == priceListId && i.Item.ID == itemId, new string[] { "Item", "PriceList" });
    }


    public IEnumerable<VModel.PriceListItem> GetDefaultPriceListItems()
    {
        return base.GetByExpression(i => i.PriceList.IsDefault ==true  && i.Active_Ind == true, new string[] { "Item", "PriceList" });
    }

    public override System.Linq.Expressions.Expression<Func<Model.PriceListItem, bool>> UniqueEntityExp(Model.PriceListItem modelEntity, VModel.PriceListItem businessEntity) {
      return m => m.Item.ID == modelEntity.Item.ID
                  && m.PriceList.ID == modelEntity.PriceList.ID
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }
  }
}
