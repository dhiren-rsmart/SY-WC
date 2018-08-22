using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using smART.Model;
using AutoMapper;
using System.Linq.Expressions;
using Telerik.Web.Mvc;
using System.Reflection;

using smART.MVC.Library.BusinessRules;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library {
  public abstract class GenericLibrary<TEntityBusiness, TEntityModel> : ILibrary<TEntityBusiness>
    where TEntityBusiness : VModel.BaseEntity, new()
    where TEntityModel : Model.BaseEntity, new() {
    public delegate void delegateActionPerforming(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext, out bool cancel);
    public delegate void delegateActionPerformned(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext);
    public delegate void delegateGetActionPerformned(IEnumerable<TEntityBusiness> businessEntities, IEnumerable<TEntityModel> modelEntities, DbContext dbContext);

    public event delegateActionPerforming OnAdding, OnModifying, OnDeleting;
    public event delegateActionPerformned OnAdded, OnModified, OnDeleted, OnGotSingle;
    public event delegateGetActionPerformned OnGotMultiple;

    protected List<BusinessRuleElement> AddedHandlers = new List<BusinessRuleElement>();
    protected List<BusinessRuleElement> ModifiedHandlers = new List<BusinessRuleElement>();
    protected List<BusinessRuleElement> DeletedHandlers = new List<BusinessRuleElement>();

    protected List<BusinessRuleElement> AddingHandlers = new List<BusinessRuleElement>();
    protected List<BusinessRuleElement> ModifyingHandlers = new List<BusinessRuleElement>();
    protected List<BusinessRuleElement> DeletingHandlers = new List<BusinessRuleElement>();

    protected List<BusinessRuleElement> GotMultipleHandlers = new List<BusinessRuleElement>();
    protected List<BusinessRuleElement> GotSingleHandlers = new List<BusinessRuleElement>();


    protected void AddHandlers() {
      BusinessRuleSection brSection = new BusinessRuleSection();

      ClearHandler();

      AddedHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Added"));
      ModifiedHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Modified"));
      DeletedHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Deleted"));

      AddingHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Adding"));
      ModifyingHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Modifying"));
      DeletingHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "Deleting"));

      GotMultipleHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "GotMultiple"));
      GotSingleHandlers.AddRange(brSection.GetBusinessRules(this.GetType().Name, "GotSingle"));

    }

    private void ClearHandler() {
      AddedHandlers.Clear();
      ModifiedHandlers.Clear();
      DeletedHandlers.Clear();

      AddingHandlers.Clear();
      ModifyingHandlers.Clear();
      DeletingHandlers.Clear();

      GotMultipleHandlers.Clear();
      GotSingleHandlers.Clear();
    }


    protected bool Adding(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      bool cancel = false;
      try {
        // Check Unique constraints
        ValidateUniqueEntity(modelEntity, businessEntity);

        if (OnAdding != null)
          OnAdding(businessEntity, modelEntity, dbContext, out cancel);

        if (AddingHandlers.Count > 0)

          foreach (BusinessRuleElement handler in AddingHandlers) {
            bool cancelVal = BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext, false);

            if (cancelVal)
              return false;
          }
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
      return !cancel;
    }
    protected bool Modifying(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      bool cancel = false;
      try {
        // Check Unique constraints
        ValidateUniqueEntity(modelEntity, businessEntity);

        if (OnModifying != null)
          OnModifying(businessEntity, modelEntity, dbContext, out cancel);

        if (ModifyingHandlers.Count > 0)
          foreach (BusinessRuleElement handler in ModifyingHandlers) {
            bool cancelVal = BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext, false);
            if (cancelVal)
              return false;
          }
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
      return !cancel;
    }
    protected bool Deleting(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      bool cancel = false;
      if (OnDeleting != null)
        OnDeleting(businessEntity, modelEntity, dbContext, out cancel);

      if (DeletingHandlers.Count > 0)
        foreach (BusinessRuleElement handler in DeletingHandlers) {
          bool cancelVal = BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext, false);
          if (cancelVal)
            return false;
        }
      return !cancel;
    }
    protected void Added(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      if (OnAdded != null)
        OnAdded(businessEntity, modelEntity, dbContext);
      if (AddedHandlers.Count > 0)
        foreach (BusinessRuleElement handler in AddedHandlers)
          BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext);
    }
    protected void Modified(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      if (OnModified != null)
        OnModified(businessEntity, modelEntity, dbContext);
      if (ModifiedHandlers.Count > 0)
        foreach (BusinessRuleElement handler in ModifiedHandlers)
          BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext);
    }
    protected void Deleted(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      if (OnDeleted != null)
        OnDeleted(businessEntity, modelEntity, dbContext);
      if (DeletedHandlers.Count > 0)
        foreach (BusinessRuleElement handler in DeletedHandlers)
          BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext);
    }

    protected void GotMultiple(IEnumerable<TEntityBusiness> businessEntities, IEnumerable<TEntityModel> modelEntities, DbContext dbContext) {
      if (OnGotMultiple != null)
        OnGotMultiple(businessEntities, modelEntities, dbContext);
      if (GotMultipleHandlers.Count > 0)
        foreach (BusinessRuleElement handler in GotMultipleHandlers)
          BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntities, modelEntities, dbContext);
    }

    protected void GetSingle(TEntityBusiness businessEntity, TEntityModel modelEntity, DbContext dbContext) {
      if (OnGotSingle != null)
        OnGotSingle(businessEntity, modelEntity, dbContext);
      if (GotSingleHandlers.Count > 0)
        foreach (BusinessRuleElement handler in GotSingleHandlers)
          BusinessRuleEngine.InvokeMethod(handler.Type, handler.MethodName, businessEntity, modelEntity, dbContext);
    }

    protected void ValidateUniqueEntity(TEntityModel modelEntity, TEntityBusiness businessEntity) {
      Expression<Func<TEntityModel, bool>> exp = UniqueEntityExp(modelEntity, businessEntity);
      if (exp != null) {
        TEntityModel result = _repository.Find<TEntityModel>(exp).FirstOrDefault();
        bool exists = result != null && result.ID > 0;
        if (exists)
          throw new smART.Common.DuplicateException();
      }
    }

    public virtual Expression<Func<TEntityModel, bool>> UniqueEntityExp(TEntityModel modelEntity, TEntityBusiness businessEntity) {
      return null;
    }

    public DbContext _dbContext;
    public IRepository _repository;
    protected string _dbContextConnectionString;

    public GenericLibrary() {
      AddHandlers();
    }

    public GenericLibrary(DbContext dbContext) {
      this.Initialize(dbContext);
      AddHandlers();
    }

    public GenericLibrary(string dbContextConnectionString)
      : this(new Model.smARTDBContext(dbContextConnectionString)) {
      _dbContextConnectionString = dbContextConnectionString;
      AddHandlers();
    }

    #region ILibrary Members

    public virtual void Initialize(string dbContextConnectionString) {
      _dbContextConnectionString = dbContextConnectionString;
      this.Initialize(new smARTDBContext(dbContextConnectionString));
    }

    protected virtual void Initialize(DbContext dbContext) {
      try {
        _dbContext = dbContext;
        _repository = new GenericRepository(_dbContext);
        Mapper.CreateMap<TEntityModel, TEntityBusiness>();
        Mapper.CreateMap<TEntityBusiness, TEntityModel>();
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
    }

    public void Map() {
      try {
        Mapper.CreateMap<TEntityModel, TEntityBusiness>();
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
    }

    public IEnumerable<TEntityBusiness> Map(IEnumerable<TEntityModel> dbEntities) {
      try {
        IEnumerable<TEntityBusiness> busEntities = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(dbEntities);
        return busEntities;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }

    public IEnumerable<TEntityModel> Map(IEnumerable<TEntityBusiness> dbEntities) {
      try {
        IEnumerable<TEntityModel> modEntities = Mapper.Map<IEnumerable<TEntityBusiness>, IEnumerable<TEntityModel>>(dbEntities);
        return modEntities;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }

    public virtual IEnumerable<TEntityBusiness> GetAll(string[] includePredicate = null) {
      try {
        IEnumerable<TEntityModel> modEnumeration = _repository.GetAll<TEntityModel>(includePredicate);
        IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);

        return busEnumeration;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }

    public virtual IEnumerable<TEntityBusiness> GetAllByPaging(
        out int totalRows,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) {
      try {
        IEnumerable<TEntityModel> modEnumeration = _repository.GetAllByPaging<TEntityModel>(out totalRows, page, pageSize, sortColumn, sortType, includePredicate, filters);
        IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
        GotMultiple(busEnumeration, modEnumeration, _dbContext);
        return busEnumeration;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        totalRows = 0;
        return null;
      }
    }

    public virtual IEnumerable<TEntityBusiness> GetAllByPaging(IEnumerable<TEntityBusiness> newBusObjects,
           out int totalRows,
           int page,
           int pageSize,
           string sortColumn,
           string sortType,
           string[] includePredicate = null,
           IList<IFilterDescriptor> filters = null) {
      try {
        IEnumerable<TEntityModel> newModObjects = Mapper.Map<IEnumerable<TEntityBusiness>, IEnumerable<TEntityModel>>(newBusObjects);
        IEnumerable<TEntityModel> modEnumeration = _repository.GetAllByPaging<TEntityModel>(newModObjects.AsQueryable(), out totalRows, page, pageSize, sortColumn, sortType, includePredicate, filters);
        IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
        GotMultiple(busEnumeration, modEnumeration, _dbContext);
        return busEnumeration;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        totalRows = 0;
        return null;
      }
    }
    public virtual TEntityBusiness GetByID(string id, string[] includePredicate = null) {
      try {
        // EmpRole => RoleId
        // M_Feturee => FeatureId
        // DataAccessControlMaster => IsReadOnly
        string idFieldName = "ID";
        TEntityBusiness entity = this.GetSingleByCriteria(GetModelIDExpression(idFieldName, Convert.ToInt32(id), typeof(int)), includePredicate);

        //if (userId != null && entity != null) {
        //  entity.IsReadOnly = IsDataAccessRights();
        //}

        return entity;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        return null;
      }
    }

    protected virtual IEnumerable<TEntityBusiness> GetByExpression(Expression<Func<TEntityModel, bool>> predicate, string[] includePredicate = null) {
      try {
        IEnumerable<TEntityModel> modEnumeration = _repository.Find(predicate, includePredicate);
        IEnumerable<TEntityBusiness> busEnumeration = Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TEntityBusiness>>(modEnumeration);
        GotMultiple(busEnumeration, modEnumeration, _dbContext);
        return busEnumeration;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    protected virtual TEntityBusiness GetSingleByExpression(Expression<Func<TEntityModel, bool>> predicate, string[] includePredicate = null) {
      try {
        TEntityBusiness entity = this.GetSingleByCriteria(predicate, includePredicate);
        return entity;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public virtual TEntityBusiness Add(TEntityBusiness addObject) {
      TEntityBusiness insertedObjectBusiness = addObject;
      try {
        TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(addObject);
        if (Adding(addObject, newModObject, _dbContext)) {
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

    public virtual TEntityBusiness Modify(TEntityBusiness modObject, string[] includePredicate = null) {
      try {
        string idFieldName = "ID";
        PropertyInfo property = typeof(TEntityBusiness).GetProperty(idFieldName);
        this.Modify(GetModelIDExpression(idFieldName, property.GetValue(modObject, null), typeof(int)), modObject, includePredicate);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
        if (rethrow)
          throw ex;
      }
      return modObject;
    }

    public virtual void Delete(string id, string[] includePredicate = null) {
      string idFieldName = "ID";
      this.Delete(GetModelIDExpression(idFieldName, Convert.ToInt32(id), typeof(int)), includePredicate);
    }

    protected virtual TEntityBusiness GetSingleByCriteria(Expression<Func<TEntityModel, bool>> predicate, string[] includePredicate = null) {
      try {
        IQueryable<TEntityModel> getQuery = _repository.GetQuery<TEntityModel>();
        if (includePredicate != null) {
          foreach (string singlePredicate in includePredicate) {
            getQuery = getQuery.Include(singlePredicate);
          }
        }
        TEntityModel dbEntity = getQuery.AsQueryable().SingleOrDefault(predicate);
        TEntityBusiness busEntity = Mapper.Map<TEntityModel, TEntityBusiness>(dbEntity);

        GetSingle(busEntity, dbEntity, _dbContext);

        return busEntity;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    protected virtual void Modify(Expression<Func<TEntityModel, bool>> predicate, TEntityBusiness modObject, string[] includePredicate = null) {
      try {
        TEntityModel newModObject = Mapper.Map<TEntityBusiness, TEntityModel>(modObject);

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

    //protected virtual void Delete(Expression<Func<TEntityModel, bool>> predicate)
    //{
    //    _repository.Delete<TEntityModel>(predicate);
    //    _repository.SaveChanges();
    //}

    protected virtual void Delete(Expression<Func<TEntityModel, bool>> predicate, string[] includePredicate = null) {
      try {
        IQueryable<TEntityModel> getQuery = _repository.GetQuery<TEntityModel>();
        if (includePredicate != null) {
          foreach (string singlePredicate in includePredicate) {
            getQuery = getQuery.Include(singlePredicate);
          }
        }
        TEntityModel dbEntity = getQuery.AsQueryable().SingleOrDefault(predicate);
        TEntityBusiness busEntity = Mapper.Map<TEntityModel, TEntityBusiness>(dbEntity);

        _repository.Delete<TEntityModel>(predicate);
        _repository.SaveChanges();

        Deleted(busEntity, dbEntity, _dbContext);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
      }
    }

    protected virtual Expression<Func<TEntityModel, bool>> GetModelIDExpression(string idFieldName, object idFieldValue, Type idFieldType) {
      try {
        ParameterExpression paramExp = Expression.Parameter(typeof(TEntityModel));
        MemberExpression memberExp = Expression.PropertyOrField(paramExp, idFieldName);
        ConstantExpression valueExp = Expression.Constant(idFieldValue, idFieldType);
        Expression exp = Expression.Equal(memberExp, valueExp);
        return Expression.Lambda<Func<TEntityModel, bool>>(exp, paramExp);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    protected virtual bool IsDataAccessRights(string userId, string featureName) {
      IQueryable<Feature> getQuery = _repository.GetQuery<Feature>();
      Feature dbEntity = getQuery.AsQueryable().SingleOrDefault();

      return true;
    }


    #endregion
  }
}
