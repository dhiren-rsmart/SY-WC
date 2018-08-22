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

  public class DispatcherRequestLibrary : GenericLibrary<VModel.DispatcherRequest, Model.DispatcherRequest> {

    public DispatcherRequestLibrary()
      : base() {
    }
    public DispatcherRequestLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();
      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();
      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();
      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();
      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();
      Mapper.CreateMap<VModel.Asset, Model.Asset>();
      Mapper.CreateMap<Model.Asset, VModel.Asset>();
    }

    public override VModel.DispatcherRequest Add(VModel.DispatcherRequest addObject) {
      VModel.DispatcherRequest insertedObjectBusiness = addObject;
      int truckingCompany_ID, driver_ID, booking_Ref_No_ID, purchase_Order_No_ID, sales_Order_No_ID, location_ID, party_ID, shipper_ID, asset_ID, container_ID;
      truckingCompany_ID = driver_ID = booking_Ref_No_ID = purchase_Order_No_ID = sales_Order_No_ID = location_ID = party_ID = shipper_ID = asset_ID = container_ID = 0;

      try {

        Model.DispatcherRequest newModObject = Mapper.Map<VModel.DispatcherRequest, Model.DispatcherRequest>(addObject);

        if (addObject.TruckingCompany != null)
          truckingCompany_ID = addObject.TruckingCompany.ID;
        newModObject.TruckingCompany = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == truckingCompany_ID);

        if (addObject.Driver != null)
          driver_ID = addObject.Driver.ID;
        newModObject.Driver = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == driver_ID);

        if (addObject.Booking_Ref_No != null)
          booking_Ref_No_ID = addObject.Booking_Ref_No.ID;
        newModObject.Booking_Ref_No = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == booking_Ref_No_ID);

        if (addObject.Purchase_Order_No != null)
          purchase_Order_No_ID = addObject.Purchase_Order_No.ID;
        newModObject.Purchase_Order_No = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == purchase_Order_No_ID);

        if (addObject.Sales_Order_No != null)
          sales_Order_No_ID = addObject.Sales_Order_No.ID;
        newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == sales_Order_No_ID);

        if (addObject.Location != null)
          location_ID = addObject.Location.ID;
        newModObject.Location = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == location_ID);

        if (addObject.Party != null)
          party_ID = addObject.Party.ID;
        newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == party_ID);

        if (addObject.Shipper != null)
          shipper_ID = addObject.Shipper.ID;
        newModObject.Shipper = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == shipper_ID);

        if (addObject.Asset != null)
          asset_ID = addObject.Asset.ID;
        newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == asset_ID);

        if (addObject.Container != null)
          container_ID = addObject.Container.ID;
        newModObject.Container = _repository.GetQuery<Model.Container>().SingleOrDefault(o => o.ID == container_ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.DispatcherRequest insertedObject = _repository.Add<Model.DispatcherRequest>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.DispatcherRequest, VModel.DispatcherRequest>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.DispatcherRequest, bool>> predicate, VModel.DispatcherRequest modObject, string[] includePredicate = null) {
      try {

        Model.DispatcherRequest newModObject = Mapper.Map<VModel.DispatcherRequest, Model.DispatcherRequest>(modObject);

        if (modObject.TruckingCompany != null)
          newModObject.TruckingCompany = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.TruckingCompany.ID);

        if (modObject.Driver != null)
          newModObject.Driver = _repository.GetQuery<Model.Contact>().SingleOrDefault(o => o.ID == modObject.Driver.ID);

        if (modObject.Booking_Ref_No != null)
          newModObject.Booking_Ref_No = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking_Ref_No.ID);

        if (modObject.Purchase_Order_No != null)
          newModObject.Purchase_Order_No = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == modObject.Purchase_Order_No.ID);

        if (modObject.Sales_Order_No != null)
          newModObject.Sales_Order_No = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == modObject.Sales_Order_No.ID);

        if (modObject.Location != null)
          newModObject.Location = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == modObject.Location.ID);

        if (modObject.Party != null)
          newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);

        if (modObject.Shipper != null)
          newModObject.Shipper = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Shipper.ID);

        if (modObject.Asset != null)
          newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == modObject.Asset.ID);

        if (modObject.Container != null)
          newModObject.Container = _repository.GetQuery<Model.Container>().SingleOrDefault(o => o.ID == modObject.Container.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.DispatcherRequest>(predicate, newModObject, includePredicate);
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

    //public IEnumerable<VModel.DispatcherRequest> GetOpenDispatcherByCategory(string category, string[] includePredicate) {
    //  IEnumerable<Model.DispatcherRequest> modDispatcher = _repository.Find<Model.DispatcherRequest>(
    //                                                                          o => o.RequestCategory.Equals(category, StringComparison.InvariantCultureIgnoreCase)
    //                                                                              && o.RequestStatus != "Closed",
    //                                                                              includePredicate
    //                                                                         );
    //  IEnumerable<VModel.DispatcherRequest> busDispatcher = Map(modDispatcher);
    //  return busDispatcher;
    //}

    public IEnumerable<VModel.DispatcherRequest> GetOpenDispatcherByCategoryWithPagging(string category, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.DispatcherRequest> modEnumeration = _repository.FindByPaging<Model.DispatcherRequest>(out totalRows,
                                                                                                              o => o.RequestCategory.Equals(category, StringComparison.InvariantCultureIgnoreCase)
                                                                                                              && o.RequestStatus != "Closed",
                                                                                                              page, pageSize, sortColumn, sortType, includePredicate,
                                                                                                              filters
                                                                                                             );

      IEnumerable<VModel.DispatcherRequest> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }


    public IEnumerable<VModel.DispatcherRequest> GetDispatcherByBookingAndContainerNo(int bookingId, string containerNo, string[] includePredicate) {
      IEnumerable<Model.DispatcherRequest> modDispatcher = _repository.Find<Model.DispatcherRequest>(
                                                                              o => o.Container.Container_No.Equals(containerNo, StringComparison.InvariantCultureIgnoreCase)
                                                                                  && o.Booking_Ref_No.ID == bookingId && o.RequestCategory.Equals("Container", StringComparison.InvariantCultureIgnoreCase),
                                                                                  includePredicate);
      IEnumerable<VModel.DispatcherRequest> busDispatcher = Map(modDispatcher);
      return busDispatcher;
    }


    public override System.Linq.Expressions.Expression<Func<Model.DispatcherRequest, bool>> UniqueEntityExp(Model.DispatcherRequest modelEntity, VModel.DispatcherRequest businessEntity) {
      if (!string.IsNullOrEmpty(businessEntity.Container_No))
        return m => m.RequestCategory.Equals(modelEntity.RequestCategory, StringComparison.InvariantCultureIgnoreCase)
                   && m.RequestType.Equals(modelEntity.RequestType, StringComparison.InvariantCultureIgnoreCase)
                   && m.Container.Container_No.Equals(businessEntity.Container_No, StringComparison.InvariantCultureIgnoreCase)
                   && m.Active_Ind == true
                   && m.ID != modelEntity.ID;
      else
        return null;
    }

  }
}
