using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace smART.Business.Rules {

  public class Settlement {

    public void Added(smART.ViewModel.Settlement businessEntity, smART.Model.Settlement modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdateScale(businessEntity, modelEntity, dbContext);
    }

    public void Deleted(smART.ViewModel.Settlement businessEntity, smART.Model.Settlement modelEntity, smART.Model.smARTDBContext dbContext) {
      DeleteSettlementScale(businessEntity, modelEntity, dbContext);
    }

    public void DeleteSettlementScale(smART.ViewModel.Settlement businessEntity, smART.Model.Settlement modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.Scale scale = dbContext.T_Scale.FirstOrDefault(i => i.ID == modelEntity.Scale.ID);
        if (scale != null) {
          scale.Updated_By = modelEntity.Updated_By;
          scale.Last_Updated_Date = modelEntity.Last_Updated_Date;
          scale.Ticket_Settled = false;
          scale.Settlement_Diff_NetWeight = 0;
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

    public void UpdateScale(smART.ViewModel.Settlement businessEntity, smART.Model.Settlement modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.Scale scale = dbContext.T_Scale.FirstOrDefault(i => i.ID == modelEntity.Scale.ID);
        if (scale != null) {
          scale.Updated_By = modelEntity.Updated_By;
          scale.Last_Updated_Date = modelEntity.Last_Updated_Date;
          scale.Ticket_Settled = true;
          scale.Settlement_Diff_NetWeight = businessEntity.Actual_Net_Weight - scale.Net_Weight;
          scale.Net_Weight = businessEntity.Actual_Net_Weight;
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
