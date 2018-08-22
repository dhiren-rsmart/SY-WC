using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.Library;
using System.Web.Mvc;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present.Helpers {

  public class LOVHelper {

    public static SelectList LOVList(string LOVType, object selectedItem, string parentValue) {

      LOV selectLOV = new LOV() {
        LOV_Value = " ", LOV_Display_Value = " -- Select Value ---"
      };
      IEnumerable<LOV> selectList = new LOV[] { selectLOV };
      ILibrary<LOV> lib = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<LOV> result;

      if (!string.IsNullOrEmpty(parentValue))
        result = ((LOVLibrary) lib).GetByParentValue(parentValue);
      else
        result = ((LOVLibrary) lib).GetByLOVType(LOVType);

       result = result.OrderBy(o => o.LOV_Display_Value);
      selectList = selectList.Concat<LOV>(result);
      SelectList sList = new SelectList(selectList, "LOV_Value", "LOV_Display_Value", selectedItem);
      return sList;
    }
  }
}