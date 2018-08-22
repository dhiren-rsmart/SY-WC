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
using smART.Common;

namespace smART.Library {
  public class PaymentReceiptLibrary : GenericLibrary<VModel.PaymentReceipt, Model.PaymentReceipt> {
    public PaymentReceiptLibrary()
      : base() {
    }
    public PaymentReceiptLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Bank, Model.Bank>();
      Mapper.CreateMap<Model.Bank, VModel.Bank>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();

    }

    public override VModel.PaymentReceipt Add(VModel.PaymentReceipt addObject) {
      VModel.PaymentReceipt insertedObjectBusiness = addObject;
      try {
        Model.PaymentReceipt newModObject = Mapper.Map<VModel.PaymentReceipt, Model.PaymentReceipt>(addObject);

        newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
        newModObject.Account_Name = _repository.GetQuery<Model.Bank>().SingleOrDefault(o => o.ID == addObject.Account_Name.ID);

        if (addObject.Booking != null)
          newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == addObject.Booking.ID);

        if (addObject.Party_Address != null)
          newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == addObject.Party_Address.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {

          Model.PaymentReceipt insertedObject = _repository.Add<Model.PaymentReceipt>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.PaymentReceipt, VModel.PaymentReceipt>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.PaymentReceipt, bool>> predicate, VModel.PaymentReceipt modObject, string[] includePredicate = null) {
      try {
        Model.PaymentReceipt newModObject = Mapper.Map<VModel.PaymentReceipt, Model.PaymentReceipt>(modObject);

        newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);
        newModObject.Account_Name = _repository.GetQuery<Model.Bank>().SingleOrDefault(o => o.ID == modObject.Account_Name.ID);

        if (modObject.Booking != null)
          newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);

        if (modObject.Party_Address != null)
          newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == modObject.Party_Address.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.PaymentReceipt>(predicate, newModObject, includePredicate);
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

    public IEnumerable<VModel.PaymentReceipt> GetReceiptsByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      string trasnType = EnumTransactionType.Receipt.ToString();
      IEnumerable<Model.PaymentReceipt> modEnumeration = _repository.FindByPaging<Model.PaymentReceipt>(out totalRows, o => o.Transaction_Type == trasnType,
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.PaymentReceipt> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.PaymentReceipt> GetPaymentsByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      string trasnType = EnumTransactionType.Payment.ToString();
      IEnumerable<Model.PaymentReceipt> modEnumeration = _repository.FindByPaging<Model.PaymentReceipt>(out totalRows, o => o.Transaction_Type == trasnType,
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.PaymentReceipt> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }

    public int GetNextCheckNo() {
      string trasnType = EnumTransactionType.Payment.ToString();
      Model.PaymentReceipt modEnumeration = _repository.Last<Model.PaymentReceipt>(o => o.Transaction_Type == trasnType);
      VModel.PaymentReceipt busEnumeration = Mapper.Map<Model.PaymentReceipt, VModel.PaymentReceipt>(modEnumeration);
      int id = 0;
      if (busEnumeration == null)
        id = 1;
      else if (int.TryParse(busEnumeration.Check_Wire_Transfer, out id))
        id += 1;
      return id;
    }
  }
}
