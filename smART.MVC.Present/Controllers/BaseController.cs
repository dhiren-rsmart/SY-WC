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

   [HandleError(View = "Error")]
    public abstract class BaseController<TLibrary, TEntity> : Controller
    where TLibrary : ILibrary<TEntity>, new()
    where TEntity : ViewModelBase, new() {
    protected TLibrary Library { get; private set; }

    #region /* Local Members */

    //public delegate void delegateActionPerforming(TEntity businessEntity);
    //public event delegateActionPerforming OnAdding, OnModifying, OnDeleting;

    #endregion

    #region /* Constructors */

    public BaseController()
      : this(ConfigurationHelper.GetsmARTDBContextConnectionString()) {     
    }

    void BaseController_OnAdding(TEntity businessEntity) {
      throw new NotImplementedException();
    }

    public BaseController(string dbContextConnectionString)
      : base() {
      Library = new TLibrary();
      Library.Initialize(dbContextConnectionString);
    }

    #endregion

    #region /* Default Action */

    public virtual ActionResult Index(int? id) {
      return View();
    }

    [HttpPost]
    public virtual ActionResult _GetJSon(string id) {
      TEntity entity = Library.GetByID(id.ToString());

      return Json(entity);
    }

    public void ApplyFilterDescriptor(GridCommand command, Dictionary<string, string> values) {
      foreach (IFilterDescriptor filter in command.FilterDescriptors) {
        ApplyFilter(filter, values);
      }
    }

    private void ApplyFilter(IFilterDescriptor filter, Dictionary<string, string> values) {
      var filters = string.Empty;
      if (filter is CompositeFilterDescriptor) {
        var compositeFilterDescriptor = (CompositeFilterDescriptor) filter;
        foreach (IFilterDescriptor childFilter in compositeFilterDescriptor.FilterDescriptors) {
          ApplyFilter(childFilter, values);
        }
      }
      else {
        var descriptor = (FilterDescriptor) filter;
        foreach (var item in values) {
          if (descriptor.Member == item.Key)
            descriptor.Member = item.Value;
        }
      }
    }

    #endregion  
  }
}
