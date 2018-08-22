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

namespace smART.Library {
  public class SettlementDetailsLibrary : GenericLibrary<VModel.SettlementDetails, Model.SettlementDetails>, IParentChildLibrary<VModel.SettlementDetails> {
    public SettlementDetailsLibrary()
      : base() {
    }

    public SettlementDetailsLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {

    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Settlement, Model.Settlement>();
      Mapper.CreateMap<Model.Settlement, VModel.Settlement>();

      Mapper.CreateMap<VModel.ScaleDetails, Model.ScaleDetails>();
      Mapper.CreateMap<Model.ScaleDetails, VModel.ScaleDetails>();

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();

      Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();
    }

    public IEnumerable<VModel.SettlementDetails> GetAllByPagingByParentID(out int totalRows, int id, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      throw new NotImplementedException();
    }



    //public override VModel.SettlementDetails Add(VModel.SettlementDetails addObject) {
    //  VModel.SettlementDetails insertedObjectBusiness = addObject;
    //  try {
    //    Model.SettlementDetails newModObject = Mapper.Map<VModel.SettlementDetails, Model.SettlementDetails>(addObject);
    //    decimal actualNetWt = addObject.Actual_Net_Weight;

    //    if (newModObject.Scale_Details_ID != null)
    //      newModObject.Scale_Details_ID = _repository.GetQuery<Model.ScaleDetails>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.ID);

    //    if (newModObject.Settlement_ID != null)
    //      newModObject.Settlement_ID = _repository.GetQuery<Model.Settlement>().SingleOrDefault(o => o.ID == addObject.Settlement_ID.ID);

    //    if (newModObject.Scale_Details_ID != null && newModObject.Scale_Details_ID.Apply_To_Item != null)
    //      newModObject.Scale_Details_ID.Apply_To_Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Apply_To_Item.ID);

    //    if (newModObject.Scale_Details_ID != null && newModObject.Scale_Details_ID.Scale != null)
    //      newModObject.Scale_Details_ID.Scale = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Scale.ID);

    //    if (addObject.Scale_Details_ID.Scale.Purchase_Order.Price_List != null)
    //      newModObject.Price_List_ID = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Scale.Purchase_Order.Price_List.ID);
    //      //newModObject.Price_List_ID = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.Settlement_ID.Scale.Purchase_Order.Price_List.ID);

    //    if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
    //      Model.SettlementDetails insertedObject = _repository.Add<Model.SettlementDetails>(newModObject);
    //      _repository.SaveChanges();

    //      insertedObjectBusiness = Mapper.Map<Model.SettlementDetails, VModel.SettlementDetails>(insertedObject);

    //      insertedObjectBusiness.Actual_Net_Weight = actualNetWt;
    //      insertedObjectBusiness.Scale_Details_ID.Apply_To_Item = addObject.Scale_Details_ID.Apply_To_Item;
    //      insertedObjectBusiness.Scale_Details_ID.Scale = addObject.Scale_Details_ID.Scale;

    //      Added(insertedObjectBusiness, newModObject, _dbContext);
    //    }
    //  }
    //  catch (Exception ex) {
    //    bool rethrow;
    //    rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
    //    if (rethrow)
    //      throw ex;
    //  }
    //  return insertedObjectBusiness;
    //}

    public override VModel.SettlementDetails Add(VModel.SettlementDetails addObject) {
      VModel.SettlementDetails insertedObjectBusiness = addObject;
      try {
        Model.SettlementDetails newModObject = Mapper.Map<VModel.SettlementDetails, Model.SettlementDetails>(addObject);
        decimal actualNetWt = addObject.Actual_Net_Weight;

        if (addObject.Scale_Details_ID != null)
          newModObject.Scale_Details_ID = _repository.GetQuery<Model.ScaleDetails>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.ID);

        if (addObject.Settlement_ID != null)
          newModObject.Settlement_ID = _repository.GetQuery<Model.Settlement>().SingleOrDefault(o => o.ID == addObject.Settlement_ID.ID);

        if (addObject.Scale_Details_ID != null && addObject.Scale_Details_ID.Apply_To_Item != null)
          newModObject.Scale_Details_ID.Apply_To_Item = _repository.GetQuery<Model.Item>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Apply_To_Item.ID);

        if (addObject.Scale_Details_ID != null && addObject.Scale_Details_ID.Scale != null)
          newModObject.Scale_Details_ID.Scale = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Scale.ID);

        if (addObject.Scale_Details_ID != null && addObject.Scale_Details_ID.Scale != null && addObject.Scale_Details_ID.Scale.Purchase_Order != null && addObject.Scale_Details_ID.Scale.Purchase_Order.Price_List != null)
          newModObject.Price_List_ID = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.Scale_Details_ID.Scale.Purchase_Order.Price_List.ID);
        //newModObject.Price_List_ID = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.Settlement_ID.Scale.Purchase_Order.Price_List.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.SettlementDetails insertedObject = _repository.Add<Model.SettlementDetails>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.SettlementDetails, VModel.SettlementDetails>(insertedObject);

          insertedObjectBusiness.Actual_Net_Weight = actualNetWt;
          insertedObjectBusiness.Scale_Details_ID.Apply_To_Item = addObject.Scale_Details_ID.Apply_To_Item;
          insertedObjectBusiness.Scale_Details_ID.Scale = addObject.Scale_Details_ID.Scale;

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
    public IEnumerable<VModel.SettlementDetails> GetAllByParentID(int parentId, string[] includePredicate = null) {
      IEnumerable<Model.SettlementDetails> modEnumeration = _repository.Find<Model.SettlementDetails>(o => o.Settlement_ID.ID == parentId, includePredicate);
      IEnumerable<VModel.SettlementDetails> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);
      return busEnumeration;
    }

    // Unique record check
    public override System.Linq.Expressions.Expression<Func<Model.SettlementDetails, bool>> UniqueEntityExp(Model.SettlementDetails modelEntity, VModel.SettlementDetails businessEntity) {
      return m => m.Scale_Details_ID.ID.Equals(modelEntity.Scale_Details_ID.ID)
                 && m.Active_Ind == true
                 && m.ID != modelEntity.ID;
    }
  }

}
