using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Telerik.Web.Mvc;

namespace smART.Library
{
    public interface ILibrary<TEntityBusiness>
       where TEntityBusiness : class, new()
    {
        void Initialize(string dbContextConnectionString);

        TEntityBusiness GetByID(string id, string[] includePredicate = null);
        IEnumerable<TEntityBusiness> GetAll(string[] includePredicate = null);
        IEnumerable<TEntityBusiness> GetAllByPaging(
            out int totalRows, 
            int page, 
            int pageSize, 
            string sortColumn,
            string sortType, 
            string[] includeEntities = null, 
            IList<IFilterDescriptor> filters = null);

        IEnumerable<TEntityBusiness> GetAllByPaging(
                  IEnumerable<TEntityBusiness> newBusObjects,
                  out int totalRows,
                  int page,
                  int pageSize,
                  string sortColumn,
                  string sortType,
                  string[] includeEntities = null,
                  IList<IFilterDescriptor> filters = null);
        TEntityBusiness Add(TEntityBusiness addObject);
        void Delete(string id,string[] includePredicate = null);
        TEntityBusiness Modify(TEntityBusiness modObject, string[] includePredicate = null);
    }
}
