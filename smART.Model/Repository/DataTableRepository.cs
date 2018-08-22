using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Telerik.Web.Mvc;

namespace smART.Model {

  public class DataTableRepository : ITableRepository {

    private readonly ConnectionStringSettings connConfiguration;
    private readonly DbProviderFactory providerFactory;

    public DataTableRepository(string dbContextConnectionString) {
      connConfiguration = ConfigurationManager.ConnectionStrings["smARTDBContext"];
      providerFactory = DbProviderFactories.GetFactory(connConfiguration.ProviderName);
    }

    private DataTable FetchData(string commandText) {
      try {
        using (var conn = providerFactory.CreateConnection()) {
          conn.ConnectionString = connConfiguration.ConnectionString;
          using (var command = conn.CreateCommand()) {
            command.CommandText = commandText;
            var result = new DataTable();
            conn.Open();
            result.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
            return result;
          }
        }
      }
      catch (Exception ex) {
        return new DataTable();
      }
    }

    public DataTable GetAllAsDt(string tableName, string orderByClause) {
      if (string.IsNullOrEmpty(tableName))
        return new DataTable();
      string sql = "Select * from " + tableName;
      sql += !string.IsNullOrEmpty(orderByClause) ? string.Format(" Order by {0}", orderByClause) : "";
      return FetchData(sql);
    }

    public DataTable GetAllByPagingAsDt(
           string tableName,
           out int totalRows,
           int page,
           int pageSize,
           string sortColumn,
           string sortType,
           IList<IFilterDescriptor> filters = null) {
      totalRows = 0;
      try {

        string sql = "Select * from" +
                      "(" +
                        " SELECT ROW_NUMBER() OVER (ORDER BY [t0]." + (string.IsNullOrEmpty(sortColumn) ? "ID" : sortColumn) + " ) AS [ROW_NUMBER]" +
                        " , * from " + tableName + " As [t0]" +
                       " ) AS [t1]" +
                      " WHERE [t1].[ROW_NUMBER] BETWEEN " + ((page - 1) * pageSize + 1) + " AND " + (page * pageSize);


        // Apply sorting     
        if (!string.IsNullOrEmpty(sortColumn)) {
          if (sortType.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            sql += " order by " + sortColumn + " desc";
          else
            sql += "order by " + sortColumn + " asc";
        }

        //if (filters != null)
        //  result = FilterExpression(result, filters);


        // Apply pagging
        totalRows = (int)FetchData("Select Count(*) as Count from " + tableName).Rows[0]["Count"];

        return FetchData(sql);
      }

      catch (Exception ex) {
        bool rethrow;
        rethrow = DataExceptionHandler.HandleException(ref ex);
        if (rethrow)
          throw ex;
        return null;
      }
    }
  }

}
