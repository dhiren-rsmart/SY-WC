using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business {

  public class BusinessUtils {

    public static double Convert(string UnitFrom, string UnitTo, double UnitValue, smART.Model.smARTDBContext dbContext) {
      if (UnitFrom.Equals(UnitTo, StringComparison.OrdinalIgnoreCase))
        return UnitValue;

      smART.Model.UOMConversion uomConversion = dbContext.M_UOM_Conversion.Where(o => o.Conversion_UOM.Equals(UnitFrom, StringComparison.OrdinalIgnoreCase)
                                                                                  && o.Base_UOM.Equals(UnitFrom, StringComparison.OrdinalIgnoreCase)
                                                                                  && o.Is_Base_UOM == true
                                                                               ).SingleOrDefault();
      double factor = uomConversion.Factor;
      return UnitValue * factor;
    }

    public static  decimal GetNetWeight(decimal grossWeight, decimal tareWeight, decimal settlementNetWeight) {
      return grossWeight - tareWeight + settlementNetWeight;
    }

  }
}
