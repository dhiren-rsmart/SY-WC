using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq.Expressions;
using Telerik.Web.Mvc;

namespace smART.Model
{
    public interface IRepository : IDisposable
    {
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;

        IQueryable<TEntity> GetQueryActiveItems<TEntity>() where TEntity : BaseEntity;

        IEnumerable<TEntity> GetAll<TEntity>(string[] includePredicate = null) where TEntity : BaseEntity;
        IEnumerable<TEntity> GetAllByPaging<TEntity>(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity;
        IEnumerable<TEntity> GetAllByPaging<TEntity>(IQueryable<TEntity> result,out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity;

        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate, string[] includePredicate = null) where TEntity : BaseEntity;
        IEnumerable<TEntity> FindByPaging<TEntity>(out int totalRows, Expression<Func<TEntity, bool>> predicate, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity;
        IEnumerable<TEntity> FindByPaging<TEntity>(IQueryable<TEntity> query,out int totalRows, Expression<Func<TEntity, bool>> predicate, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) where TEntity : BaseEntity;

        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
        TEntity Last<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
        
        TEntity Add<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void Modify<TEntity>(Expression<Func<TEntity, bool>> predicate, TEntity entity, string[] includePredicate = null,bool active_Ind=true) where TEntity : BaseEntity;
        void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, bool hardDelete = false) where TEntity : BaseEntity;
  
        void SaveChanges();
    }
}
