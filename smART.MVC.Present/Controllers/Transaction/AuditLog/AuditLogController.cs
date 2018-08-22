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

  [Feature(EnumFeatures.Transaction_AuditLog)]
  public class AuditLogController : BaseGridController<AuditLogLibrary, AuditLog> {
    #region /* Constructors */

    public AuditLogController()
      : base("AuditLog") {
    }

    #endregion

    #region /* Supporting Actions - Display Actions */

    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetAuditLogByEntity(GridCommand command,string entityName,string entityId) {
      int totalRows = 0;      
      AuditLogLibrary lib = new AuditLogLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<AuditLog> resultList = lib.GetAuditLogByEntityWithPagging(entityName, int.Parse(entityId),
                                                      out totalRows,
                                                      command.Page,
                                                      command.PageSize == 0 ? 20 : command.PageSize,
                                                      "",
                                                      "Asc",
                                                      IncludePredicates
                                                     );
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [GridAction(EnableCustomBinding = true)]
    public ActionResult _GetCashAuditLog(GridCommand command) {
      int totalRows = 0;
      AuditLogLibrary lib = new AuditLogLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<AuditLog> resultList = lib.GetCashAuditLogWithPagging(out totalRows,
                                                      command.Page,
                                                      command.PageSize == 0 ? 20 : command.PageSize,
                                                      "Created_Date",
                                                      "Desc",
                                                      IncludePredicates
                                                     );
      return View(new GridModel {
        Data = resultList, Total = totalRows
      });
    }

    #endregion
  }
}
