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

  public class ContainerLibrary : GenericLibrary<VModel.Container, Model.Container>, IParentChildLibrary<VModel.Container> {
    public ContainerLibrary()
      : base() {
    }
    public ContainerLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();


      Mapper.CreateMap<VModel.Container, Model.Container>();
      Mapper.CreateMap<Model.Container, VModel.Container>();
    }


    public override VModel.Container Add(VModel.Container addObject) {
      VModel.Container insertedObjectBusiness = addObject;
      try {
        Model.Container newModObject = Mapper.Map<VModel.Container, Model.Container>(addObject);
        int bookingId = 0;
        if (addObject.Booking != null)
          bookingId = addObject.Booking.ID;
        newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == bookingId);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.Container insertedObject = _repository.Add<Model.Container>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.Container, VModel.Container>(insertedObject);
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

    protected override void Modify(Expression<Func<Model.Container, bool>> predicate, VModel.Container modObject, string[] includePredicate = null) {
      try {
        Model.Container newModObject = Mapper.Map<VModel.Container, Model.Container>(modObject);
        if (modObject.Booking != null)
          newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.Container>(predicate, newModObject, includePredicate);
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

    //public VModel.Container TransferContainerFromDispatcher(VModel.Container addObject)
    //{           

    //    if (IsValidate(addObject))
    //    {
    //        VModel.Container insertedObjectBusiness = addObject;
    //        Model.Container newModObject = Mapper.Map<VModel.Container, Model.Container>(addObject);
    //        newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.Booking_Ref_No == addObject.Booking.Booking_Ref_No);

    //        if (Adding(insertedObjectBusiness, newModObject, _dbContext))
    //        {
    //            Model.Container insertedObject = _repository.Add<Model.Container>(newModObject);
    //            _repository.SaveChanges();

    //            insertedObjectBusiness = Mapper.Map<Model.Container, VModel.Container>(insertedObject);
    //            Added(insertedObjectBusiness, newModObject, _dbContext);
    //        }
    //        return insertedObjectBusiness;              

    //    }

    //    return null;
    //}

    //private bool IsValidate(VModel.Container addObject)
    //{
    //    if (string.IsNullOrEmpty(addObject.Booking.Booking_Ref_No) && string.IsNullOrEmpty(addObject.Container_No))
    //    {
    //        return false ;
    //    }

    //    if (Exits(addObject)){
    //        return false ;
    //    }

    //    return true ;
    //}

    //public bool Exits(VModel.Container addObject)
    //{
    //   IEnumerable<VModel.Container> containers = GetByBookingRefNoAndContainerNo(addObject.Booking.Booking_Ref_No, addObject.Container_No);

    //   if (containers == null || containers .Count()<=0 ) return false;
    //    return true;
    //}

    public IEnumerable<VModel.Container> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Container> modEnumeration = _repository.FindByPaging<Model.Container>(out totalRows, o => o.Booking.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Container> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);

      //foreach (VModel.Container c in  busEnumeration ){c.Container_Return_Date =   AddBusinessDays(c.Date_In,4);}

      return busEnumeration;
    }

    public IEnumerable<VModel.Container> GetAllByParentID(
        int parentId,
        string[] includePredicate = null) {
      IEnumerable<Model.Container> modEnumeration = _repository.Find<Model.Container>(o => o.Booking.ID == parentId, includePredicate);
      IEnumerable<VModel.Container> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);
      return busEnumeration;
    }


    public virtual IEnumerable<VModel.Container> GetOpenContainers() {
      IEnumerable<Model.Container> modParties = from parties in _repository.GetQuery<Model.Container>()
                                                where parties.Status.ToLower() == "open-empty" || parties.Status.ToLower() == "wip"
                                                select parties;
      IEnumerable<VModel.Container> busParties = Map(modParties);
      return busParties;
    }

    public virtual IEnumerable<VModel.Container> GetOpenContainersWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Container> modEnumeration = _repository.FindByPaging<Model.Container>(out totalRows,
                                                      o => o.Status.ToLower() == "open-empty" || o.Status.ToLower() == "wip",
                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Container> busEnumeration = Map(modEnumeration);
      return busEnumeration;

    }

    public virtual IEnumerable<VModel.Container> GetOpenBookingContainersWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Container> modEnumeration = _repository.FindByPaging<Model.Container>(out totalRows,
                                                      o => o.Booking.Booking_Status.Equals("Open", StringComparison.OrdinalIgnoreCase) && o.Status.ToLower() == "open-empty",
                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Container> busEnumeration = Map(modEnumeration);
      return busEnumeration;

    }

    public virtual IEnumerable<VModel.Container> GetByBookkingRefNo(string bookingRefNo) {
      IEnumerable<Model.Container> modParties = from parties in _repository.GetQuery<Model.Container>()
                                                where parties.Booking.Booking_Ref_No.Equals(bookingRefNo)
                                                select parties;
      IEnumerable<VModel.Container> busParties = Map(modParties);
      return busParties;
    }

    public bool IsRefExits(int containerId, out string errorMsg) {
      errorMsg = string.Empty;
      var result1 = from s in _repository.GetQuery<Model.Scale>()
                    where s.Container_No.ID == containerId && s.Active_Ind==true
                    select new { ID = s.ID };

      var result2 = from d in _repository.GetQuery<Model.DispatcherRequest>()
                    where d.Container.ID == containerId && d.Active_Ind==true
                    select new { ID = d.ID };
      errorMsg = "Please check if Scale ticket/Dispatcher Request exist for this container and delete those transactions before deleting this Container record";
      var result = result1.Union(result2);
      return result.Count() > 0;
    }

    public virtual IEnumerable<VModel.Container> GetLoadedContainersWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Container> modEnumeration = _repository.FindByPaging<Model.Container>(out totalRows,
                                                                                              o => o.Status.ToLower() == "open-loaded",
                                                                                              page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Container> busEnumeration = Map(modEnumeration);
      return busEnumeration;

    }

    public override System.Linq.Expressions.Expression<Func<Model.Container, bool>> UniqueEntityExp(Model.Container modelEntity, VModel.Container businessEntity) {
      return m => m.Container_No.Equals(modelEntity.Container_No, StringComparison.InvariantCultureIgnoreCase)
                  && m.Booking.Booking_Ref_No.Equals(modelEntity.Booking.Booking_Ref_No, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }

    //public virtual IEnumerable<VModel.Container> GetByBookingRefNoAndContainerNo(string bookingRefNo,string containerNo)
    //{
    //    IEnumerable<Model.Container> modParties = from parties in _repository.GetQuery<Model.Container>()
    //                                              where parties.Booking.Booking_Ref_No.Equals(bookingRefNo) && parties.Container_No.Equals(containerNo)
    //                                              select parties;
    //    IEnumerable<VModel.Container> busParties = Mapper.Map<IEnumerable<Model.Container>, IEnumerable<VModel.Container>>(modParties);
    //    return busParties;
    //}




  }
}

