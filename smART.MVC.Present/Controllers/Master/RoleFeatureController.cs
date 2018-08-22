using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Master_RoleFeature)]
  public class RoleFeatureController : BaseGridController<RoleFeatureLibrary, RoleFeature> {

    public RoleFeatureController() : base("RoleFeature", new string[] { "Role", "Feature" }, new string[] { "Role", "Feature" }) { }


    #region /* Supporting Actions - Display Actions */

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<RoleFeature> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = (Library.GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates));
        //resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
      }

      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    protected override ActionResult Display(GridCommand command, RoleFeature entity, bool isNew = false) {
      if (entity.Role != null && entity.Role.ID != 0)
        return Display(command, entity.Role.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }

    [HttpPost]
    public virtual ActionResult GetByParentID(string id) {
      IEnumerable<RoleFeature> resultList = Library.GetAllByParentID(int.Parse(id));
      SelectList list = new SelectList(resultList, "ListValue", "ListText");

      return Json(list);
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
    //  if (isNew) {
    //    //TODO: Delete entity with id
    //    RoleFeature entity = TempEntityList.FirstOrDefault(m => m.ID == int.Parse(id));
    //    TempEntityList.Remove(entity);
    //  }
    //  else {
    //    RoleFeature roleFeature = Library.GetByID(id,IncludePredicates);
    //    roleFeature.Active_Ind = false;
    //    roleFeature.Last_Updated_Date = DateTime.Now;
    //    roleFeature.Updated_By = System.Web.HttpContext.Current.User.Identity.Name;
    //    Library.Modify(roleFeature, IncludePredicates);

    //  }
    //  if (string.IsNullOrEmpty(MasterID))
    //    return Display(command, isNew);
    //  else
    //    return Display(command, MasterID, isNew);

    //}


    #endregion

  }
}