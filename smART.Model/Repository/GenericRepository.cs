using System.Data.Entity;
using System.Linq;
using System.Data.Objects;
using System.Data.Entity.Design.PluralizationServices;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using System.Data.Objects.DataClasses;
using Telerik.Web.Mvc;
using System.Reflection;
using System.Data.Entity.Validation;
using System.Diagnostics;
using smART.Common;
using System.Web;

namespace smART.Model {
  /// <summary>
  /// A generic repository for working with data in the database
  /// </summary>
  /// <typeparam name="T">A POCO that represents an Entity Framework entity</typeparam>
  public class GenericRepository : IRepository {
    /// <summary>
    /// The context object for the database
    /// </summary>
    private DbContext _context;
    private readonly PluralizationService _pluralizer;

    /// <summary>
    /// Initializes a new instance of the GenericRepository class
    /// </summary>
    /// <param name="context">The Entity Framework ObjectContext</param>
    public GenericRepository(DbContext context) {
      _context = context;
      _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
    }

    public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class {
      return _context.Set<TEntity>().AsQueryable();
    }

    public IQueryable<TEntity> GetQueryActiveItems<TEntity>() where TEntity : BaseEntity {
      return _context.Set<TEntity>().AsQueryable().Where(o => o.Active_Ind == true);
    }

    public IEnumerable<TEntity> GetAll<TEntity>(string[] includePredicate = null) where TEntity : BaseEntity {
      try {
        if (includePredicate == null)
          //return GetQuery<TEntity>().AsEnumerable<TEntity>();
          return GetQueryActiveItems<TEntity>().AsEnumerable<TEntity>(); //Chnage by sanjeev for deleted items
        else {
          IQueryable<TEntity> result = GetQueryActiveItems<TEntity>();

          foreach (string predicate in includePredicate)
            result = result.Include(predicate);

          return result.AsEnumerable<TEntity>();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public IEnumerable<TEntity> GetAllByPaging<TEntity>(
        out int totalRows,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity {
      totalRows = 0;
      try {
        IQueryable<TEntity> result = GetQueryActiveItems<TEntity>();
        return GetAllByPaging<TEntity>(result, out totalRows, page, pageSize,sortColumn, sortType, includePredicate, filters);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public IEnumerable<TEntity> GetAllByPaging<TEntity>(
           IQueryable<TEntity> result,
            out int totalRows,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null
        ) where TEntity : BaseEntity {
      totalRows = 0;
      try {
        if (filters != null)
          result = FilterExpression(result, filters);

        if (!string.IsNullOrEmpty(sortColumn)) {
          if (sortType.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            result = result.OrderByDescending<TEntity>(sortColumn);
          else
            result = result.OrderBy<TEntity>(sortColumn);
        }
        else {
          result = (from data in result
                    orderby sortColumn ascending
                    select data);
        }


        totalRows = result.Count();

        result = result.Skip((page <= 0 ? 0 : page - 1) * pageSize).Take(pageSize);

        if (includePredicate != null)
          foreach (string predicate in includePredicate)
            result = result.Include(predicate);

        return result.AsEnumerable<TEntity>();
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }
    public IQueryable Sort<T>(IQueryable<T> source, string sortBy, string sortOrder) {
      //string[] sortParts = sortExpression.Split(' ');
      var param = Expression.Parameter(typeof(T), string.Empty);
      try {

        if (source.AsEnumerable().Count() <= 0 || (string.IsNullOrEmpty(sortBy))) {
          return (from data in source
                  orderby sortBy ascending
                  select data);
        }
        var property = Expression.Property(param, sortBy);
        var sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

        if (sortOrder.Length > 1 && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
          return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);
        }
        return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
      }
      catch (ArgumentException) {
        return source;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }

    }


    public IEnumerable<TEntity> FindByPaging<TEntity>(
        out int totalRows,
        Expression<Func<TEntity, bool>> predicate,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity {
      totalRows = 0;
      try {
        IQueryable<TEntity> query = GetQueryActiveItems<TEntity>();
        return FindByPaging<TEntity>(query, out totalRows, predicate, page, pageSize, sortColumn, sortType, includePredicate, filters);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public IEnumerable<TEntity> FindByPaging<TEntity>(
            IQueryable<TEntity> query,
            out int totalRows,
            Expression<Func<TEntity, bool>> predicate,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity {
      totalRows = 0;
      try {
        if (filters != null)
          query = FilterExpression(query, filters);

        if (predicate != null)
          query = query.Where(predicate);

        totalRows = query.Count();

        if (!string.IsNullOrEmpty(sortColumn)) {
          if (sortType.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            query = query.OrderByDescending<TEntity>(sortColumn);
          else
            query = query.OrderBy<TEntity>(sortColumn);
        }
        else {
          query = (from data in query
                   orderby sortColumn ascending
                   select data);
        }

        if (includePredicate == null)
          return (from data in query
                  //orderby sortColumn
                  select data).Skip((page <= 0 ? 0 : page - 1) * pageSize).Take(pageSize).AsEnumerable<TEntity>();
        else {
          IQueryable<TEntity> result = (from data in query
                                        //orderby sortColumn
                                        select data).Skip((page <= 0 ? 0 : page - 1) * pageSize).Take(pageSize);

          foreach (string strpredicate in includePredicate)
            result = result.Include(strpredicate);

          return result.AsEnumerable<TEntity>();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate, string[] includePredicate = null) where TEntity : BaseEntity {
      try {
        IQueryable<TEntity> result = GetQueryActiveItems<TEntity>().Where(predicate);

        if (includePredicate != null)
          foreach (string strpredicate in includePredicate)
            result = result.Include(strpredicate);

        return result.AsEnumerable();
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity {
      try {
        return GetQueryActiveItems<TEntity>().Single<TEntity>(predicate);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity {
      try {
        return GetQueryActiveItems<TEntity>().Where(predicate).FirstOrDefault();
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public TEntity Last<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity {
      try {
        IEnumerable<TEntity> entities=  GetQueryActiveItems<TEntity>().Where(predicate);
        return entities.LastOrDefault();
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }
    public TEntity Add<TEntity>(TEntity entity) where TEntity : BaseEntity {
      try {
        entity.Created_Date = DateTime.Now;
        entity.Active_Ind = true;
        //ValidateDuplicatedKeys<TEntity>(entity);
        TEntity newEntity = _context.Set<TEntity>().Add(entity);
        return newEntity;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex, entity.Created_By, entity.GetType().Name, entity.ID.ToString());
        if (rethrow)
          throw ex;
        return null;
      }
    }

    public void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, bool hardDelete = false) where TEntity : BaseEntity {
      try {
        IEnumerable<TEntity> originalEntities = this.GetQueryActiveItems<TEntity>().Where(predicate).AsEnumerable();
        if (originalEntities == null)
          return;

        //_context.Set<TEntity>().Remove(originalEntity);

        if (hardDelete) {
          foreach (TEntity originalEntity in originalEntities) {
            _context.Set<TEntity>().Remove(originalEntity);
          }
        }
        else {
          foreach (TEntity originalEntity in originalEntities) {
            originalEntity.Active_Ind = false;
            originalEntity.Last_Updated_Date = DateTime.Now;
            originalEntity.Updated_By = HttpContext.Current.User.Identity.Name;
            DbEntityEntry entry = _context.Entry<TEntity>(originalEntity);
            entry.CurrentValues.SetValues(originalEntity);
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
      }
    }

    public void Modify<TEntity>(Expression<Func<TEntity, bool>> predicate, TEntity entity, string[] includePredicate = null, bool active_Ind = true) where TEntity : BaseEntity {
      try {
        entity.Active_Ind = active_Ind;

        TEntity originalEntity;
        if (includePredicate != null)//Added By Dharmendra to fill associated properties.
            {
          IQueryable<TEntity> result = this.GetQuery<TEntity>();
          foreach (var item in includePredicate) {
            result = result.Include(item);
          }

          originalEntity = result.SingleOrDefault(predicate);
        }
        else {
          originalEntity = this.GetQuery<TEntity>().SingleOrDefault(predicate);
        }

        if (!String.IsNullOrEmpty(originalEntity.Created_By))
          entity.Created_By = originalEntity.Created_By;
        if (originalEntity.Created_Date.HasValue)
          entity.Created_Date = originalEntity.Created_Date;

        DbEntityEntry entry = _context.Entry<TEntity>(originalEntity);
        //ValidateDuplicatedKeys<TEntity>(entity);
        entry.CurrentValues.SetValues(entity);
        if (includePredicate != null)//Added By Dharmendra to fill associated properties.
            {
          Type t = typeof(TEntity);
          foreach (var item in includePredicate) {
            System.Reflection.PropertyInfo pInfo = t.GetProperty(item);
            entry.Reference(item).CurrentValue = pInfo.GetValue(entity, null);
          }

        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex, entity.Updated_By, entity.GetType().Name, entity.ID.ToString());
        if (rethrow)
          throw ex;
      }
      //entry.Reference("Party").CurrentValue = (entity as SalesOrder).Party;
    }

    private string GetEntityName<TEntity>() where TEntity : class {
      try {
        return string.Format("ObjectContext.{0}", _pluralizer.Pluralize(typeof(TEntity).Name));
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }

    }

    /// <summary>
    /// Saves all context changes
    /// </summary>
    public void SaveChanges() {
      try {
        _context.SaveChanges();
      }
      catch (DbEntityValidationException dbEx) {
        foreach (var validationErrors in dbEx.EntityValidationErrors) {
          foreach (var validationError in validationErrors.ValidationErrors) {
            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
          }
        }
      }
    }

    /// <summary>
    /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
    /// </summary>
    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        if (_context != null) {
          _context.Dispose();
          _context = null;
        }
      }
    }

    private IQueryable<TEntity> FilterExpression<TEntity>(IQueryable<TEntity> query, IList<IFilterDescriptor> filters)
        where TEntity : class {
      foreach (IFilterDescriptor filter in filters)
        query = FilterExpression<TEntity>(query, filter);

      return query;
    }

    private IQueryable<TEntity> FilterExpression<TEntity>(IQueryable<TEntity> query, IFilterDescriptor filter)
        where TEntity : class {
      try {
        Expression<Func<TEntity, bool>> filterExpression = null;

        if (filter is CompositeFilterDescriptor) {
          foreach (IFilterDescriptor childFilter in ((CompositeFilterDescriptor) filter).FilterDescriptors) {
            query = FilterExpression<TEntity>(query, childFilter);
          }
        }
        else {
          FilterDescriptor filterDescriptor = (FilterDescriptor)filter;
          ParameterExpression paramExp = Expression.Parameter(typeof(TEntity));

          Expression exp = filterDescriptor.CreateFilterExpression(paramExp);

          //MemberExpression fieldExp = Expression.PropertyOrField(paramExp, filterDescriptor.Member);
          //ConstantExpression valueExp = Expression.Constant(filterDescriptor.Value, filterDescriptor.Value.GetType());

          //Expression exp = null;

          //if (fieldExp.Type == typeof(string))
          //{
          //    MethodInfo method = typeof(string).GetMethod(filterDescriptor.Operator.ToString(), new[] { typeof(string) });
          //    exp = Expression.Call(fieldExp, method, valueExp);
          //}
          //else if (filterDescriptor.MemberType == typeof(int)
          //    || filterDescriptor.MemberType == typeof(long))
          //{
          //    exp = Expression.LessThanOrEqual(fieldExp, valueExp);
          //}
          //else
          //{
          //    exp = Expression.Equal(fieldExp, valueExp);
          //}

          filterExpression = Expression.Lambda<Func<TEntity, bool>>(exp, paramExp);
          query = query.Where(filterExpression);

        }
        return query;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }

    }

    //private void ValidateDuplicatedKeys<T>(T entity) where T : BaseEntity {
    //  var atts = typeof(T).GetCustomAttributes(typeof(UniqueAttribute), true); if (atts == null || atts.Count() < 1) {
    //    return;
    //  }

    //  foreach (var att in atts) {
    //    UniqueAttribute uniqueAtt = (UniqueAttribute)att;
    //    var newkeyValues = from pi in entity.GetType().GetProperties()
    //                       join k in uniqueAtt.KeyFields on pi.Name equals k
    //                       select new {
    //                         KeyField = k,
    //                         Value = pi.GetValue(entity, null).ToString()
    //                       };
    //    var data = from d in _context.Set<T>()
    //               where d.Active_Ind == true && d.ID != entity.ID
    //               select d;

    //    foreach (var item in data) {
    //      var keyValues = from pi in item.GetType().GetProperties()
    //                      join k in uniqueAtt.KeyFields on pi.Name equals k
    //                      select new {
    //                        KeyField = k,
    //                        Value = pi.GetValue(item, null).ToString()
    //                      };

    //      var exists = keyValues.SequenceEqual(newkeyValues);
    //      if (exists) {
    //        throw new System.Exception("Duplicated Entry found");
    //      }
    //    }
    //  }
    //}
  }

}
