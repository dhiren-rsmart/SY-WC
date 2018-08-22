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

  public class Cash {

    public void Adding(smART.ViewModel.Cash businessEntity, smART.Model.Cash modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateBalance(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    public void Modifying(smART.ViewModel.Cash businessEntity, smART.Model.Cash modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateBalance(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    public void GotSingle(smART.ViewModel.Cash businessEntity, smART.Model.Cash modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.Transaction_Type.Equals("add cash", StringComparison.InvariantCultureIgnoreCase))
        businessEntity.Amount = modelEntity.Amount_Received;
      else if (businessEntity.Transaction_Type.Equals("update balance", StringComparison.InvariantCultureIgnoreCase))
        businessEntity.Amount = modelEntity.Balance;
      else if (businessEntity.Transaction_Type.Equals("payment", StringComparison.InvariantCultureIgnoreCase))
        businessEntity.Amount = modelEntity.Amount_Paid;
    }

    private void UpdateBalance(smART.ViewModel.Cash businessEntity, smART.Model.Cash modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.Transaction_Type.Equals("add cash", StringComparison.InvariantCultureIgnoreCase))
        modelEntity.Amount_Received = businessEntity.Amount;
      if (businessEntity.Transaction_Type.Equals("payment", StringComparison.InvariantCultureIgnoreCase))
        modelEntity.Amount_Paid = businessEntity.Amount;
      if (businessEntity.Transaction_Type.Equals("update balance", StringComparison.InvariantCultureIgnoreCase))
        modelEntity.Balance = businessEntity.Amount;
      if (!businessEntity.Transaction_Type.Equals("update balance", StringComparison.InvariantCultureIgnoreCase)) {
        Model.Cash prevEntity = dbContext.Set<smART.Model.Cash>().AsQueryable().OrderByDescending(s => s.Created_Date).FirstOrDefault();
        modelEntity.Balance = (prevEntity == null ? 0 : prevEntity.Balance) + modelEntity.Amount_Received - modelEntity.Amount_Paid;
      }
    }

  }
}
