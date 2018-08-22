using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telerik.Web.Mvc;

namespace smART.Model {

  public interface ITableRepository {

    DataTable GetAllAsDt(string tableName, string orderByClause);

    DataTable GetAllByPagingAsDt(
               string tableName,
               out int totalRows,
               int page,
               int pageSize,
               string sortColumn,
               string sortType,
               IList<IFilterDescriptor> filters = null);
  }

}
