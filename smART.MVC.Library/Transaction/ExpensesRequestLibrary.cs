using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

namespace smART.Library {

  public class ExpensesRequestLibrary : ExpenseLibrary<VModel.ExpensesRequest, Model.ExpensesRequest, VModel.ExpensesRequest, Model.ExpensesRequest> {
    public ExpensesRequestLibrary() : base() { }
    public ExpensesRequestLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
      Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();
    }

    public override string GetRefrenceTable() { return new Model.ExpensesRequest().GetType().Name; }

    public IEnumerable<VModel.ExpensesRequest> GetUnPaidExpensesWithPaging(
              out int totalRows,
              int page,
              int pageSize,
              string sortColumn,
              string sortType,
              string[] includePredicate = null,
              IList<IFilterDescriptor> filters = null,
              int partyId = 0,
              int bookingId = 0
             ) {
      IEnumerable<Model.ExpensesRequest> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.ExpensesRequest, bool>> exp;
        if (bookingId > 0)
          exp = o => o.Paid_Party_To.ID == partyId
                     && o.Dispatcher_Request_Ref.Booking_Ref_No.ID == bookingId  //o.Dispatcher_Request_Ref != null && o.Dispatcher_Request_Ref.RequestType=="Container" &&
                     && o.Amount_Paid > o.Amount_Paid_Till_Date
                     && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
                     && o.Active_Ind== true && o.Dispatcher_Request_Ref.Active_Ind==true && o.Dispatcher_Request_Ref.Booking_Ref_No.Active_Ind == true ;
        else
          exp = o => o.Paid_Party_To.ID == partyId
               && o.Amount_Paid > o.Amount_Paid_Till_Date
               && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
               && o.Active_Ind==true ;

        modEnumeration = _repository.FindByPaging<Model.ExpensesRequest>(out totalRows, exp,
                                                                          page, pageSize, sortColumn, sortType, includePredicate,
                                                                          filters
                                                                         );
      }
      else
        modEnumeration = _repository.FindByPaging<Model.ExpensesRequest>(out totalRows, o => o.Amount_Paid > o.Amount_Paid_Till_Date
                                                                         && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase),
                                                                         page, pageSize, sortColumn, sortType,
                                                                         includePredicate, filters
                                                                        );
      UpdateScaleRef(modEnumeration);

      IEnumerable<VModel.ExpensesRequest> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.ExpensesRequest> GetUnPaidExpenses(string[] includePredicate = null, int partyId = 0, int bookingId = 0) {
      IEnumerable<Model.ExpensesRequest> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.ExpensesRequest, bool>> exp;
        if (bookingId > 0)
          exp = o => o.Paid_Party_To.ID == partyId
                     && o.Dispatcher_Request_Ref.Booking_Ref_No.ID == bookingId  //o.Dispatcher_Request_Ref != null && o.Dispatcher_Request_Ref.RequestType=="Container" &&
                     && o.Amount_Paid > o.Amount_Paid_Till_Date
                     && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
                     && o.Active_Ind == true && o.Dispatcher_Request_Ref.Active_Ind == true && o.Dispatcher_Request_Ref.Booking_Ref_No.Active_Ind == true;
        else
          exp = o => o.Paid_Party_To.ID == partyId
               && o.Amount_Paid > o.Amount_Paid_Till_Date
               && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
               && o.Active_Ind == true;

        modEnumeration = _repository.Find<Model.ExpensesRequest>(exp, includePredicate);
      }
      else
        modEnumeration = _repository.Find<Model.ExpensesRequest>(o => o.Amount_Paid > o.Amount_Paid_Till_Date
                                                                         && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase),
                                                                 includePredicate
                                                                 );
      UpdateScaleRef(modEnumeration);
      IEnumerable<VModel.ExpensesRequest> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }
   
    public decimal GetTotalDueAmount(int partyId, int bookingId = 0) {
      IEnumerable<Model.ExpensesRequest> modEnumeration;
      if (partyId > 0) {
        Expression<Func<Model.ExpensesRequest, bool>> exp;
        if (bookingId > 0)
          exp = o => o.Paid_Party_To.ID == partyId
                     && o.Dispatcher_Request_Ref.Booking_Ref_No.ID == bookingId
                     && o.Amount_Paid > o.Amount_Paid_Till_Date
                     && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
                     && o.Active_Ind == true && o.Dispatcher_Request_Ref.Active_Ind == true && o.Dispatcher_Request_Ref.Booking_Ref_No.Active_Ind == true;
        else
          exp = o => o.Paid_Party_To.ID == partyId
               && o.Amount_Paid > o.Amount_Paid_Till_Date
               && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase) && o.Active_Ind == true
               && o.Active_Ind == true;


        modEnumeration = _repository.Find<Model.ExpensesRequest>(exp);
      }
      else
        modEnumeration = _repository.Find<Model.ExpensesRequest>(o => o.Amount_Paid > o.Amount_Paid_Till_Date && o.Active_Ind == true
                                                                 && o.Expense_Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
                                                                 );

      double amt = modEnumeration.Sum(s => s.Amount_Paid);
      double amtPaid = modEnumeration.Sum(s => s.Amount_Paid_Till_Date);
      return Convert.ToDecimal(amt - amtPaid);
    }

  }
  public static class LinqExtensions {
    /// <summary>
    /// Used to modify properties of an object returned from a LINQ query
    /// </summary>
    public static TSource Set<TSource>(this TSource input, Action<TSource> updater) {
      updater(input);
      return input;
    }
  }

}
