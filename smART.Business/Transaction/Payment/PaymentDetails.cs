using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using smART.Common;

namespace smART.Business.Rules {

  public class PaymentDetails {

    public void Added(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdatePaymentReceiptAmount(businessEntity, modelEntity, dbContext);    
    }

    public void Modified(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdatePaymentReceiptAmount(businessEntity, modelEntity, dbContext);     
    }

    public void Deleted(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdatePaymentReceiptAmount(businessEntity, modelEntity, dbContext);
    }


    private void UpdatePaymentReceiptAmount(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.PaymentReceipt.Transaction_Type.Equals(EnumTransactionType.Receipt.ToString()))
        UpdateInvoiceAmountReceivedTillDate(businessEntity, modelEntity, dbContext);
      else if (businessEntity.PaymentReceipt.Transaction_Type.Equals(EnumTransactionType.Payment.ToString())) {
        if (businessEntity.PaymentReceipt.Payment_Receipt_Type.Equals(EnumPaymentType.Expenses.ToString()))
          UpdateExpenseAmountPaidTillDate(businessEntity, modelEntity, dbContext);
        else
          UpdateSettlementAmountPaidTillDate(businessEntity, modelEntity, dbContext);
      }
    }

    private void UpdateSettlementAmountPaidTillDate(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.Settlement settlement = (from s in dbContext.T_Settlement
                                             where s.ID == businessEntity.Settlement.ID
                                             select s).FirstOrDefault();
        if (settlement != null) {
          decimal appliedAmount = 0;
          var prDetails = dbContext.T_Payment_Receipt_Details.Where(p => p.Settlement.ID == businessEntity.Settlement.ID && p.Active_Ind == true);
          if (prDetails != null && prDetails.Count() > 0)
            appliedAmount = prDetails.Sum(p => p.Apply_Amount);
          settlement.Amount_Paid_Till_Date = appliedAmount;
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    private void UpdateInvoiceAmountReceivedTillDate(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.Invoice invoice = (from s in dbContext.T_Invoice
                                       where s.ID == businessEntity.Invoice.ID
                                       select s).FirstOrDefault();
        if (invoice != null) {
          decimal appliedAmount = 0;
          var prDetails = dbContext.T_Payment_Receipt_Details.Where(p => p.Invoice.ID == businessEntity.Invoice.ID && p.Active_Ind == true);
          if (prDetails != null && prDetails.Count() > 0)
            appliedAmount = prDetails.Sum(p => p.Apply_Amount);
          invoice.Amount_Paid_Till_Date = appliedAmount;
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    private void UpdateExpenseAmountPaidTillDate(smART.ViewModel.PaymentReceiptDetails businessEntity, smART.Model.PaymentReceiptDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.ExpensesRequest exp = (from s in dbContext.T_Expenses
                                           where s.ID == businessEntity.ExpenseRequest.ID
                                           select s).FirstOrDefault();
        if (exp != null) {
          decimal appliedAmount = 0;
          var prDetails = dbContext.T_Payment_Receipt_Details.Where(p => p.ExpenseRequest.ID == businessEntity.ExpenseRequest.ID && p.Active_Ind == true);
          if (prDetails != null && prDetails.Count() > 0)
            appliedAmount = prDetails.Sum(p => p.Apply_Amount);
          exp.Amount_Paid_Till_Date = Convert.ToDouble(appliedAmount);
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }
    
  }
}
