// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;
using System.Linq.Expressions;

namespace smART.Library {

  public class CycleDetailsLibrary : GenericLibrary<VModel.CycleDetails, Model.CycleDetails>, IParentChildLibrary<VModel.CycleDetails> {

    public CycleDetailsLibrary()
      : base() {
    }
    public CycleDetailsLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.SettlementDetails, Model.SettlementDetails>();
      Mapper.CreateMap<Model.SettlementDetails, VModel.SettlementDetails>();
    }

    public IEnumerable<VModel.CycleDetails> GetAllByPagingByParentID(
          out int totalRows,
          int id,
          int page,
          int pageSize,
          string sortColumn,
          string sortType,
          string[] includePredicate = null,
          IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.CycleDetails> modEnumeration = _repository.FindByPaging<Model.CycleDetails>(out totalRows, o => o.Cycle.ID == id && o.Purchase_ID.ID == null, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.CycleDetails> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.CycleDetails> GetAllByParentID(int parentId, string[] includePredicate = null) {
      IEnumerable<Model.CycleDetails> modEnumeration = _repository.Find<Model.CycleDetails>(o => o.Item.ID == parentId, includePredicate);
      IEnumerable<VModel.CycleDetails> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public VModel.CycleDetails GetOpItemByParentID(int parentId,int itemId, string[] includePredicate = null) {
      return GetSingleByCriteria(o => o.Item.ID == itemId && o.Cycle .ID == parentId && o.Purchase_ID == null, includePredicate);      
    }

    public IEnumerable<VModel.CycleDetails> GetAllByStartDate(int prevCycleId,DateTime start, string[] includePredicate = null) {
      IEnumerable<Model.CycleDetails> modEnumeration = (from c in _repository.GetAll<Model.CycleDetails>(includePredicate)
                  where (c.Cycle.ID == prevCycleId && c.Purchase_Qty>0 && c.Purchase_Amount>0)
                   group c by c.Item.ID into g
                   let PurchaseQty = g.Sum(c => c.Purchase_Qty)
                   let PurchaseCost = g.Sum(c => c.Purchase_Cost)
                   let PurchaseAmount = g.Sum(c => c.Purchase_Amount)
                   let AverageCost = Decimal.Round( (g.Sum(c => c.Purchase_Amount) / g.Sum(c => c.Purchase_Qty)),3)
                   select new Model.CycleDetails {
                     Purchase_Qty =  PurchaseQty,
                     Purchase_Cost = PurchaseCost,
                     Purchase_Amount = PurchaseAmount,
                     Average_Cost = AverageCost,
                     Item = g.First().Item
                   }).ToList();

      IEnumerable<VModel.CycleDetails> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public override VModel.CycleDetails Add(VModel.CycleDetails addObject) {
      VModel.CycleDetails insertedObjectBusiness = addObject;
      try {
        // To execute trigger
        addObject.Allow_Triggering = true;  

        Model.CycleDetails newModObject = Mapper.Map<VModel.CycleDetails, Model.CycleDetails>(addObject);

        if (newModObject.Cycle != null)
          newModObject.Cycle = _repository.GetQuery<Model.Cycle>().SingleOrDefault(o => o.ID == addObject.Cycle.ID);

        if (newModObject.Item != null)
          newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Item.ID);

        if (newModObject.Purchase_ID != null)
          newModObject.Purchase_ID = _repository.GetQuery<Model.SettlementDetails>().SingleOrDefault(o => o.ID == addObject.Purchase_ID.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.CycleDetails insertedObject = _repository.Add<Model.CycleDetails>(newModObject);

          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.CycleDetails, VModel.CycleDetails>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.CycleDetails, bool>> predicate, VModel.CycleDetails modObject, string[] includePredicate = null) {
      try {
        // To execute trigger
        modObject.Allow_Triggering = true;  

        Model.CycleDetails newModObject = Mapper.Map<VModel.CycleDetails, Model.CycleDetails>(modObject);

        if (newModObject.Cycle != null)
          newModObject.Cycle = _repository.GetQuery<Model.Cycle>().SingleOrDefault(o => o.ID == modObject.Cycle.ID);

        if (newModObject.Item != null)
          newModObject.Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == modObject.Item.ID);

        if (newModObject.Purchase_ID != null)
          newModObject.Purchase_ID = _repository.GetQuery<Model.SettlementDetails>().SingleOrDefault(o => o.ID == modObject.Purchase_ID.ID);


        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.CycleDetails>(predicate, newModObject, includePredicate);
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

    public override System.Linq.Expressions.Expression<Func<Model.CycleDetails, bool>> UniqueEntityExp(Model.CycleDetails modelEntity, VModel.CycleDetails businessEntity) {
      return m => m.Item.ID == modelEntity.Item.ID
                  && m.Cycle.ID == modelEntity.Cycle.ID
                  && m.Purchase_ID.ID == null
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;

    }
  }
}
