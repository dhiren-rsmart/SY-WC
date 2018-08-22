using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;
using Omu.ValueInjecter;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Master_PriceList)]
  public class PriceListItemController : PriceListChildGridController<PriceListItemLibrary, PriceListItem> {


    public PriceListItemController() : base("PriceListItem", new string[] { "PriceList", "Item" }) {
    }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<PriceListItem> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      //if (isNew==true || id=="0" || id==null)
      if (isNew == true || id == "0") {
        //PriceListItem priceListItem = new PriceListItem();
        //priceListItem.Item = new Item() { ID=1,Item_Category="A",Item_Group="B",Long_Name="Item1",Short_Name="I1",Site_Org_ID=1,Priced=true,Active_Ind=true};
        //TempEntityList.Add(priceListItem);

        if (id == "-1") {
          string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
          ItemLibrary ItemLibrary = new smART.Library.ItemLibrary();
          ItemLibrary.Initialize(dbContextConnectionString);
          IEnumerable<Item> itemList = ItemLibrary.GetAll().Where(o => o.Priced == true);
          int iValue = 1;
          foreach (var item in itemList) {
            PriceListItem priceListItem = new PriceListItem();
            priceListItem.Item = item;
            priceListItem.ID = iValue;
            TempEntityList.Add(priceListItem);
            iValue++;
            //((PriceListItemLibrary)Library).Add(priceListItem);
          }
        }
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {


        resultList = ((IParentChildLibrary<PriceListItem>) Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
        //resultList = ((IParentChildLibrary<PriceListItem>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
      }

      return View(new GridModel {
        Data = resultList, Total = totalRows
      });
    }

    // + ", ItemID: " + itemID +       
    //
    [HttpPost]
    public JsonResult _GetItemPrice(int PriceListID, int ItemID) {
      decimal Price = 0;

      IEnumerable<PriceListItem> itemList = (IEnumerable<PriceListItem>) ((IParentChildLibrary<PriceListItem>) Library).GetAllByParentID(PriceListID, IncludePredicates);
      if (itemList != null) {
        PriceListItem priceListItem = itemList.SingleOrDefault(o => o.Item.ID == ItemID);
        if (priceListItem != null) {
          Price = priceListItem.Price;
        }
      }
      return Json(Price);
    }

    [HttpGet]
    public JsonResult _GetItemPriceByPriceIdAndItemId(string priceListID, string itemID) {
      decimal price = 0;
      PriceListItemLibrary lib = new PriceListItemLibrary( ConfigurationHelper.GetsmARTDBContextConnectionString());

      PriceListItem item = lib.GetItemByPriceListAndItemId(Convert.ToInt32( priceListID), Convert.ToInt32(itemID));
      if (item != null) {
        price = item.Price;
      }
      var data = new {
        Price = price
      };
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Update(PriceListItem data, GridCommand command, bool isNew = false)
    //{
    //    ModelState.Clear();
    //    if (data.Item == null)
    //    {
    //        ModelState.AddModelError("Item", "Item is Required.");
    //    }
    //    if (ModelState.IsValid)
    //    {
    //        if (isNew)
    //        {
    //            //TODO: Add logic to update in memory data
    //            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //        }
    //        else
    //        {
    //            Library.Modify(data);
    //        }
    //    }
    //    return Display(command, data, isNew);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(PriceListItem data, GridCommand command, bool isNew = false)
    //{
    //    ModelState.Clear();
    //    if (data.Item == null || data.Item.ID==0)
    //    {
    //        ModelState.AddModelError("Item", "Item is Required.");
    //    }
    //    if (ModelState.IsValid)
    //  {
    //    if (isNew)
    //        {
    //            TempEntityList.Add(data);
    //        }
    //        else
    //        {
    //            data = Library.Add(data);
    //        }
    //    }
    //    return Display(command, data, isNew);
    //}

    protected override void ValidateEntity(PriceListItem entity) {
      ModelState.Clear();
      if (entity.Item == null || entity.Item.ID == 0) {
        ModelState.AddModelError("Item", "Item is Required.");
      }
    }

  }
}
