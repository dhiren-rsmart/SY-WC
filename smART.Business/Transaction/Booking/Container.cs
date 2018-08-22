using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class Container {

    #region Events

    public void Adding(smART.ViewModel.Container businessEntity, smART.Model.Container modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      // If Status is null or empty set default status to "Open-Loaded"
      if (string.IsNullOrEmpty(modelEntity.Status.Trim()))
        modelEntity.Status = "Open-Loaded";
      // Set default Date_In date to current system date.
      modelEntity.Date_In = DateTime.Now;
      cancel = false;
    }

    public void GotMultiple(IEnumerable<smART.ViewModel.Container> businessEntities, IEnumerable<smART.Model.Container> modelEntities, smART.Model.smARTDBContext dbContext) {
      try {
        // Set Container_Return_Date to next 4 business days.  
        foreach (smART.ViewModel.Container c in businessEntities) {
          DateTime dt = Convert.ToDateTime(c.Created_Date);
          c.Container_Return_Date = AddBusinessDays(dt, 4);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntities.FirstOrDefault().Updated_By, modelEntities.FirstOrDefault().GetType().Name, "0");
        if (rethrow)
          throw ex;
      }
    }


    #endregion Events

    #region Helper Methods

    public DateTime AddBusinessDays(DateTime date, int days) {
      date = date.AddDays((days / 5) * 7);

      int remainder = days % 5;

      switch (date.DayOfWeek) {
        case DayOfWeek.Tuesday:
          if (remainder > 3)
            date = date.AddDays(2);
          break;
        case DayOfWeek.Wednesday:
          if (remainder > 2)
            date = date.AddDays(2);
          break;
        case DayOfWeek.Thursday:
          if (remainder > 1)
            date = date.AddDays(2);
          break;
        case DayOfWeek.Friday:
          if (remainder > 0)
            date = date.AddDays(2);
          break;
        case DayOfWeek.Saturday:
          if (days > 0)
            date = date.AddDays((remainder == 0) ? 2 : 1);
          break;
        case DayOfWeek.Sunday:
          if (days > 0)
            date = date.AddDays((remainder == 0) ? 1 : 0);
          break;
        default:  // monday
          break;
      }
      return date.AddDays(remainder);
    }

    #endregion Helper Methods

   
  }
}
