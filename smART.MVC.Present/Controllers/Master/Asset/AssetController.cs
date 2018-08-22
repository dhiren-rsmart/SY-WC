using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;
using Telerik.Web.Mvc;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Master_Asset)]
  public class AssetController : BaseFormController<AssetLibrary, Asset> {

    #region Constructor

    public AssetController()
      : base("~/Views/Master/Asset/_List.cshtml", null, new string[] { "AssetAudit", "AssetAttachments" }) {
    }

    #endregion Constructor

    #region Override Methods

    protected override void ValidateEntity(smART.ViewModel.Asset entity) {
      if (string.IsNullOrEmpty(entity.Asset_No))
        ModelState.AddModelError("AssetNo", "The Asset# field is required.");
      if (string.IsNullOrWhiteSpace(entity.Asset_Type))
        ModelState.AddModelError("AssetType", "The Asset Type field is required.");
      if (!entity.Asset_Type.Equals("Bin", StringComparison.InvariantCultureIgnoreCase) && Session["AssetAudit"] != null && ((IList<AssetAudit>)Session["AssetAudit"]).Count > 0)
        ModelState.AddModelError("AssetType", "Only bin type asset is allowed to add in asset tracking.");
    }

    protected override void SaveChildEntities(string[] childEntityList, Asset entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "AssetAudit":
            if (Session[ChildEntity] != null) {
              ILibrary<AssetAudit> lib = new AssetAuditLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AssetAudit> resultList = (IList<AssetAudit>)Session[ChildEntity];
              foreach (AssetAudit result in resultList) {
                result.Asset = new Asset() { ID = entity.ID };
                lib.Add(result);
              }
            }
            break;

          case "AssetAttachments":
            if (Session[ChildEntity] != null) {
              AssetAttachmentsLibrary lib = new AssetAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AssetAttachments> resultList = (IList<AssetAttachments>)Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (AssetAttachments result in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(result.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(result.Document_RefId.ToString());
                result.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(result.Document_Name, sourcePath, destinationPath);
                result.Parent = new Asset { ID = entity.ID };
                lib.Add(result);
              }
            }
            break;

          #endregion
        }
      }
    }

    protected override void DeleteChildEntities(string[] childEntityList, string parentID) {

      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */

          case "AssetAudit":
            if (Convert.ToInt32(parentID) > 0) {
              AssetAuditLibrary library = new AssetAuditLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AssetAudit> resultList = library.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (AssetAudit entity in resultList) {
                library.Delete(entity.ID.ToString());
              }
            }
            break;

          case "AssetAttachments":
            if (Convert.ToInt32(parentID) > 0) {
              AssetAttachmentsLibrary library = new AssetAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AssetAttachments> resultList = library.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (AssetAttachments entity in resultList) {
                library.Delete(entity.ID.ToString());
              }
            }
            break;
        }
          #endregion /* Case Statements - All child grids */

      }
    }

    #endregion Override Methods

  }
}