using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class Booking {

    #region Events

    public void GotSingle(smART.ViewModel.Booking businessEntity, smART.Model.Booking modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        // Calculate assigned and due container and container netweight.  
        IEnumerable<smART.Model.Container> containers = from c in dbContext.T_Container_Ref
                                                        where c.Booking.ID == modelEntity.ID && c.Active_Ind == true
                                                        select c;
        if (containers != null) {
          businessEntity.Containers_Assigned = containers.Count();
          businessEntity.Total_Weight = containers.Sum(od => od.Net_Weight);
          businessEntity.Containers_Due = businessEntity.No_Of_Containers - businessEntity.Containers_Assigned;
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    public void Adding(smART.ViewModel.Booking businessEntity, smART.Model.Booking modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      // If Booking Status  is null set default status to "Open"
      if (string.IsNullOrEmpty(modelEntity.Booking_Status))
        modelEntity.Booking_Status = "Open";
      cancel = false;
    }

    #endregion Events

   
  }
}
