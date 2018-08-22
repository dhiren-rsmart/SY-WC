using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Transaction_InvoiceItem)]
    public class InvoiceItemController : InvoiceChildGridController<InvoiceItemLibrary, InvoiceItem>
    {
        public InvoiceItemController() : base("InvoiceItem", new string[] { "Invoice", "Item" }) { }

        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<InvoiceItem> resultList;
            //if (isNew || id == "0")
            //{
            //    resultList = TempEntityList;
            //    totalRows = TempEntityList.Count;
            //    resultList = ((IParentChildLibrary<InvoiceItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" });

            //}
            //else
            //{
            //  resultList = ((IParentChildLibrary<InvoiceItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" });
            //}

            //totalRows = resultList.Count();
            resultList = ((IParentChildLibrary<InvoiceItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" });
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        [HttpGet]
        public string _GetTotal(string id)
        {
            int intBookingId = Convert.ToInt32(id);
            InvoiceItemLibrary lib = new InvoiceItemLibrary( ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetTotal(intBookingId, new string[] { "Scale.Container_No.Booking.Sales_Order_No.Party", "Apply_To_Item", "Item_Received" }).ToString();
        }
        


        //private IEnumerable<InvoiceItem> GetInvoiceItems(out int totalRows, int id, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        //{
        //    IEnumerable<ScaleDetails> resultList = GetScaleDetails(out totalRows, id, page, pageSize, sortColumn, sortType, includePredicate, filters);

        //    InvoiceItemLibrary lib = new InvoiceItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());

        //    IEnumerable<InvoiceItem> busEnumeration = lib.GetInvoiceItems(resultList);
         
        //    return busEnumeration;
        //}

        
        

        //protected override ActionResult Display(GridCommand command, bool isNew = false)
        //{
        //    int totalRows = 0;
        //    IEnumerable<InvoiceItem> resultList = null;

        //    if (isNew)
        //    {
        //        resultList = TempEntityList;
        //        totalRows = TempEntityList.Count;
        //    }
        //    else
        //    {
        //        resultList = Library.GetAllByPaging(out totalRows, command.Page, command.PageSize, "", "Asc", IncludePredicates,
        //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
        //    }

        //    return View(new GridModel { Data = resultList, Total = totalRows });
        //}


        //[HttpPost]
        //[GridAction(EnableCustomBinding = true)]
        //public override ActionResult _Index(GridCommand command)
        //{
        //    int totalRows = 0;
        //    IEnumerable<InvoiceItem> resultList = ((ILibrary<InvoiceItem>)Library).GetAllByPaging(
        //                                                    out totalRows,
        //                                                    command.Page,
        //                                                    command.PageSize,
        //                                                    "",
        //                                                    "Asc",
        //                                                    new string[] { "Item" },
        //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

        //    return View(new GridModel { Data = resultList, Total = totalRows });
        //}

    }

}