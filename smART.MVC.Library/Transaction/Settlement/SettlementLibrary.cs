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

  public class SettlementLibrary : GenericLibrary<VModel.Settlement, Model.Settlement> {
    public SettlementLibrary()
      : base() {
    }

    public SettlementLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>().ForMember(d => d.Purchase_Order, o => o.NullSubstitute(new VModel.PurchaseOrder()));

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();

      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Settlement, Model.Settlement>();
      Mapper.CreateMap<Model.Settlement, VModel.Settlement>().ForMember(d => d.Scale, o => o.NullSubstitute(new VModel.Scale()));

    }

    public override VModel.Settlement Add(VModel.Settlement addObject) {
      VModel.Settlement insertedObjectBusiness = addObject;
      try {
        Model.Settlement newModObject = Mapper.Map<VModel.Settlement, Model.Settlement>(addObject);
        decimal actualNetWt = addObject.Actual_Net_Weight;

        if (addObject.Scale != null)
          newModObject.Scale = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Scale.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {

          Model.Settlement insertedObject = _repository.Add<Model.Settlement>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.Settlement, VModel.Settlement>(insertedObject);
          insertedObjectBusiness.Actual_Net_Weight = actualNetWt;
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


    public IEnumerable<VModel.Settlement> GetUnPaidTicketsWithPaging(
       out int totalRows,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null,
       int partyId = 0,
       int locationId = 0
      ) {
      IEnumerable<Model.Settlement> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.Settlement, bool>> exp;
        if (locationId > 0)
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                     && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase)
                        )
                     && o.Scale.Party_Address.ID == locationId;
        else
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                                                                  && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase));

        modEnumeration = _repository.FindByPaging<Model.Settlement>(out totalRows, exp,
                                                                    page, pageSize, sortColumn, sortType, includePredicate, filters);
      }
      else
        modEnumeration = _repository.FindByPaging<Model.Settlement>(out totalRows, o => o.Amount > o.Amount_Paid_Till_Date &&
                                                                    (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase)),
                                                                    page, pageSize, sortColumn, sortType, includePredicate, filters);

      IEnumerable<VModel.Settlement> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.Settlement> GetUnPaidTickets(string[] includePredicate = null, int partyId = 0, int locationId = 0) {
      IEnumerable<Model.Settlement> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.Settlement, bool>> exp;
        if (locationId > 0)
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                     && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase)
                        )
                     && o.Scale.Party_Address.ID == locationId
                     && o.Active_Ind == true && o.Scale.Active_Ind == true;
        else
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                                                                  && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                                    && o.Active_Ind == true && o.Scale.Active_Ind == true;
        ;

        modEnumeration = _repository.Find<Model.Settlement>(exp, includePredicate);
      }
      else
        modEnumeration = _repository.Find<Model.Settlement>(o => o.Amount > o.Amount_Paid_Till_Date &&
                                                                    (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                                                                    o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                                    && o.Active_Ind == true && o.Scale.Active_Ind == true, includePredicate);

      IEnumerable<VModel.Settlement> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public VModel.Settlement GetSettlementByScaleId(int scaleId ,string[] includePredicate = null) {
      Model.Settlement modEnumeration = _repository.Find<Model.Settlement>(o => o.Scale.ID == scaleId).FirstOrDefault();
      VModel.Settlement busEnumeration = Mapper.Map<Model.Settlement, VModel.Settlement>(modEnumeration);
      return busEnumeration;
    }


    public decimal GetTotalDueAmount(int partyId, int locationId = 0) {
      IEnumerable<Model.Settlement> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.Settlement, bool>> exp;
        if (locationId > 0)
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                     && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase) ||
                         o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase)
                        )
                     && o.Scale.Party_Address.ID == locationId
                     && o.Active_Ind == true && o.Scale.Active_Ind == true;
        else
          exp = o => o.Scale.Party_ID.ID == partyId && o.Amount > o.Amount_Paid_Till_Date
                                                                  && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase)
                                                                  || o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase)
                                                                  || o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                                && o.Active_Ind == true && o.Scale.Active_Ind == true;

        modEnumeration = _repository.Find<Model.Settlement>(exp);
      }
      else
        modEnumeration = _repository.Find<Model.Settlement>(o => o.Amount > o.Amount_Paid_Till_Date
                                                                 && (o.Scale.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase)
                                                                 || o.Scale.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase)
                                                                 || o.Scale.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                              && o.Active_Ind == true && o.Scale.Active_Ind == true
                                                            );

      //IEnumerable<VModel.Settlement> busEnumeration = Map(modEnumeration);
      decimal amt = modEnumeration.Sum(s => s.Amount);
      decimal amtPaid = modEnumeration.Sum(s => s.Amount_Paid_Till_Date);
      return amt - amtPaid;
    }


    // Unique record check
    public override System.Linq.Expressions.Expression<Func<Model.Settlement, bool>> UniqueEntityExp(Model.Settlement modelEntity, VModel.Settlement businessEntity) {
        return m => m.Scale.ID.Equals(modelEntity.Scale.ID)                   
                   && m.Active_Ind == true
                   && m.ID != modelEntity.ID;
    }
  }
}
