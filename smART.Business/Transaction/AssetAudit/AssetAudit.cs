using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class AssetAudit {

    #region Events

    public void Adding(smART.ViewModel.AssetAudit businessEntity, smART.Model.AssetAudit modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdatePreviousLocation(modelEntity, dbContext);
      // Set Asset_Current_Location_Flg to true for current location.
      modelEntity.Asset_Current_Location_Flg = true;
      cancel = false;
    }

    #endregion Events

    #region Helper Methods

    /// <summary>
    /// Update Asset_Current_Location_Flg to false for all previous location records.  
    /// </summary>
    /// <param name="businessEntity"></param>
    /// <param name="dbContext"></param>
    public void UpdatePreviousLocation(smART.Model.AssetAudit modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        IEnumerable<Model.AssetAudit> results = dbContext.T_Asset_Audit.Where(o => o.Asset.ID == modelEntity.Asset.ID && o.Asset.Asset_Type == "Bin");
        if (results != null && results.Count() > 0) {
          foreach (var item in results) {
            item.Asset_Current_Location_Flg = false;
          }
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, "0");
        if (rethrow)
          throw ex;
      }
    }

    #endregion


    public void AddNewLocation(smART.Model.AssetAudit assetAudit, smART.Model.smARTDBContext dbContext) {
      smART.Model.AssetAudit modelAssetAudit = dbContext.T_Asset_Audit.Where(o => o.Asset.ID == assetAudit.Asset.ID && o.Party.ID == assetAudit.Party.ID && o.Location.ID == assetAudit.Location.ID && o.Dispatcher_Request.ID == assetAudit.Dispatcher_Request.ID && o.Asset_Current_Location_Flg == true).FirstOrDefault();
      if (modelAssetAudit == null) {
        UpdatePreviousLocation(assetAudit, dbContext);
        dbContext.T_Asset_Audit.Add(assetAudit);
        dbContext.SaveChanges();
      }
    }
  }
}
