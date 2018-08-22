using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Diagnostics;
using smART.Common;
using System.Linq.Expressions;
using smART.Model;
using System.Configuration;
using System.Data.Entity;

namespace smART.Test {
  public class Class1 {
    static void Main(string[] args) {

        string dobString = "May 23 1988 12:00AM";
        DateTime dobDt;
        smART.Common.DateTimeHelper.IsValidDate(dobString, out dobDt);
        

        //try
        //{
        //    Exception ex = new DataException("ABC");
        //    HandleException(ref  ex);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    Console.ReadLine();
        //}
      //StringBuilder sb = new StringBuilder();

      //List<Item> Customers = new List<Item>();
      //Customers.Add(new Item() {
      //  ID = 1, Short_Name = "sanjeev", Item_Category = "A",Created_Date = DateTime .Now
      //});

      //Customers.Add(new Item() {
      //  ID = 2, Short_Name = "nitin", Item_Category = "b", Created_Date = DateTime.Now
      //});

      //Customers.Add(new Item() {
      //  ID = 3, Short_Name = "manish", Item_Category = "b", Created_Date = DateTime.Now
      //});

      //Customers.Add(new Item() {
      //  ID = 4, Short_Name = "bhopu", Item_Category = "d", Created_Date = DateTime.Now
      //});

      //sb.AppendFormat("{0,-8}{1,-20}{2,-15}{3,-10:yyyyMMdd}{4}",
      //                  "ID".PadLeft(6, '0'),
      //                  "Short_Name",
      //                  "Item_Category",
      //                  "Created Date",
      //                  Environment.NewLine);

      //sb.AppendFormat("====================================================");
      //sb.AppendFormat(Environment.NewLine);
      //foreach (Item c in Customers) {
      //  sb.AppendFormat("{0,-8}{1,-20}{2,-15}{3,-10:yyyyMMdd}{4}",
      //                  c.ID.ToString().PadLeft(6, '0'),
      //                  c.Short_Name,
      //                  c.Item_Category,        
      //                  c.Created_Date,
      //                  Environment.NewLine);
      //}
      //  sb.AppendFormat("====================================================");


        // Sanjeev
        string strConnectionString = ConfigurationManager.ConnectionStrings["smARTDBContext"].ConnectionString;

        Database.SetInitializer<smARTDBContext>(new smARTDbContextInitializer());
        smARTDBContext dbContext = new Model.smARTDBContext(strConnectionString);

        LOVType lovType = dbContext.M_LOV.SingleOrDefault(o => o.LOVType_Name == "PARTY_TYPE");
        //IQueryable<LOVType> lovTypes = dbContext.M_LOV.Where(Lambda("LOVType_Name", "Party"));
    }
    /// <summary>
    /// This method is used to handle all data module exception.
    /// </summary>
    /// <param name="ex">Exception.</param> 
    /// <param name="severity">Severity.</param>
    /// <param name="message">Message.It is an optional parameter.</param>
    /// <returns></returns>
    public static bool HandleException(ref System.Exception ex, TraceEventType severity = TraceEventType.Error, string message = "")
    {
        return ExceptionHandler.HandleException(ref ex, severity, message, Constants.PresentPolicyKey);
    }

  }
}
