// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/04/2011

using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.MVC.Present.Helpers;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using smART.Common;
using System;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_PaymentExpense)]
  public class PaymentExpenseController : ExpenseGridController<PaymentExpenseLibrary, ExpensesRequest, PaymentReceipt> {
    #region Constructor

    public PaymentExpenseController() : base("PaymentExpense", null) { }

    #endregion Constructor

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<ExpensesRequest> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);
      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        resultList = lib.GetAllByPagingByPaymentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Paid_Party_To", "Payment" });
      }
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    protected override void ValidateEntity(ExpensesRequest entity) {
      base.ValidateEntity(entity);
      if (entity.Reference_Table == new Model.Scale().GetType().Name || entity.Reference_Table == new Model.DispatcherRequest().GetType().Name) {
        ModelState.AddModelError("ScaleDispatcherModify", "Can not modify.");
      }

    }

    [HttpPost]
    public void AddUnPaidPurchaseExpenses(string scaleId) {
      int refId = Convert.ToInt32(scaleId);
      if (refId > 0)
        AddUnPaidPurchaseScaleExpenses(refId);
    }

    [HttpPost]
    public void RemoveUnPaidPurchaseExpenses(string scaleId) {
      // If TEmpEntityList has no Items.
      if (TempEntityList != null) {
        int intScaleId = Convert.ToInt32(scaleId);
        if (intScaleId > 0) {
          // Get all scale exepenses.        
          IEnumerable<ExpensesRequest> scaleExps = from exp in TempEntityList
                                                   where exp.Reference_Table == "Scale" && exp.Reference_ID == intScaleId
                                                   select exp;
          if (scaleExps != null) {
            IList<ExpensesRequest> removeExpenses = new List<ExpensesRequest>(); 
            ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            foreach (var item in scaleExps) {
              // Get scale entity.
              Scale scale = scaleLib.GetByID(item.Reference_ID.ToString(), new string[] { "Dispatch_Request_No" });
              if (scale != null && scale.Dispatch_Request_No != null && scale.Dispatch_Request_No.ID > 0) {
                // Get dispatcher id.
                int dispatcherId = scale.Dispatch_Request_No.ID;
                // Get all dispatcher expenses. 
                IEnumerable<ExpensesRequest> dispatcherExps = from exp in TempEntityList
                                                              where exp.Reference_Table == "DispatcherRequest" && exp.Reference_ID == dispatcherId
                                                              select exp;
                if (dispatcherExps != null) {
                  foreach (var dispExp in dispatcherExps) {
                    // Add dispatcher expense in removeExpense list.
                    removeExpenses.Add(dispExp);
                  }
                }

              }
              // Add scale expense in removeExpense list.
              removeExpenses.Add(item);
            }
            // Delete scale and dispatcher expense.
            foreach (var removeExp in removeExpenses) {
              TempEntityList.Remove(removeExp);
            }
          }
        }
      }
    }

    public void AddUnPaidPurchaseScaleExpenses(int scaleId) {
      // Search for all expenses that matches scaleId.
      ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      string refTableName = new Model.Scale().GetType().Name;
      IEnumerable<ExpensesRequest> expenses = lib.GetAllPurchasingExepneseByRefTableAndRefId(scaleId, refTableName, new[] { "Payment", "Paid_Party_To" });
      ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      foreach (var item in expenses) {
        // If scale expense already exists.
        if (TempEntityList.FirstOrDefault(m => m.ID == item.ID) == null) {
          // Get scale entity    
          Scale scale = scaleLib.GetByID(item.Reference_ID.ToString(), new string[] { "Dispatch_Request_No" });
          if (item.Paid_Party_To == null)
            item.Paid_Party_To = new Party();
          // Add eexpense to TempList.
          TempEntityList.Add(item);
          // Add dispatcher expense.
          if (scale.Dispatch_Request_No != null && scale.Dispatch_Request_No.ID > 0)
            AddUnPaidPurchaseDispatcherExpenses(scale.Dispatch_Request_No.ID);
        }
      }
    }

    private void AddUnPaidPurchaseDispatcherExpenses(int dispathcerId) {
      // Search for expenses that matches dispatcher id.
      ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      string refTableName = new Model.DispatcherRequest().GetType().Name;
      IEnumerable<ExpensesRequest> expenses = lib.GetAllPurchasingExepneseByRefTableAndRefId(dispathcerId, refTableName, new[] { "Payment", "Paid_Party_To" });
      foreach (var item in expenses) {
        // If expense already exists.
        if (TempEntityList.FirstOrDefault(m => m.ID == item.ID) == null) {
          // If party is null.
          if (item.Paid_Party_To == null)
            item.Paid_Party_To = new Party();

          // Add to TempList.
          TempEntityList.Add(item);
        }
      }
    }

  }
}