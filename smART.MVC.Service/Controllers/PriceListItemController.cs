using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.ViewModel;
using smART.Library;

using smART.ViewModel;

namespace smART.MVC.Service.Controllers
{

    public class PriceListItemController : BaseController<PriceListItemLibrary, PriceListItem>
    {

        public PriceListItemController()
        {
        }

        // GET api/values/5
        [HttpGet]
        public IEnumerable<PriceListItem> GetByParent(int id)
        {
            try
            {
                PriceListItemLibrary lib = new PriceListItemLibrary(base.ConString);
                return lib.GetAllByParentID(id, new string[] { "PriceList" });
            }
            catch (Exception ex)
            {
                string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
                smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
                return null;
            }
        }

        // GET api/values/5
        [HttpGet]
        public IEnumerable<Service.Model.PriceListItem> GetDefaultPriceListItems()
        {
            try
            {
                PriceListItemLibrary lib = new PriceListItemLibrary(base.ConString);
                IEnumerable<smART.ViewModel.PriceListItem> items = lib.GetDefaultPriceListItems();
                return from i in items select new Service.Model.PriceListItem() {ID= i.ID,  Item_ID = i.Item.ID, PriceList_ID = i.PriceList.ID, Price = i.Price ,Active_Ind=i.Active_Ind};

            }
            catch (Exception ex)
            {
                string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
                smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
                return null;
            }
        }
    }
}
