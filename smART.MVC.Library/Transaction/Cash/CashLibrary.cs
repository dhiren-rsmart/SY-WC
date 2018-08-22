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

  public class CashLibrary : GenericLibrary<VModel.Cash, Model.Cash> {
    public CashLibrary()
      : base() {
    }
    public CashLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.PaymentReceipt, Model.PaymentReceipt>();
      Mapper.CreateMap<Model.PaymentReceipt, VModel.PaymentReceipt>();
    }

    public override VModel.Cash Add(VModel.Cash addObject) {
      VModel.Cash insertedObjectBusiness = addObject;
      try {
        Model.Cash newModObject = Mapper.Map<VModel.Cash, Model.Cash>(addObject);

        if (addObject.Payment != null)
          newModObject.Payment = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == addObject.Payment.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.Cash insertedObject = _repository.Add<Model.Cash>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.Cash, VModel.Cash>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.Cash, bool>> predicate, VModel.Cash modObject, string[] includePredicate = null) {
      try {
        Model.Cash newModObject = Mapper.Map<VModel.Cash, Model.Cash>(modObject);

        if (newModObject.Payment != null)
          newModObject.Payment = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == modObject.Payment.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.Cash>(predicate, newModObject, includePredicate);
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

    public decimal GetBalance() {
      decimal balance = 0;
      Model.Cash cash = _repository.GetQuery<Model.Cash>().OrderByDescending(s => s.Created_Date).FirstOrDefault();
      return cash != null ? cash.Balance : balance;
    }

  }
}
