using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class Payment {
    
    public void GotSingle(smART.ViewModel.PaymentReceipt businessEntity, smART.Model.PaymentReceipt modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        IEnumerable<smART.Model.PaymentReceiptDetails> modelPaymentDetails = from a in dbContext.T_Payment_Receipt_Details
                                                                             where a.PaymentReceipt.ID == modelEntity.ID
                                                                             select a;
        decimal detailsAppliedAmtTotal = (from a in modelPaymentDetails
                                          select a.Apply_Amount).Sum();
        decimal detailsBalanceAmountTotal = (from a in modelPaymentDetails
                                             select a.Balance_Amount).Sum();
        businessEntity.Applied_Amount = businessEntity.Total_Amount_Paid - detailsAppliedAmtTotal;
        businessEntity.Total_Amount_Due = detailsBalanceAmountTotal;
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
