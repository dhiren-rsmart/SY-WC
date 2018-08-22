using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;

namespace smART.Library {

  public class BookingLibrary : GenericLibrary<VModel.Booking, Model.Booking> {
    public BookingLibrary()
      : base() {
    }
    public BookingLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();
    }

    public override VModel.Booking Add(VModel.Booking addObject) {
      VModel.Booking insertedObjectBusiness = addObject;
      try {
        int booking_Closed_ById, shipping_CompanyId, forwarder_Party_ID, sales_Order_NoId;
        booking_Closed_ById = shipping_CompanyId = forwarder_Party_ID = sales_Order_NoId = 0;

        Model.Booking newModObject = Mapper.Map<VModel.Booking, Model.Booking>(addObject);

        if (addObject.Booking_Closed_By != null)
          booking_Closed_ById = addObject.Booking_Closed_By.ID;

        if (addObject.Shipping_Company != null)
          shipping_CompanyId = addObject.Shipping_Company.ID;

        if (addObject.Forwarder_Party_ID != null)
          forwarder_Party_ID = addObject.Forwarder_Party_ID.ID;

        if (addObject.Sales_Order_No != null)
          sales_Order_NoId = addObject.Sales_Order_No.ID;

        newModObject.Booking_Closed_By = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == booking_Closed_ById);
        newModObject.Shipping_Company = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == shipping_CompanyId);
        newModObject.Forwarder_Party_ID = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == forwarder_Party_ID);
        newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == sales_Order_NoId);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.Booking insertedObject = _repository.Add<Model.Booking>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.Booking, VModel.Booking>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.Booking, bool>> predicate, VModel.Booking modObject, string[] includePredicate = null) {
      try {
        Model.Booking newModObject = Mapper.Map<VModel.Booking, Model.Booking>(modObject);

        if (modObject.Booking_Closed_By != null)
          newModObject.Booking_Closed_By = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == modObject.Booking_Closed_By.ID);
        if (modObject.Shipping_Company != null)
          newModObject.Shipping_Company = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Shipping_Company.ID);
        if (modObject.Forwarder_Party_ID != null)
          newModObject.Forwarder_Party_ID = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Forwarder_Party_ID.ID);
        if (modObject.Sales_Order_No != null)
          newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == modObject.Sales_Order_No.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.Booking>(predicate, newModObject, includePredicate);
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

    public override VModel.Booking GetByID(string id, string[] includePredicate = null) {
      string idFieldName = "ID";
      VModel.Booking booking = this.GetSingleByCriteria(GetModelIDExpression(idFieldName, Convert.ToInt32(id), typeof(int)), includePredicate);

      return booking;
    }

    public IEnumerable<VModel.Booking> GetOpenBookings() {
      IEnumerable<Model.Booking> modBooking = from Booking in _repository.GetQuery<Model.Booking>()
                                              where Booking.Booking_Status.ToLower().Equals("open")
                                              select Booking;
      IEnumerable<VModel.Booking> busBooking = Map(modBooking);
      return busBooking;
    }

    public IEnumerable<VModel.Booking> GetOpenBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {

      IEnumerable<Model.Booking> modEnumeration = _repository.FindByPaging<Model.Booking>(out totalRows, o => o.Booking_Status.ToLower().Equals("open"),
                                                                                          page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.Booking> GetOpenBrokerageBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {

      IEnumerable<Model.Booking> modEnumeration = _repository.FindByPaging<Model.Booking>(out totalRows, o => o.Booking_Status.ToLower().Equals("open") && o.Sales_Order_No.Scale_Broker.ToLower().Equals("brokerage"),
                                                                                          page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.Booking> PendingInvoiceBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      Expression<Func<Model.Booking, bool>> predicate = null;

      List<int> requiredScoreIds = _repository.Find<Model.Invoice>(x => x.Booking != null && x.Booking.ID > 0, new string[] { "Booking" }).Select(x => x.Booking.ID).ToList();

      if (requiredScoreIds != null && requiredScoreIds.Count() > 0) {
        var filteredProdIds = requiredScoreIds.ToArray();
        predicate = b => !requiredScoreIds.Contains(b.ID);     
      }

      IEnumerable<Model.Booking> modEnumeration = _repository.FindByPaging<Model.Booking>(out totalRows, predicate,
                                                                                          page, pageSize, sortColumn,
                                                                                          sortType, includePredicate,
                                                                                          filters
                                                                                          );
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.Booking> PendingInvoiceBrokerageTypeBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      Expression<Func<Model.Booking, bool>> predicate = null;

      List<int> requiredScoreIds = _repository.Find<Model.Invoice>(x => x.Booking != null && x.Booking.ID > 0, new string[] { "Booking" }).Select(x => x.Booking.ID).ToList();

      if (requiredScoreIds != null && requiredScoreIds.Count() > 0) {
        var filteredProdIds = requiredScoreIds.ToArray();
        predicate = b => !requiredScoreIds.Contains(b.ID) && b.Sales_Order_No.Scale_Broker.ToLower()=="brokerage";
      }

      IEnumerable<Model.Booking> modEnumeration = _repository.FindByPaging<Model.Booking>(out totalRows, predicate,
                                                                                          page, pageSize, sortColumn,
                                                                                          sortType, includePredicate,
                                                                                          filters
                                                                                          );
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.Booking> NonTradingOpenBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {

      IEnumerable<Model.Booking> modEnumeration = _repository.FindByPaging<Model.Booking>(out totalRows, o => o.Booking_Status.ToLower().Equals("open") && o.Sales_Order_No.Scale_Broker.ToLower() != "trading",
                                                                                          page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.Booking> GetUnPaidExpBookingsByParty(int partyId, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {

      IEnumerable<Model.Booking> modEnumeration = (from bookings in _repository.GetQuery<Model.Booking>()
                                                   join dispatcher in _repository.GetQuery<Model.DispatcherRequest>()
                                                   on bookings.ID equals dispatcher.Booking_Ref_No.ID
                                                   join expense in _repository.GetQuery<Model.ExpensesRequest>()
                                                   on dispatcher.ID equals expense.Dispatcher_Request_Ref.ID
                                                   where (dispatcher.TruckingCompany.ID == partyId && dispatcher.Active_Ind == true
                                                   && bookings.Active_Ind == true && expense.Active_Ind == true && expense.Amount_Paid != expense.Amount_Paid_Till_Date)
                                                   select bookings).Distinct();


      modEnumeration = _repository.FindByPaging<Model.Booking>(modEnumeration.AsQueryable(), out totalRows, null, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Booking> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public override System.Linq.Expressions.Expression<Func<Model.Booking, bool>> UniqueEntityExp(Model.Booking modelEntity, VModel.Booking businessEntity) {
      return m => m.Booking_Ref_No.Equals(modelEntity.Booking_Ref_No, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }
  }
}