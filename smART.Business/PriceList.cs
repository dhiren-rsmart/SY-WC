using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;


namespace smART.Business.Rules {

  public class PriceList {

    public void Adding(smART.ViewModel.PriceList businessEntity, smART.Model.PriceList modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateDefault(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    public void Modifying(smART.ViewModel.PriceList businessEntity, smART.Model.PriceList modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateDefault(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    private void UpdateDefault(smART.ViewModel.PriceList businessEntity, smART.Model.PriceList modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.IsDefault == true) {
        smART.Model.PriceList price = dbContext.T_PriceList.FirstOrDefault(i => i.IsDefault == true);
        if (price != null) {
          price.IsDefault = false;
          price.Last_Updated_Date = modelEntity.Last_Updated_Date;
          dbContext.SaveChanges();
        }
      }
    }
  }

}
