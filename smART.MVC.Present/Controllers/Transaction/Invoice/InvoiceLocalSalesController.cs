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
  public class InvoiceLocalSalesController : InvoiceChildGridController<InvoiceLocalSalesLibrary, InvoiceLocalSales> {

    public InvoiceLocalSalesController() : base("InvoiceLocalSales", new string[] { "Invoice", "LocalSales" }) { }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<InvoiceLocalSales> resultList;
      InvoiceLocalSalesLibrary invLib = new InvoiceLocalSalesLibrary();
      invLib.Initialize(ConfigurationHelper.GetsmARTDBContextConnectionString());
      resultList = invLib.GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Sales_Order.Party", "Item_Received", "Apply_To_Item" });
      //if (isNew == false)
      //  resultList = ((IParentChildLibrary<InvoiceLocalSales>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Sales_Order.Party", "Item_Received", "Apply_To_Item" });
      //else {
      //  InvoiceLocalSalesLibrary invLib = new InvoiceLocalSalesLibrary();
      //  invLib.Initialize(ConfigurationHelper.GetsmARTDBContextConnectionString());
      //  resultList = invLib.GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Sales_Order.Party", "Item_Received", "Apply_To_Item" });
      //}
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [HttpGet]
    public string _GetTotal(string id) {
      int intBookingId = Convert.ToInt32(id);
      InvoiceLocalSalesLibrary lib = new InvoiceLocalSalesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetTotal(intBookingId, new string[] { "Scale.Sales_Order.Party", "Item_Received", "Apply_To_Item" }).ToString();
    }

  }

}