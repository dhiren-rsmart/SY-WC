using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Master_PriceList)]
  public class PriceListController : BaseFormController<PriceListLibrary, PriceList> {
    #region Constructor

    public PriceListController() : base("~/Views/Master/PriceList/_List.cshtml", null, new string[] { "PriceListItem" }) {
    }

    #endregion Constructor

    #region Override Methods

    protected override void SaveChildEntities(string[] childEntityList, PriceList entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "PriceListItem":
            if (Session[ChildEntity] != null) {
              ILibrary<PriceListItem> priceListLibrary = new PriceListItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PriceListItem> resultList = (IList<PriceListItem>)Session[ChildEntity];
              foreach (PriceListItem priceListItem in resultList) {
                priceListItem.PriceList = new PriceList() {
                  ID = entity.ID
                };
                priceListLibrary.Add(priceListItem);
              }
            }
            break;



          #endregion
        }
      }
    }

    protected override void ValidateEntity(PriceList entity) {
      ModelState.Clear();

      if (string.IsNullOrWhiteSpace(entity.PriceList_Name)) {
        ModelState.AddModelError("Name", "Name is Required");
      }
      if (string.IsNullOrWhiteSpace(entity.UOM)) {
        ModelState.AddModelError("Name", "UOM is Required");
      }
    }

    #endregion Override Methods

    #region Deleted
    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<PriceList> resultList = ((ILibrary<PriceList>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc");
    //    return View("~/Views/Administration/PriceList/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<PriceList> resultList = ((ILibrary<PriceList>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc");

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}


    //[HttpPost]
    //public override ActionResult Save(PriceList entity) {

    //  if (ModelState.IsValid) {

    //    if (entity.ID == 0) {
    //      entity = Library.Add(entity);

    //      // Also save all relevant child records in database
    //      if (ChildEntityList != null) {
    //        SaveChildEntities(ChildEntityList, entity);
    //        ClearChildEntities(ChildEntityList);
    //      }
    //    }
    //    else {
    //      Library.Modify(entity);
    //    }
    //    ModelState.Clear();
    //  }
    //  return Display(entity);
    //}
       
    #endregion Deleted

  }
}
