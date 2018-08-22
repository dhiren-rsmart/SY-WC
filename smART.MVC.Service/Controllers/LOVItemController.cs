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

    public class LOVItemController : BaseController<LOVLibrary, LOV>
    {

        public LOVItemController()
        {
        }

        // GET api/values/5
        [HttpGet]
        public IEnumerable<LOV> GetByParent(int id)
        {
            try
            {
                LOVLibrary lib = new LOVLibrary(base.ConString);
                IEnumerable<LOV> lovs = lib.GetAllByParentID(id, new string[] { "LOVType","Parent" });
                foreach (var item in lovs)
                {
                    if (item.Parent != null)
                        item.Parent_Type_ID = item.Parent.ID;
                }              
                return lovs;
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
