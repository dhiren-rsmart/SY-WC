using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VModel = smART.ViewModel;
using Telerik.Web.Mvc;

namespace smART.Library
{
    public interface ISalesOrderChildLibrary<TEntityBusiness>
       where TEntityBusiness : class, new()
    {
        IEnumerable<TEntityBusiness> GetAllByPagingBySalesOrderID(out int totalRows, int id, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null);
        IEnumerable<TEntityBusiness> GetAllBySalesOrderID(int salesOrderId, string[] includePredicate = null);
    }
}
