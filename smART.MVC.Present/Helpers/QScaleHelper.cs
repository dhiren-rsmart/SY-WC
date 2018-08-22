using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using System.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Helpers {

  public class QScaleHelper {

    public static SelectList ScaleReaders(object selectedItem) {      
      IEnumerable<string> result = new List<string>(){"Big Scale Reading","Small Scale Rading"};
      SelectList sList = new SelectList(result, selectedItem);
      return sList;
    }

    public static SelectList Make(object selectedItem) {
      IEnumerable<string> result = new List<string>() { "TATA", "Hyundai" };
      SelectList sList = new SelectList(result, selectedItem);
      return sList;
    }

    public static SelectList Model(object selectedItem) {
      IEnumerable<string> result = new List<string>() { "2001", "2002" };
      SelectList sList = new SelectList(result, selectedItem);
      return sList;
    }

    public static SelectList Color(object selectedItem) {
      IEnumerable<string> result = new List<string>() { "White", "Black" };
      SelectList sList = new SelectList(result, selectedItem);
      return sList;
    }

    public static IEnumerable<Scale> OpenTicketsList() {
        ScaleLibrary lib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Scale> result = lib.GetOpenTickets(smART.MVC.Present.Controllers.QScaleController.ReceivingTicketType);
      return result;
    }

    public static IEnumerable<PriceList> PriceList() {
        PriceListLibrary lib = new PriceListLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<PriceList> result = lib.GetActivePriceList();
      return result;
    }

    public static SelectList PriceList(object selectedItem) {
      PriceList selectItem = new PriceList() {
        ID = 0,  PriceList_Name = " -- Select Value ---"
      };
      IEnumerable<PriceList> selectList = new PriceList[] { selectItem };
      IEnumerable<PriceList> result = PriceList();
      PriceList defaultItem = result.FirstOrDefault(p => p.IsDefault);
      result = result.OrderBy(s => s.IsDefault);
      selectList = selectList.Concat<PriceList>(result);
      SelectList sList = new SelectList(selectList, "ID", "PriceList_Name", defaultItem == null ? selectedItem : defaultItem.ID);
      return sList;
    }

  }
}