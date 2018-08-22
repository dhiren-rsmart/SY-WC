using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;

using Telerik.Web.Mvc;

namespace smART.Library {

  public class QuickBookLibrary : GenericLibrary<VModel.QBLog, Model.QBLog> {
    public QuickBookLibrary()
      : base() {
    }
    public QuickBookLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.PaymentReceipt, Model.PaymentReceipt>();
      Mapper.CreateMap<Model.PaymentReceipt, VModel.PaymentReceipt>();

    }

    public void AddInvoiceIntoQBLog(int invoiceId,decimal totalAvgCostAmt) {
      // Check duplicate log.
      smART.Model.QBLog parentQBLog = _repository.Find<Model.QBLog>(o => o.Source_ID == invoiceId && (o.Source_Type == "Invoice")).FirstOrDefault();
      if (parentQBLog != null)
        return;

      // Get invoice.  
      string[] predicates = { "Booking.Sales_Order_No.Party", "Sales_Order_No.Party" };
      Model.Invoice entity = _repository.Find<Model.Invoice>(o => o.ID == invoiceId, predicates).FirstOrDefault();

      if (entity != null && entity.Invoice_Status == "Closed") {

        // Get last entry to generate group.
        Model.QBLog latestQBLog = _repository.GetAll<Model.QBLog>().LastOrDefault();
        int group = latestQBLog != null ? latestQBLog.Group + 1 : 1;

        if (entity.Invoice_Type == "Exports") {
          AddExportInvoiceIntoQBLog(entity, group,totalAvgCostAmt);
        }
      }
    }

    private void AddExportInvoiceIntoQBLog(Model.Invoice invoice, int group, decimal totalAvgCostAmt) {
      //===================Sales======================
      //  Add Debit entry.
      smART.Model.QBLog parentQBLog = new Model.QBLog();
      parentQBLog.Source_ID = invoice.ID;
      parentQBLog.Source_Type = "Invoice";
      parentQBLog.Parent_ID = 0;
      parentQBLog.Account_No = "";
      parentQBLog.Account_Name = "Accounts Receivable";
      parentQBLog.Credit_Amt = invoice.Net_Amt;
      parentQBLog.Remarks = string.Format("Invoice#: {0}, Invoice Party: {1}", invoice.ID, invoice.Booking.Sales_Order_No.Party.Party_Name);
      parentQBLog.Parent_ID = 0;
      parentQBLog.Group = group;
      parentQBLog.Name = invoice.Booking.Sales_Order_No.Party.Party_Name;
      parentQBLog.RS_Ref_No = invoice.ID.ToString();
      parentQBLog = AddInQBLog(parentQBLog, invoice);

      //  Add Credit entry.
      smART.Model.QBLog childQBLog = new Model.QBLog();
      childQBLog.Source_ID = invoice.ID;
      childQBLog.Parent_ID = parentQBLog.ID;
      childQBLog.Account_No = "";
      childQBLog.Account_Name = "Sales";
      childQBLog.Debit_Amt = invoice.Net_Amt;
      childQBLog.Remarks = string.Format("Invoice#: {0}, Invoice Party: {1}", invoice.ID, invoice.Booking.Sales_Order_No.Party.Party_Name);
      childQBLog.Group = group;
      childQBLog.Source_Type = "Invoice";
      childQBLog.Name = invoice.Booking.Sales_Order_No.Party.Party_Name;
      childQBLog.RS_Ref_No = invoice.ID.ToString();
      AddInQBLog(childQBLog, invoice);


      //===================Inventory Asset======================

      //  Add Debit entry.
      smART.Model.QBLog parentQBLogAcRec  = new Model.QBLog();
      parentQBLogAcRec.Source_ID = invoice.ID;
      parentQBLogAcRec.Source_Type = "Invoice";
      parentQBLogAcRec.Parent_ID = 0;
      parentQBLogAcRec.Account_No = "";
      parentQBLogAcRec.Account_Name = "Cost of Goods Sold";
      parentQBLogAcRec.Credit_Amt = totalAvgCostAmt;
      parentQBLogAcRec.Remarks = string.Format("Invoice#: {0}, Invoice Party: {1}", invoice.ID, invoice.Booking.Sales_Order_No.Party.Party_Name);
      parentQBLogAcRec.Parent_ID = 0;
      parentQBLogAcRec.Group = group+1;
      parentQBLogAcRec.Name = invoice.Booking.Sales_Order_No.Party.Party_Name;
      parentQBLogAcRec.RS_Ref_No = invoice.ID.ToString();
      parentQBLogAcRec = AddInQBLog(parentQBLogAcRec, invoice);

      //  Add Credit entry.
      smART.Model.QBLog childQBLogAcRec = new Model.QBLog();
      childQBLogAcRec.Source_ID = invoice.ID;
      childQBLogAcRec.Parent_ID = parentQBLogAcRec.ID;
      childQBLogAcRec.Account_No = "";
      childQBLogAcRec.Account_Name = "Inventory Asset";
      childQBLogAcRec.Debit_Amt = totalAvgCostAmt;
      childQBLogAcRec.Remarks = string.Format("Invoice#: {0}, Invoice Party: {1}", invoice.ID, invoice.Booking.Sales_Order_No.Party.Party_Name);
      childQBLogAcRec.Group = group+1;
      childQBLogAcRec.Source_Type = "Invoice";
      childQBLogAcRec.Name = invoice.Booking.Sales_Order_No.Party.Party_Name;
      childQBLogAcRec.RS_Ref_No = invoice.ID.ToString();
      AddInQBLog(childQBLogAcRec, invoice);
    }

    #region Payment/Receipt

    #endregion Payment/Receipt

    public void AddPaymentReceiptIntoQBLog(int paymentId) {
      // Check duplicate log.
      smART.Model.QBLog parentQBLog = _repository.Find<Model.QBLog>(o => o.Source_ID == paymentId && (o.Source_Type == "Payment" || o.Source_Type == "Receipt")).FirstOrDefault();
      if (parentQBLog != null)
        return;

      // Get payment.  
      string[] paymentPredicates = { "Party", "Account_Name" };
      Model.PaymentReceipt entity = _repository.Find<Model.PaymentReceipt>(o => o.ID == paymentId, paymentPredicates).FirstOrDefault();

      if (entity != null && entity.Transaction_Status == "Closed") {
        // Get last entry to generate group.
        Model.QBLog latestQBLog = _repository.GetAll<Model.QBLog>().LastOrDefault();
        int group = latestQBLog != null ? latestQBLog.Group + 1 : 1;

        if (entity.Transaction_Type == "Payment") {
          // For Scale Ticket Payment.
          if (entity.Payment_Receipt_Type == "Tickets")
            AddScalePaymentIntoQBLog(entity, group);
          //// For Expense Payment.
          //else if (entity.Payment_Receipt_Type == "Expenses")
          //  AddExpensePaymentIntoQBLog(entity, group);
        }
        else if (entity.Transaction_Type == "Receipt") {
          AddReceiptIntoQBLog(entity, group);
        }
      }
    }

    private void AddScalePaymentIntoQBLog(Model.PaymentReceipt payment, int group) {
      //  Add Debit entry.
      smART.Model.QBLog parentQBLog = new Model.QBLog();
      parentQBLog.Source_ID = payment.ID;
      parentQBLog.Source_Type = "Payment";
      parentQBLog.Parent_ID = 0;
      parentQBLog.Account_No = "";
      parentQBLog.Account_Name = payment.Transaction_Mode == "Cash" ? "Petty Cash" : "Gold Star Metalex";
      parentQBLog.Credit_Amt = payment.Net_Amt;
      parentQBLog.Remarks = string.Format("Payment#: {0}, Payment Party: {1}", payment.ID, payment.Party.Party_Name);
      parentQBLog.Parent_ID = 0;
      parentQBLog.Group = group;
      parentQBLog.Name = payment.Party.Party_Name;
      parentQBLog.RS_Ref_No = string.IsNullOrEmpty(payment.Check_Wire_Transfer) ? string.Format("P{0}", payment.ID) : payment.Check_Wire_Transfer;
      parentQBLog = AddInQBLog(parentQBLog, payment);

      string[] paymentReceiptPredicates = { "Settlement.Scale", "PaymentReceipt", "ExpenseRequest" };
      IEnumerable<Model.PaymentReceiptDetails> paymentReceipts = _repository.Find<Model.PaymentReceiptDetails>(o => o.PaymentReceipt.ID == payment.ID, paymentReceiptPredicates);

      foreach (var item in paymentReceipts) {
        //  Add Credit entry.
        smART.Model.QBLog childQBLog = new Model.QBLog();
        childQBLog.Source_ID = payment.ID;
        childQBLog.Parent_ID = parentQBLog.ID;
        childQBLog.Account_No = "";
        childQBLog.Account_Name = "Inventory Asset";
        childQBLog.Debit_Amt = item.Apply_Amount;
        childQBLog.Remarks = string.Format("Payment#: {0}, Scale#: {1}, Payment Party: {2}", payment.ID, item.Settlement.Scale.ID, payment.Party.Party_Name);
        childQBLog.Group = group;
        childQBLog.Source_Type = "Payment";
        childQBLog.Name = payment.Party.Party_Name;
        childQBLog.RS_Ref_No = string.IsNullOrEmpty(payment.Check_Wire_Transfer) ? string.Format("P{0}", payment.ID) : payment.Check_Wire_Transfer;
        AddInQBLog(childQBLog, item);
      }
    }

    private void AddExpensePaymentIntoQBLog(Model.PaymentReceipt payment, int group) {
      //  Add Debit entry.
      smART.Model.QBLog parentQBLog = new Model.QBLog();
      parentQBLog.Source_ID = payment.ID;
      parentQBLog.Source_Type = "Payment";
      parentQBLog.Parent_ID = 0;
      parentQBLog.Account_No = "";
      parentQBLog.Account_Name = "Gold Star Metalex";
      parentQBLog.Debit_Amt = payment.Net_Amt;
      parentQBLog.Remarks = string.Format("Payment ID: {0}", payment.ID);
      parentQBLog.Parent_ID = 0;
      parentQBLog.Group = group;
      parentQBLog = AddInQBLog(parentQBLog, payment);

      // Add Credit entry
      smART.Model.QBLog childQBLog = new Model.QBLog();
      childQBLog.Source_ID = payment.ID;
      childQBLog.Parent_ID = parentQBLog.ID;
      childQBLog.Account_No = "";
      childQBLog.Account_Name = "Inventory Asset";
      childQBLog.Credit_Amt = payment.Net_Amt;
      childQBLog.Remarks = string.Format("Payment ID: {0}", payment.ID);
      childQBLog.Group = group;
      childQBLog.Source_Type = "Payment";
      AddInQBLog(childQBLog, payment);
    }

    public void AddReceiptIntoQBLog(Model.PaymentReceipt receipt, int group) {
      //  Add Debit entry.
      smART.Model.QBLog parentQBLog = new Model.QBLog();
      parentQBLog.Source_ID = receipt.ID;
      parentQBLog.Source_Type = "Receipt";
      parentQBLog.Parent_ID = 0;
      parentQBLog.Account_No = "";
      parentQBLog.Account_Name = receipt.Transaction_Mode == "Cash" ? "Petty Cash" : "Gold Star Metalex";
      parentQBLog.Debit_Amt = receipt.Net_Amt;
      parentQBLog.Remarks = string.Format("Receipt#: {0}, Receipt Party: {1}", receipt.ID, receipt.Party.Party_Name);
      parentQBLog.Parent_ID = 0;
      parentQBLog.Group = group;
      parentQBLog.Name = receipt.Party.Party_Name;
      parentQBLog.RS_Ref_No = string.Format("R{0}", receipt.ID);
      parentQBLog = AddInQBLog(parentQBLog, receipt);

      // Get Invoice# and Booking#
      string[] paymentReceiptPredicates = { "Invoice.Booking.Sales_Order_No.Party", "PaymentReceipt" };
      IEnumerable<Model.PaymentReceiptDetails> paymentReceipts = _repository.Find<Model.PaymentReceiptDetails>(o => o.PaymentReceipt.ID == receipt.ID, paymentReceiptPredicates);
      string strBooking = string.Empty;
      string strInvoice = string.Empty;
      foreach (var item in paymentReceipts) {
        if (string.IsNullOrEmpty(strBooking))
          strBooking += item.Invoice.Booking.Booking_Ref_No;
        else
          strBooking += ", " + item.Invoice.Booking.Booking_Ref_No;

        if (string.IsNullOrEmpty(strInvoice))
          strInvoice += item.Invoice.ID;
        else
          strInvoice += ", " + item.Invoice.ID;
      }

      //  Add Credit entry.
      smART.Model.QBLog childQBLog = new Model.QBLog();
      childQBLog.Source_ID = receipt.ID;
      childQBLog.Parent_ID = parentQBLog.ID;
      childQBLog.Account_No = "";
      childQBLog.Account_Name = "Accounts Receivable";
      childQBLog.Credit_Amt = receipt.Net_Amt;
      childQBLog.Remarks = string.Format("Receipt#: {0}, Invoice#: {1}, Booking#: {2}, Receipt Party: {3}", receipt.ID, strInvoice, strBooking, receipt.Party.Party_Name);
      childQBLog.Group = group;
      childQBLog.Source_Type = "Receipt";
      childQBLog.Name = receipt.Party.Party_Name;
      childQBLog.RS_Ref_No = string.Format("R{0}", receipt.ID);
      AddInQBLog(childQBLog, receipt);
    }

    //public override VModel.PaymentReceipt Add(VModel.PaymentReceipt addObject) {
    //  VModel.PaymentReceipt insertedObjectBusiness = addObject;
    //  try {
    //    Model.PaymentReceipt newModObject = Mapper.Map<VModel.PaymentReceipt, Model.PaymentReceipt>(addObject);

    //    newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party.ID);
    //    newModObject.Account_Name = _repository.GetQuery<Model.Bank>().SingleOrDefault(o => o.ID == addObject.Account_Name.ID);

    //    if (addObject.Booking != null)
    //      newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == addObject.Booking.ID);

    //    if (addObject.Party_Address != null)
    //      newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == addObject.Party_Address.ID);

    //    if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {

    //      Model.PaymentReceipt insertedObject = _repository.Add<Model.PaymentReceipt>(newModObject);
    //      _repository.SaveChanges();

    //      insertedObjectBusiness = Mapper.Map<Model.PaymentReceipt, VModel.PaymentReceipt>(insertedObject);
    //      Added(insertedObjectBusiness, newModObject, _dbContext);

    //    }
    //  }
    //  catch (Exception ex) {
    //    bool rethrow;
    //    rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
    //    if (rethrow)
    //      throw ex;
    //  }
    //  return insertedObjectBusiness;
    //}

    //protected override void Modify(Expression<Func<Model.PaymentReceipt, bool>> predicate, VModel.PaymentReceipt modObject, string[] includePredicate = null) {
    //  try {
    //    Model.PaymentReceipt newModObject = Mapper.Map<VModel.PaymentReceipt, Model.PaymentReceipt>(modObject);

    //    newModObject.Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party.ID);
    //    newModObject.Account_Name = _repository.GetQuery<Model.Bank>().SingleOrDefault(o => o.ID == modObject.Account_Name.ID);

    //    if (modObject.Booking != null)
    //      newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);

    //    if (modObject.Party_Address!= null)
    //      newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == modObject.Party_Address.ID);

    //    if (Modifying(modObject, newModObject, _dbContext)) {
    //      _repository.Modify<Model.PaymentReceipt>(predicate, newModObject, includePredicate);
    //      _repository.SaveChanges();
    //      Modified(modObject, newModObject, _dbContext);
    //    }
    //  }
    //  catch (Exception ex) {
    //    bool rethrow;
    //    rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
    //    if (rethrow)
    //      throw ex;
    //  }
    //}

    //public IEnumerable<VModel.PaymentReceipt> GetReceiptsByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
    //  string trasnType = EnumTransactionType.Receipt.ToString();
    //  IEnumerable<Model.PaymentReceipt> modEnumeration = _repository.FindByPaging<Model.PaymentReceipt>(out totalRows, o => o.Transaction_Type == trasnType,
    //                                                                                  page, pageSize, sortColumn, sortType, includePredicate, filters);
    //  IEnumerable<VModel.PaymentReceipt> busEnumeration = Map(modEnumeration);
    //  return busEnumeration;
    //}

    //public IEnumerable<VModel.PaymentReceipt> GetPaymentsByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
    //  string trasnType = EnumTransactionType.Payment.ToString();
    //  IEnumerable<Model.PaymentReceipt> modEnumeration = _repository.FindByPaging<Model.PaymentReceipt>(out totalRows, o => o.Transaction_Type == trasnType,
    //                                                                                  page, pageSize, sortColumn, sortType, includePredicate, filters);
    //  IEnumerable<VModel.PaymentReceipt> busEnumeration = Map(modEnumeration);

    //  return busEnumeration;

    //private void AddInQBLog(smART.ViewModel.PaymentReceipt businessEntity, smART.Model.PaymentReceipt modelEntity, smART.Model.smARTDBContext dbContext) {
    //  try {

    //    if (businessEntity.Active_Ind == true && businessEntity.Transaction_Status == "Closed") {
    //      // Add in QBLog  
    //      smART.Model.QBLog qbLog = new Model.QBLog();
    //      qbLog.Source_ID = businessEntity.ID;
    //      qbLog.Parent_ID = businessEntity.ID;
    //      qbLog.Account_No = "";
    //      qbLog.Account_Name = "Gold Star Metalex";
    //      qbLog.Created_By = businessEntity.Created_By;
    //      qbLog.Created_Date = businessEntity.Created_Date;
    //      qbLog.Last_Updated_Date = businessEntity.Last_Updated_Date;
    //      qbLog.Updated_By = businessEntity.Updated_By;
    //      qbLog.Debit_Amt = businessEntity.Applied_Amount;
    //      qbLog.Remarks = string.Format("Payment ID: {0}", businessEntity.ID);
    //      qbLog = dbContext.T_QB_Log.Add(qbLog);
    //      dbContext.SaveChanges();
    //      // Update QBstatus
    //    }
    //  }
    //  catch (Exception ex) {
    //    bool rethrow;
    //    rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
    //    if (rethrow)
    //      throw ex;
    //  }
    //}

    private Model.QBLog AddInQBLog(smART.Model.QBLog newModObject, smART.Model.BaseEntity targetEntity) {
      // Add in QBLog     
      newModObject.Created_By = targetEntity.Created_By;
      newModObject.Created_Date = DateTime.Now;
      newModObject.Last_Updated_Date = DateTime.Now;
      newModObject.Updated_By = targetEntity.Updated_By;
      newModObject.Status = smART.Common.EnumPostingStatus.Pending.ToString();
      _repository.Add<Model.QBLog>(newModObject);
      _repository.SaveChanges();
      return newModObject;
    }

    protected override void Modify(Expression<Func<Model.QBLog, bool>> predicate, VModel.QBLog modObject, string[] includePredicate = null) {
      try {
        Model.QBLog newModObject = Mapper.Map<VModel.QBLog, Model.QBLog>(modObject);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.QBLog>(predicate, newModObject, includePredicate);
          _repository.SaveChanges();
          Modified(modObject, newModObject, _dbContext);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    public IEnumerable<VModel.QBLog> GetPendingParentQBLogsByStatusWithPagging(string status, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.QBLog> modEnumeration = _repository.FindByPaging<Model.QBLog>(out totalRows,
                                                                                      o => string.IsNullOrEmpty(o.QB_Ref_No)
                                                                                      && o.Parent_ID == 0
                                                                                      && o.Status == status
                                                                                      && o.Active_Ind == true,
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters
                                                                                     );

      IEnumerable<VModel.QBLog> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.QBLog> GetUnPostedParentQBLogs() {
      IEnumerable<Model.QBLog> modQBLogs = _repository.Find<Model.QBLog>(o => string.IsNullOrEmpty(o.QB_Ref_No)
                                                                          && o.Parent_ID == 0
                                                                          && o.Status == "Pending"
                                                                          && o.Active_Ind == true
                                                                         );
      IEnumerable<VModel.QBLog> busQBLogs = Map(modQBLogs);
      return busQBLogs;
    }

    public IEnumerable<VModel.QBLog> GetByParentID(int parentID) {
      IEnumerable<Model.QBLog> modQBLogs = _repository.Find<Model.QBLog>(o => o.Parent_ID == parentID && o.Active_Ind == true);
      IEnumerable<VModel.QBLog> busQBLogs = Map(modQBLogs);
      return busQBLogs;
    }

    public IEnumerable<VModel.QBLog> GetByPrentIDWithPagging(int parentID, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.QBLog> modEnumeration = _repository.FindByPaging<Model.QBLog>(out totalRows,
                                                                                      o => o.Parent_ID == parentID
                                                                                       && o.Active_Ind == true,
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters
                                                                                     );

      IEnumerable<VModel.QBLog> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }
  }
}
