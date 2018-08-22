// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/04/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Transaction_InvoiceExpense)]
    public class InvoiceExpenseController : ExpenseGridController<InvoiceExpenseLibrary, ExpensesRequest, Invoice>
    {
        #region Constructor

        public InvoiceExpenseController() : base("InvoiceExpense", null) { }

        #endregion Constructor

        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<ExpensesRequest> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);
            if (isNew || id == "0")
            {
                resultList = TempEntityList;
                totalRows = TempEntityList.Count;
            }
            else
            {
                ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                resultList = lib.GetAllByPagingByInvoiceID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Paid_Party_To", "Payment", "Invoice" });
            }
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override void ValidateEntity(ExpensesRequest entity)
        {
            base.ValidateEntity(entity);
            if (entity.Reference_Table == new Model.Scale().GetType().Name || entity.Reference_Table == new Model.DispatcherRequest().GetType().Name)
            {
                ModelState.AddModelError("ScaleDispatcherModify", "Can not modify.");
            }

        }

        [HttpPost]
        public void AddNonInvoiceSellingExpenses(string bookingId)
        {
            int refId = Convert.ToInt32(bookingId);
            if (refId > 0)
                AddNonInvoiceSellingScaleExpenses(refId);
        }

        //[HttpPost]
        //public void RemoveUnPaidSellingExpenses(string bookingId) {
        //  // If TEmpEntityList has no Items.
        //  if (TempEntityList != null) {
        //    int intBookingId = Convert.ToInt32(bookingId);
        //    if (intBookingId > 0) {
        //      // Get all scale exepenses.        
        //      IEnumerable<ExpensesRequest> scaleExps = from exp in TempEntityList
        //                                               where exp.Reference_Table == "Scale" && exp.Reference_ID == intBookingId
        //                                               select exp;
        //      if (scaleExps != null) {
        //        IList<ExpensesRequest> removeExpenses = new List<ExpensesRequest>();
        //        ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        //        foreach (var item in scaleExps) {
        //          // Get scale entity.
        //          Scale scale = scaleLib.GetByID(item.Reference_ID.ToString(), new string[] { "Dispatch_Request_No" });
        //          if (scale != null && scale.Dispatch_Request_No != null && scale.Dispatch_Request_No.ID > 0) {
        //            // Get dispatcher id.
        //            int dispatcherId = scale.Dispatch_Request_No.ID;
        //            // Get all dispatcher expenses. 
        //            IEnumerable<ExpensesRequest> dispatcherExps = from exp in TempEntityList
        //                                                          where exp.Reference_Table == "DispatcherRequest" && exp.Reference_ID == dispatcherId
        //                                                          select exp;
        //            if (dispatcherExps != null) {
        //              foreach (var dispExp in dispatcherExps) {
        //                // Add dispatcher expense in removeExpense list.
        //                removeExpenses.Add(dispExp);
        //              }
        //            }

        //          }
        //          // Add scale expense in removeExpense list.
        //          removeExpenses.Add(item);
        //        }
        //        // Delete scale and dispatcher expense.
        //        foreach (var removeExp in removeExpenses) {
        //          TempEntityList.Remove(removeExp);
        //        }
        //      }
        //    }
        //  }
        //}

        public void AddNonInvoiceSellingScaleExpenses(int bookingId)
        {
            TempEntityList.Clear();
            ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Scale> scales = scaleLib.GetScalesByBookingId(bookingId);
            // Search for all expenses that matches scaleId.
            ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            string refTableName = new Model.Scale().GetType().Name;
            foreach (var scale in scales)
            {
                IEnumerable<ExpensesRequest> expenses = lib.GetAllSellingExepneseByRefTableAndRefId(scale.ID, refTableName, new[] { "Payment", "Paid_Party_To", "Invoice" });
                foreach (var expense in expenses)
                {
                    // If scale expense already exists.
                    if (TempEntityList.FirstOrDefault(m => m.ID == expense.ID) == null)
                    {
                        if (expense.Paid_Party_To == null)
                            expense.Paid_Party_To = new Party();
                        // Add eexpense to TempList.
                        TempEntityList.Add(expense);
                        // Add dispatcher expense.            

                    }
                }
                AddNonInvoiceSellingDispatcherExpenses(bookingId, scale.ContainerNo);
            }

        }

        private void AddNonInvoiceSellingDispatcherExpenses(int bookingId, string containerNo)
        {
            // Search for expenses that matches dispatcher id.
            DispatcherRequestLibrary dispathcerRequestLib = new DispatcherRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<DispatcherRequest> dispatcherReqs = dispathcerRequestLib.GetDispatcherByBookingAndContainerNo(bookingId, containerNo, new string[] { "Booking_Ref_No","Container" });
            foreach (DispatcherRequest dispatcherReq in dispatcherReqs)
            {
                ExpensesRequestLibrary lib = new ExpensesRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                string refTableName = new Model.DispatcherRequest().GetType().Name;
                IEnumerable<ExpensesRequest> expenses = lib.GetAllPurchasingExepneseByRefTableAndRefId(dispatcherReq.ID, refTableName, new[] { "Payment", "Paid_Party_To", "Invoice" });
                foreach (var item in expenses)
                {
                    // If expense already exists.
                    if (TempEntityList.FirstOrDefault(m => m.ID == item.ID) == null)
                    {
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
}