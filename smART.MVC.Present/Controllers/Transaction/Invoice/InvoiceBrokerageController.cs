using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Transaction_InvoiceItem)]
  public class InvoiceBrokerageController : InvoiceChildGridController<InvoiceBrokerageLibrary, InvoiceItem> {

    public InvoiceBrokerageController() : base("InvoiceItem", new string[] { "Invoice", "Item" }) { }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<InvoiceItem> resultList;    
      resultList = ((IParentChildLibrary<InvoiceItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Booking.Sales_Order_No.Party"});
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [HttpGet]
    public string _GetTotal(string id) {
      int intBookingId = Convert.ToInt32(id);
      InvoiceBrokerageLibrary lib = new InvoiceBrokerageLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetTotal(intBookingId, new string[] { "Booking.Sales_Order_No.Party"}).ToString();
    }

  }
}