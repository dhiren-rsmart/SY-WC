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
  public class InvoiceLibrary : GenericLibrary<VModel.Invoice, Model.Invoice> {
    public InvoiceLibrary()
      : base() {
    }
    public InvoiceLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, VModel.Invoice>().ForMember(d => d.Booking, o => o.NullSubstitute(new VModel.Booking()));

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>().ForMember(d => d.Sales_Order_No, o => o.NullSubstitute(new VModel.SalesOrder()));
    }

    public override VModel.Invoice Add(VModel.Invoice addObject) {
      VModel.Invoice insertedObjectBusiness = addObject;
      try {
        Model.Invoice newModObject = Mapper.Map<VModel.Invoice, Model.Invoice>(addObject);

        if (newModObject.Booking!= null)
        newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == addObject.Booking.ID);
        if (newModObject.Sales_Order_No != null)
        newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == addObject.Sales_Order_No.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.Invoice insertedObject = _repository.Add<Model.Invoice>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.Invoice, VModel.Invoice>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.Invoice, bool>> predicate, VModel.Invoice modObject, string[] includePredicate = null) {
      try {
        Model.Invoice newModObject = Mapper.Map<VModel.Invoice, Model.Invoice>(modObject);
        if (newModObject.Booking!= null)
        newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);
        if (newModObject.Sales_Order_No != null)
        newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == modObject.Sales_Order_No.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.Invoice>(predicate, newModObject, includePredicate);
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

    //protected override void Modify(Expression<Func<Model.Invoice, bool>> predicate, VModel.Invoice modObject, string[] includePredicate = null)
    //{
    //    try
    //    {
    //        Model.Invoice newModObject = Mapper.Map<VModel.Invoice, Model.Invoice>(modObject);
    //        newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);
      
    //        if (Modifying(modObject, newModObject, _dbContext))
    //        {
    //            _repository.Modify<Model.Invoice>(predicate, newModObject, includePredicate);
    //            _repository.SaveChanges();
    //            Modified(modObject, newModObject, _dbContext);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        bool rethrow;
    //        rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
    //        if (rethrow)
    //            throw ex;
    //    }
    //}


    public IEnumerable<VModel.Invoice> GetUnPaidInvoicesWithPaging(
       out int totalRows,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null,
        int partyId = 0
      ) {

      IEnumerable<Model.Invoice> modEnumeration;
      Expression<Func<Model.Invoice, bool>> exp;
      if (partyId > 0)
        exp = o => (o.Booking.Sales_Order_No.Party.ID == partyId || o.Sales_Order_No.Party.ID==partyId) && o.Net_Amt > o.Amount_Paid_Till_Date;
      else
        exp = o => o.Net_Amt > o.Amount_Paid_Till_Date;
      modEnumeration = _repository.FindByPaging<Model.Invoice>(out totalRows, exp,
                                                               page, pageSize, sortColumn,
                                                               sortType, includePredicate, filters
                                                               );
      IEnumerable<VModel.Invoice> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public decimal GetTotalDueAmount(int partyId) {
      IEnumerable<Model.Invoice> modEnumeration;
      modEnumeration = _repository.Find<Model.Invoice>(o => o.Net_Amt > o.Amount_Paid_Till_Date && (o.Booking.Sales_Order_No.Party.ID == partyId || o.Sales_Order_No.Party.ID == partyId));
      IEnumerable<VModel.Invoice> busEnumeration = Mapper.Map<IEnumerable<Model.Invoice>, IEnumerable<VModel.Invoice>>(modEnumeration);
      decimal amt = busEnumeration.Sum(s => s.Net_Amt);
      decimal amtPaid = busEnumeration.Sum(s => s.Amount_Paid_Till_Date);
      return amt - amtPaid;
    }

    public override System.Linq.Expressions.Expression<Func<Model.Invoice, bool>> UniqueEntityExp(Model.Invoice modelEntity, VModel.Invoice businessEntity) {
      if (modelEntity.Invoice_Type.ToLower().Contains("exports"))
        return m => m.Booking.ID == modelEntity.Booking.ID
                    && m.Active_Ind == true
                    && m.ID != modelEntity.ID;
      else
        return null;
    }

    public VModel.Invoice GetInvoiceBySalesOrder(int SOId) {
      IEnumerable<Model.Invoice> modEnumeration = null;
      IEnumerable<Model.Booking> modBooking = _repository.Find<Model.Booking>(o => o.Sales_Order_No.ID == SOId);
      if (modBooking != null && modBooking.Count() > 0) {
        int bookId = modBooking.FirstOrDefault().ID;
        modEnumeration = _repository.Find<Model.Invoice>(o => o.Booking.ID == bookId);
      }
      IEnumerable<VModel.Invoice> busEnumeration = Map(modEnumeration);
      return busEnumeration.FirstOrDefault();
    }

  }
}
