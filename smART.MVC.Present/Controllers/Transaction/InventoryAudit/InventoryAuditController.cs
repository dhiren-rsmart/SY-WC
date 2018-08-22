using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using Omu.ValueInjecter;
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers.Transaction {

  [Feature(EnumFeatures.Transaction_InventoryAudit)]
  public class InventoryAuditController : BaseGridController<InventoryAuditLibrary, InventoryAudit> {

    #region /* Constructors */

    public InventoryAuditController()
      : base("InventoryAudit",new string [] {"Item"}) {
    }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<InventoryAudit> resultList;
      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<InventoryAudit>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "Date", "Desc", new string[] { "Item"});
      }
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    protected override ActionResult Display(GridCommand command, InventoryAudit entity, bool isNew = false) {
      if (entity.Item != null && entity.Item.ID != 0)
        return Display(command, entity.Item.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }

    #endregion
  
  }
}
