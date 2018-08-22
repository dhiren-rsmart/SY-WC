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

  public class ScaleDetails : BaseScale {

    public void Added(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdateWeight(businessEntity, modelEntity, dbContext);
    }

    public void Modified(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext) {
      UpdateWeight(businessEntity, modelEntity, dbContext);
    }

    public void Deleting(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateWeight(businessEntity, modelEntity, dbContext,true);
      cancel = false;
    }

    //public void Added(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext) {
    //  //if (modelEntity.Scale != null && modelEntity.Scale.Ticket_Status.ToLower().Equals("close") && modelEntity.Apply_To_Item != null)
    //  //{
    //  //    AddInventory(modelEntity, dbContext);
    //  //    dbContext.SaveChanges();
    //  //}
    //}

    //public void Modified(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext) {
    //  //if (modelEntity.Scale != null && modelEntity.Scale.Ticket_Status.ToLower().Equals("close") && modelEntity.Apply_To_Item != null)
    //  //{
    //  //    AddInventory(modelEntity, dbContext);
    //  //    dbContext.SaveChanges();
    //  //}
    //}

    //public void Deleted(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext) {
    //  //IQueryable<smART.Model.ScaleDetails> scaleDetails = dbContext.Set<smART.Model.ScaleDetails>().AsQueryable().Where(o => o.ID == modelEntity.ID);

    //  //if (scaleDetails != null && scaleDetails.Count()>0)
    //  //{
    //  //    scaleDetails = scaleDetails.Include("Apply_To_Item");
    //  //    scaleDetails = scaleDetails.Include("Scale");

    //  //    smART.Model.ScaleDetails scaleDetail = scaleDetails.FirstOrDefault();

    //  //    if (scaleDetail.Scale != null && scaleDetail.Scale.Ticket_Status.ToLower().Equals("close") && scaleDetail.Apply_To_Item != null)
    //  //    {
    //  //        DeleteInventory(scaleDetail, dbContext);                    
    //  //    }                
    //  //}
    //}

    public void UpdateWeight(smART.ViewModel.ScaleDetails businessEntity, smART.Model.ScaleDetails modelEntity, smART.Model.smARTDBContext dbContext,bool isDelete =false) {
      try {
        if (!modelEntity.Scale.QScale) {
          modelEntity.GrossWeight = modelEntity.Scale.Gross_Weight * (modelEntity.Split_Value / 100);
          modelEntity.TareWeight = modelEntity.Scale.Tare_Weight * (modelEntity.Split_Value / 100);
          modelEntity.NetWeight = modelEntity.GrossWeight - modelEntity.TareWeight - modelEntity.Contamination_Weight + modelEntity.Settlement_Diff_NetWeight;          
        }
        else {
          IEnumerable<smART.Model.ScaleDetails> scaleDetails = from scaledetail in dbContext.T_Scale_Details
                                                               where scaledetail.Scale.ID == businessEntity.Scale.ID && scaledetail.Active_Ind==true 
                                                               select scaledetail;
          smART.Model.Scale scale = dbContext.T_Scale.FirstOrDefault(s => s.ID == businessEntity.Scale.ID);
          if (scaleDetails != null) {
            scale.Gross_Weight = scaleDetails.Sum(s => s.GrossWeight);
            scale.Tare_Weight = scaleDetails.Sum(s => s.TareWeight);
            scale.Net_Weight = scaleDetails.Sum(s => s.NetWeight);

            if (isDelete) {
              scale.Gross_Weight -= businessEntity.GrossWeight;
              scale.Tare_Weight -= businessEntity.TareWeight;
              scale.Net_Weight -= businessEntity.NetWeight;
            }

            dbContext.SaveChanges();
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, modelEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

  }
}
