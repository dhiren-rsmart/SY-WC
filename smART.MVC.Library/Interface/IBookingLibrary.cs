using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VModel = smART.ViewModel;
using Telerik.Web.Mvc;

namespace smART.Library
{
    public interface IBookingLibrary<TEntityBusiness>:ILibrary<TEntityBusiness>
       where TEntityBusiness : class, new() 
    {
        IEnumerable<TEntityBusiness> InvoicePendingBookings(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null);
    }
}
