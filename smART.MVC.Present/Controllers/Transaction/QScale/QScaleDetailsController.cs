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
using smART.MVC.Present.Extensions;


namespace smART.MVC.Present.Controllers.Transaction {

  [Feature(EnumFeatures.Transaction_ScaleDetails)]
  public class QScaleDetailsController : ScaleDetailsController {

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _InsertUpdate(ScaleDetails data, GridCommand command, bool isNew = false) {
      data.Split_Value = 100;
      if (data.ID > 0)
        return _Update(data, command, isNew);
      else {
        ScaleDetails tempItem = TempEntityList.FirstOrDefault(i => i.Item_Received.ID == data.Item_Received.ID);
        if (tempItem == null)
          return _Insert(data, command, isNew);
        else {
          data.ID = tempItem.ID;
          return _Update(data, command, isNew);
        }
      }
    }


    [HttpGet]
    public JsonResult _GetTotal(string id) {
      int scaleID = int.Parse(id);
      decimal gw = 0;
      decimal nw = 0;
      decimal amt = 0;
      if (scaleID > 0) {
        IEnumerable<ScaleDetails> sDetails = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(scaleID);
        gw = sDetails.Sum(i => i.GrossWeight);
        nw = sDetails.Sum(i => i.NetWeight);
        amt = sDetails.Sum(i => i.Amount);
      }
      else {
        gw = TempEntityList.Sum(i => i.GrossWeight);
        nw = TempEntityList.Sum(i => i.NetWeight);
        amt = TempEntityList.Sum(i => i.Amount);
      }

      var data = new {
        GW = gw, NW = nw, Amt = amt
      };

      return Json(data, JsonRequestBehavior.AllowGet);
      //string str = string.Format("{0}#{1}#{2}", gw, nw, amt);
      //return str;
    }

    protected override void ValidateEntity(ScaleDetails entity) {
      ModelState.Clear();
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult Validate(ScaleDetails data) {
      ModelState.Clear();
      // Set ApplyToItem to ItemReceived when ApplyToItem is null.
      if (data.Apply_To_Item.ID == 0) {
        data.Apply_To_Item = data.Item_Received;
      }

      if (data.Item_Received == null || data.Item_Received.ID == 0) {
        ModelState.AddModelError("ItemRecived", "Material is a required field.");
      }

      //if (data.Apply_To_Item == null || data.Apply_To_Item.ID == 0) {
      //  ModelState.AddModelError("ItemApply", "Apply To Item is required");
      //}

      //ValidateDuplicateItem(data);

      ValidItem(data);

      if (!ModelState.IsValid) {
        return Json(new {
          success = false,
          errors = ModelState.Errors()
        });

      }
      return Json(new {
        success = true,
        errors = ""
      });
    }
    public ActionResult _TicketHistory(string partyId) {
      int totalRows = 0;
      int id = string.IsNullOrEmpty(partyId) ? 0 : Convert.ToInt32(partyId);
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<ScaleDetails> resultList = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                              .GetAllByPartyIdWithPagging(
                                                                                           id,
                                                                                            out  totalRows,
                                                                                            1,
                                                                                            ViewBag.PageSize,
                                                                                            "Created_Date",
                                                                                             "Desc",
                                                                                            new string[] { "Scale", "Item_Received" },
                                                                                            null
                                                                                          );
      ViewBag.PartyId = id;
      return View("~/Views/Transaction/QScale/_TicketHistory.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _TicketHistory(string partyId, GridCommand command) {
      int totalRows = 0;
      int id = string.IsNullOrEmpty(partyId) ? 0 : Convert.ToInt32(partyId);
      IEnumerable<ScaleDetails> resultList = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                               .GetAllByPartyIdWithPagging(
                                                                                          id,
                                                                                              out totalRows,
                                                                                                           command.Page,
                                                                                                           command.PageSize,
                                                                                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                           command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                           new string[] { "Scale", "Item_Received" },
                                                                                                           (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                 );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    //public override ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
    //  ScaleLibrary scaleLib = new ScaleLibrary(Configuration.GetsmARTDBContextConnectionString());
    //  Scale scale =   scaleLib.GetByID(MasterID);
    //  if (scale.Ticket_Settled == true)
    //    throw new Exception("Can not delete paid ticket item.");
    //  return base._Delete(id, command, MasterID, isNew);
    //}
  }
}
