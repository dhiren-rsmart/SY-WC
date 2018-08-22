using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;
using System.Linq.Expressions;


namespace smART.Business.Rules {

  public class SettlementDetails : BaseScale {

    public void Added(smART.ViewModel.SettlementDetails businessEntity, smART.Model.SettlementDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdateScaleDetails(businessEntity, modelEntity, dbContext);
    }

    public void Deleted(smART.ViewModel.SettlementDetails businessEntity, smART.Model.SettlementDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      DeleteScaleDetails(businessEntity, modelEntity, dbContext);
    }

    public void UpdateScaleDetails(smART.ViewModel.SettlementDetails businessEntity, smART.Model.SettlementDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      try {

        smART.Model.ScaleDetails scaleDetails = dbContext.T_Scale_Details.Include("Scale").Include("Apply_To_Item").FirstOrDefault(i => i.ID == modelEntity.Scale_Details_ID.ID);

        if (scaleDetails != null) {

          scaleDetails.Old_Net_Weight = scaleDetails.NetWeight;
          scaleDetails.Updated_By = modelEntity.Updated_By;
          scaleDetails.Last_Updated_Date = modelEntity.Last_Updated_Date;
          scaleDetails.Settlement_Diff_NetWeight = businessEntity.Actual_Net_Weight - scaleDetails.NetWeight;
          scaleDetails.NetWeight = businessEntity.Actual_Net_Weight;
          scaleDetails.Apply_To_Item = dbContext.M_Item.FirstOrDefault(i => i.ID == scaleDetails.Apply_To_Item.ID);
          scaleDetails.Scale = dbContext.T_Scale.FirstOrDefault(i => i.ID == scaleDetails.Scale.ID);
          if (scaleDetails.Settlement_Diff_NetWeight != 0) {
            AddInventory(scaleDetails, dbContext);
          }

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

    public void DeleteScaleDetails(smART.ViewModel.SettlementDetails businessEntity, smART.Model.SettlementDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        smART.Model.ScaleDetails scaleDetails = dbContext.T_Scale_Details.FirstOrDefault(i => i.ID == modelEntity.Scale_Details_ID.ID);
        if (scaleDetails != null) {

          if (scaleDetails.Settlement_Diff_NetWeight != 0) {
            AddInventory(scaleDetails, dbContext);
          }
          scaleDetails.NetWeight = scaleDetails.NetWeight - scaleDetails.Settlement_Diff_NetWeight;
          scaleDetails.Updated_By = modelEntity.Updated_By;
          scaleDetails.Last_Updated_Date = modelEntity.Last_Updated_Date;
          scaleDetails.Settlement_Diff_NetWeight = 0;
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
