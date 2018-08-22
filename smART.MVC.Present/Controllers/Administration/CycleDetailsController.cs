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
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Administration_CycleDetails)]
  public class CycleDetailsController : BaseGridController<CycleDetailsLibrary, CycleDetails> {

    CycleDetailsLibrary _cycleDetailsOps;

    public CycleDetailsController()
      : base("Cycle", new string[] { "Item", "Purchase_ID","Cycle" }, new string[] { "Item", "Purchase_ID","Cycle" }) {
      _cycleDetailsOps = new CycleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    }

    #region /* Supporting Actions - Display Actions */

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<CycleDetails> resultList;

      if (isNew == true || id == "0") {
        if (TempEntityList.Count <= 0) {
          ItemLibrary ItemLibrary = new smART.Library.ItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          IEnumerable<Item> itemList = ItemLibrary.GetAll().Where(o => o.IsActive == true && o.Active_Ind == true);
          int iValue = 1;
          foreach (var item in itemList) {
            CycleDetails cycleDetails = new CycleDetails();
            cycleDetails.Item = item;
            cycleDetails.ID = iValue;
            TempEntityList.Add(cycleDetails);
            iValue++;
          }
          resultList = TempEntityList;
          Session["CycleDetails"] = TempEntityList;
        }
        else {
          resultList = _cycleDetailsOps.GetAllByPaging(TempEntityList, out totalRows,
                                            command.Page,
                                            command.PageSize,
                                            command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                            command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                            IncludePredicates,
                                            (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                           );
        }
        Session["CycleDetails"] = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<CycleDetails>) Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
      }

      return View(new GridModel {
        Data = resultList, Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, CycleDetails entity, bool isNew = false) {
      if (entity.Cycle != null && entity.Cycle.ID != 0)
        return Display(command, entity.Cycle.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }

    [HttpPost]
    public void _RecalculateAvgCost(string id, string startDt, string endDt) {
      CycleLibrary cycleOps = new CycleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      int prevCycleId = cycleOps.GetPreviousCycleIDByStartDate(Convert.ToDateTime(startDt));

      if (prevCycleId > 0) {
        IEnumerable<CycleDetails> listCycleDetails = _cycleDetailsOps.GetAllByStartDate(prevCycleId, Convert.ToDateTime(startDt), base.IncludePredicates);

        foreach (CycleDetails cycleItem in listCycleDetails) {
          int itemId = cycleItem.Item.ID;
          CycleDetails tempItem = null;

          if (Convert.ToInt32(id) >0) {
            tempItem = _cycleDetailsOps.GetOpItemByParentID(Convert.ToInt32( id),itemId, base.IncludePredicates);
          }
          else {
            tempItem = (from t in TempEntityList
                        where t.Item.ID == itemId
                        select t).FirstOrDefault();

          }
          if (tempItem != null) {
            tempItem.Purchase_Qty = cycleItem.Purchase_Qty;
            tempItem.Purchase_Cost = cycleItem.Purchase_Cost;
            tempItem.Purchase_Amount = cycleItem.Purchase_Amount;
            tempItem.Average_Cost = cycleItem.Average_Cost;
            if (Convert.ToInt32(id) > 0) {
              _cycleDetailsOps.Modify(tempItem, base.IncludePredicates);
            }
          }
        }
      }
    }


    //protected override ActionResult Display(GridCommand command, string id, bool isNew)
    //{
    //    int totalRows = 0;
    //    IEnumerable<LOV> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);            

    //    if (Convert.ToInt32(id)>0)
    //      ViewBag.Parent_Type_ID = id;

    //    if (isNew || id == "0")
    //    {
    //        resultList = TempEntityList;
    //        totalRows = TempEntityList.Count;
    //    }
    //    else
    //    {
    //        resultList = (Library.GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates));
    //        //resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
    //    }

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    //protected override ActionResult Display(GridCommand command, LOV entity, bool isNew = false)
    //{
    //    if (entity.LOVType != null && entity.LOVType.ID != 0)
    //        return Display(command, entity.LOVType.ID.ToString(), isNew);
    //    else
    //        return base.Display(command, entity, isNew);
    //}

    //[HttpPost]
    //public virtual ActionResult GetByParentID(string id)
    //{
    //    IEnumerable<LOV> resultList = Library.GetAllByParentID(int.Parse(id));
    //    SelectList list = new SelectList(resultList, "ListValue", "ListText");

    //    return Json(list);
    //}


    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public ActionResult _GetAllLOV(GridCommand command) {
    //  int totalRows = 0;
    //  IEnumerable<LOV> resultList = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
    //                                                                                                  out totalRows, command.Page,
    //                                                                                                  (command.PageSize == 0 ? 20 : command.PageSize),
    //                                                                                                  "", "Asc", new string[] { "LOVType" },
    //                                                                                                  (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
    //                                                                                                  );
    //  return View(new GridModel {
    //    Data = resultList,
    //    Total = totalRows
    //  });
    //}

    protected override void ValidateEntity(smART.ViewModel.CycleDetails entity) {
      ModelState.Clear();
      if (entity.Item == null || entity.Item.ID == 0)
        ModelState.AddModelError("EmptyItem", "Item is required.");

      // Check duplicate item     
      if (entity.Cycle.ID == 0) {
        CycleDetails cycleDetails = TempEntityList.FirstOrDefault<CycleDetails>(s => s.Item.ID == entity.Item.ID && s.ID != entity.ID);
        if (cycleDetails != null)
          ModelState.AddModelError("EmptyItem", "Similar transaction already exists. Please click on search button and fetch existing record.");
      }

    }

    //[HttpGet]
    //public  JsonResult _GetByParentType(string parentType) {
    // return Json(LOVHelper.LOVList("Model","",parentType), JsonRequestBehavior.AllowGet);

    //  //return Josn  {
    //  //  Data = new SelectList(LOVHelper.LOVList("Model", "", parentType).ToList(), "LOV_Value", "LOV_Display_Value")
    //  //};

    //}

    #endregion

  }
}
