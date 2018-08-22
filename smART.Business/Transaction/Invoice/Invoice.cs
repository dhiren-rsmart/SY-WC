using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class Invoice {

    public void GotMultiple(IEnumerable<smART.ViewModel.Invoice> businessEntities, IEnumerable<smART.Model.Invoice> modelEntities, smART.Model.smARTDBContext dbContext) {
      try {
        foreach (smART.ViewModel.Invoice i in businessEntities) {
          if (i.Invoice_Type.Equals("local sales", StringComparison.InvariantCultureIgnoreCase)) {
            i.Booking = new ViewModel.Booking() { ID = 0, Sales_Order_No = i.Sales_Order_No };
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntities.FirstOrDefault().Updated_By, modelEntities.FirstOrDefault().GetType().Name, "0");
        if (rethrow)
          throw ex;
      }
    }

    public void Added(smART.ViewModel.Invoice businessEntity, smART.Model.Invoice modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        if (businessEntity.Invoice_Type.Equals("local sales", StringComparison.InvariantCultureIgnoreCase)) {
        
          IEnumerable<Model.Scale> scales = dbContext.T_Scale.Where(o => o.Sales_Order.ID == businessEntity.Sales_Order_No.ID && o.Invoice.ID == null);
          foreach (var scale in scales) {
            scale.Invoice = modelEntity;
          }
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, modelEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    public void Deleted(smART.ViewModel.Invoice businessEntity, smART.Model.Invoice modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        if (businessEntity.Invoice_Type.Equals("local sales", StringComparison.InvariantCultureIgnoreCase)) {

          IEnumerable<Model.Scale> scales = dbContext.T_Scale.Where(o => o.Invoice.ID == businessEntity.ID);
          foreach (var scale in scales) {
            scale.Invoice = null ;
          }
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, modelEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }


    public void Modified(smART.ViewModel.Invoice businessEntity, smART.Model.Invoice modelEntity, smART.Model.smARTDBContext dbContext) {

    }

  }
}
