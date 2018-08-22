using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.MVC.Present.Helpers;
using Omu.ValueInjecter;
using smART.Common;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Master_AssetAudit)]
  public class AssetAuditController : BaseGridController<AssetAuditLibrary, AssetAudit> {

    #region /* Constructors */

    public AssetAuditController()
      : base("AssetAudit", new string[] { "Asset" }, new string[] { "Asset", "Party", "Location", "Dispatcher_Request" }) {
    }

    #endregion


    #region /* Supporting Actions - Display Actions */

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<AssetAudit> resultList;

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<AssetAudit>)Library).GetAllByPagingByParentID(out totalRows, 
                                                                                         int.Parse(id.ToString()),
                                                                                         command.Page, 
                                                                                         command.PageSize == 0 ? 20 : command.PageSize, 
                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                          new string[] { "Asset", "Party", "Location", "Dispatcher_Request" }
                                                                                         );

      }
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    protected override ActionResult Display(GridCommand command, AssetAudit entity, bool isNew = false) {
      if (entity.Asset != null && entity.Asset.ID != 0)
        return Display(command, entity.Asset.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }

    protected override void ValidateEntity(AssetAudit entity) {
      ModelState.Clear();
     
      if (entity.Party == null || entity.Party.ID <= 0) {
        ModelState.AddModelError("Party", "The Party Name field is required.");
      }
      if (entity.Location == null || entity.Location.ID <= 0) {
        ModelState.AddModelError("Location", "The Location Name field is required.");
      }
      if (entity.Asset != null && entity.Asset.ID > 0) {
        Asset asset = new AssetLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(entity.Asset.ID.ToString());
        if (!asset.Asset_Type.Equals("Bin", StringComparison.InvariantCultureIgnoreCase))
            ModelState.AddModelError("AssetType", "Tracking is enabled only for Asset Type = Bin");
      }
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _AssetsByDispatcherType(GridCommand command, string dispatcherType) {
      int totalRows = 0;
      AssetAuditLibrary lib = new AssetAuditLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<AssetAuditLookup> resultList = lib.GetDispatcherReqTypeAssetsByPaging(dispatcherType,
                                                    out totalRows,
                                                    command.Page,
                                                    command.PageSize,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                    new string[] { "Party", "Location.Party","Asset" },
                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [HttpPost]
    public ActionResult _GetJSonLookup(string id) {
      AssetAudit assetAudit  = Library.GetByID(id.ToString(), new string[] { "Party", "Location","Asset"} );
       AssetAuditLookup assetAuditLookup = new AssetAuditLookup(){ID=assetAudit.ID ,Party= assetAudit.Party,Location= assetAudit.Location,Asset= assetAudit.Asset};
      return Json(assetAuditLookup);
    }

    protected override void ChildGrid_OnAdding(AssetAudit entity) {
      if (Session["AssetAudit"] != null) {
        List<AssetAudit> results = (List<AssetAudit>) Session["AssetAudit"];
        results.ForEach(x => {x.Asset_Current_Location_Flg=false;});  
      }  
      entity.Asset_Current_Location_Flg = true;
    }

    #endregion /* Supporting Actions - Display Actions */
  }
}