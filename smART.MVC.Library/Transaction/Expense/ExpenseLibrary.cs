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

  public abstract class ExpenseLibrary<TEntityBusiness, TEntityModel, TParentEntityBusiness, TParentEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, IParentChildLibrary<TEntityBusiness>

    where TEntityBusiness : VModel.ExpensesRequest, new()
    where TEntityModel : Model.ExpensesRequest, new()
    where TParentEntityBusiness : VModel.BaseEntity, new()
    where TParentEntityModel : Model.BaseEntity, new() {
    public ExpenseLibrary()
      : base() {
    }
    public ExpenseLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    protected override void Initialize(DbContext dbContext) {
      base.Initialize(dbContext);

      Mapper.CreateMap<TEntityModel, TEntityBusiness>();
      Mapper.CreateMap<TEntityBusiness, TEntityModel>();

      Mapper.CreateMap<TParentEntityModel, TParentEntityBusiness>();
      Mapper.CreateMap<TParentEntityBusiness, TParentEntityModel>();

      Mapper.CreateMap<ViewModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, ViewModel.DispatcherRequest>();

      Mapper.CreateMap<ViewModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, ViewModel.AddressBook>();

      Mapper.CreateMap<ViewModel.Bank, Model.Bank>();
      Mapper.CreateMap<Model.Bank, ViewModel.Bank>();

      Mapper.CreateMap<ViewModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, ViewModel.Party>();

      Mapper.CreateMap<ViewModel.PaymentReceipt, Model.PaymentReceipt>();
      Mapper.CreateMap<Model.PaymentReceipt, ViewModel.PaymentReceipt>();

      Mapper.CreateMap<ViewModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, ViewModel.Invoice>();

      Mapper.CreateMap<ViewModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, ViewModel.SalesOrder>();

      Mapper.CreateMap<ViewModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, ViewModel.Scale>();

      Mapper.CreateMap<ViewModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, ViewModel.DispatcherRequest>();
    }

    public IEnumerable<TEntityBusiness> GetAllByPagingByParentID(
        out int totalRows,
        int id,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) {
      string refTableName = GetRefrenceTable();
      //IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => o.Reference_ID == id && o.Reference_Table == typeof(TParentEntityModel).Name, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => o.Reference_ID == id && o.Reference_Table == refTableName, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      foreach (var item in busEnumeration)    //Manually filling Parent Object.
            {
        TEntityModel mExpense = modEnumeration.SingleOrDefault(o => o.ID == item.ID);
        if (mExpense.Reference_ID > 0) {
          TParentEntityModel modParentEntity = (TParentEntityModel)_repository.Single<TParentEntityModel>(o => o.ID == mExpense.Reference_ID);
          TParentEntityBusiness busParentEntity = Mapper.Map<TParentEntityModel, TParentEntityBusiness>(modParentEntity);
        }

      }

      return busEnumeration;

    }

    public IEnumerable<TEntityBusiness> GetAllByParentID(
        int parentId,
        string[] includePredicate = null) {
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.Find<Model.ExpensesRequest>(o => o.Reference_ID == parentId, includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public override TEntityBusiness Add(TEntityBusiness addObject) {
      TEntityBusiness insertedObjectBusiness = addObject;
      try {

        int scaleId = 0;
        int dispatcherRequestId = 0;
        int paid_Party_To_ID = 0;
        int payment_ID = 0;
        int invoice_ID = 0;

        TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(addObject);

        newModObject.Reference_ID = addObject.Reference_ID;
        newModObject.Reference_Table = GetRefrenceTable();

        if (addObject.Paid_Party_To != null)
          paid_Party_To_ID = addObject.Paid_Party_To.ID;
        newModObject.Paid_Party_To = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == paid_Party_To_ID);

        if (addObject.Payment != null)
          payment_ID = addObject.Payment.ID;
        newModObject.Payment = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == payment_ID);

        if (addObject.Invoice != null)
          invoice_ID = addObject.Invoice.ID;
        newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == invoice_ID);

        if (newModObject.Reference_Table == "Scale")
          scaleId = newModObject.Reference_ID;
        newModObject.Scale_Ref = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == scaleId);

        if (newModObject.Reference_Table == "DispatcherRequest")
          dispatcherRequestId = newModObject.Reference_ID;
        newModObject.Dispatcher_Request_Ref = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == dispatcherRequestId);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          TEntityModel insertedObject = _repository.Add<TEntityModel>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<TEntityModel, TEntityBusiness>(insertedObject);
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

    protected override void Modify(Expression<Func<TEntityModel, bool>> predicate, TEntityBusiness modObject, string[] includePredicate = null) {
      try {

        TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(modObject);

        if (newModObject.Paid_Party_To != null)
          newModObject.Paid_Party_To = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == newModObject.Paid_Party_To.ID);

        if (newModObject.Payment != null)
          newModObject.Payment = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == newModObject.Payment.ID);

        if (newModObject.Invoice != null)
          newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == newModObject.Invoice.ID);

        if (newModObject.Reference_Table == "Scale")
          newModObject.Scale_Ref = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == newModObject.Reference_ID);

        if (newModObject.Reference_Table == "DispatcherRequest")
          newModObject.Dispatcher_Request_Ref = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == newModObject.Reference_ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<TEntityModel>(predicate, newModObject, includePredicate);
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

    protected  IEnumerable<TEntityBusiness> GetAllByPaging(
          out int totalRows,
          int page,
          int pageSize,
          string sortColumn,
          string sortType,
          string[] includePredicate = null,
          IList<IFilterDescriptor> filters = null) {

      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, null, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      foreach (var item in busEnumeration)    //Manually filling Parent Object.
            {
        TEntityModel mExpense = modEnumeration.SingleOrDefault(o => o.ID == item.ID);
        if (mExpense.Reference_ID > 0) {
          TParentEntityModel modParentEntity = (TParentEntityModel)_repository.Single<TParentEntityModel>(o => o.ID == mExpense.Reference_ID);
          TParentEntityBusiness busParentEntity = Mapper.Map<TParentEntityModel, TParentEntityBusiness>(modParentEntity);
        }
      }
      return busEnumeration;
    }

    public abstract string GetRefrenceTable();

    public IEnumerable<TEntityBusiness> GetAllUnApprovedExpensesWithPagging(
          out int totalRows,
          int page,
          int pageSize,
          string sortColumn,
          string sortType,
          string[] includePredicate = null,
          IList<IFilterDescriptor> filters = null) {
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => string.IsNullOrEmpty(o.Expense_Status.Trim()), page, pageSize, sortColumn, sortType, includePredicate, filters);
      UpdateScaleRef(modEnumeration);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllPurchasingExepneseByRefTableAndRefId(int refId, string refTableName, string[] includePredicate = null) {
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.Find<Model.ExpensesRequest>(o => (o.Payment.ID == null || o.Payment.ID == 0) && o.Reference_ID == refId && o.Reference_Table == refTableName && o.Paid_By == "Party", includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllSellingExepneseByRefTableAndRefId(int refId, string refTableName, string[] includePredicate = null) {
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.Find<Model.ExpensesRequest>(o => (o.Invoice.ID == null || o.Invoice.ID == 0) && o.Reference_ID == refId && o.Reference_Table == refTableName && o.Paid_By == "Party", includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllByPagingByPaymentID(
           out int totalRows,
           int id,
           int page,
           int pageSize,
           string sortColumn,
           string sortType,
           string[] includePredicate = null,
           IList<IFilterDescriptor> filters = null) {
      string refTableName = GetRefrenceTable();
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => o.Payment.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllByPaymentID(           
             int id,
             string[] includePredicate = null) {      
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.Find<Model.ExpensesRequest>( o => o.Payment.ID == id, includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllByPagingByInvoiceID(
              out int totalRows,
              int id,
              int page,
              int pageSize,
              string sortColumn,
              string sortType,
              string[] includePredicate = null,
              IList<IFilterDescriptor> filters = null) {
      string refTableName = GetRefrenceTable();
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => o.Invoice.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<TEntityBusiness> GetAllByInvoiceID(                  
                  int id,
                  string[] includePredicate = null) {      
      IEnumerable<TEntityModel> modEnumeration = (IEnumerable<TEntityModel>)_repository.Find<Model.ExpensesRequest>(o => o.Invoice.ID == id, includePredicate);
      IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
      return busEnumeration;
    }

    protected void UpdateScaleRef(IEnumerable<Model.ExpensesRequest> modEnumeration) {
      foreach (var item in modEnumeration) {
        if (item.Dispatcher_Request_Ref != null && item.Dispatcher_Request_Ref.Container != null) {
          item.Scale_Ref = _repository.Find<Model.Scale>(s => s.Container_No != null
                                                         && s.Container_No.ID == item.Dispatcher_Request_Ref.Container.ID
                                                         && s.Active_Ind == true && s.Container_No.Active_Ind == true
                                                        , new String[] { "Container_No"}).FirstOrDefault();
        }
      }
    }
  }
}
