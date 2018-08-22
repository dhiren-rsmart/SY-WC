using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.MVC.Present.Helpers;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using smART.Common;
using System;

namespace smART.MVC.Present.Controllers.Transaction {
  [Feature(EnumFeatures.Transaction_SettlementDetails)]
  public class SettlementDetailsController : BaseGridController<SettlementDetailsLibrary, SettlementDetails> {
    #region /* Constructors */

    public SettlementDetailsController() : base("SettlementDetails", new string[] { "Settlement" }) { }

    #endregion

    #region /* Supporting Actions - Display Actions */

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _Index(GridCommand command, int id) {
      int totalRows = 0;

      //Delete entity from TempEntityList if exits.
      if (TempEntityList != null && TempEntityList.Count > 0) {
        IList<SettlementDetails> settlementDetails = (from data in TempEntityList
                                                      where data.Scale_Details_ID.Scale.ID == id
                                                      select data as SettlementDetails).ToList<SettlementDetails>();

        foreach (var item in settlementDetails) {
          TempEntityList.Remove(item);
        }
      }


      //Get all unsettled scale details tickets
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      ScaleDetailsLibrary scaleDetailsLibrary = new smART.Library.ScaleDetailsLibrary();
      scaleDetailsLibrary.Initialize(dbContextConnectionString);
      IEnumerable<ScaleDetails> scaleDetailsList = scaleDetailsLibrary.GetAllByPagingByParentID
                                                          (out totalRows,
                                                          int.Parse(id.ToString()),
                                                          1,
                                                          20,
                                                          "",
                                                          "Asc",
                                                          new string[] { "Scale", "Scale.Purchase_Order", "Item_Received", "Apply_To_Item" }
                                                          );


      // Create temp settlement details collection by scale details tickets
      int iValue = 1;
      PurchaseOrderItemLibrary poItemLibrary = new smART.Library.PurchaseOrderItemLibrary();
      poItemLibrary.Initialize(dbContextConnectionString);

      foreach (var item in scaleDetailsList) {

        SettlementDetails settlementDetails = new SettlementDetails();
        settlementDetails.ID = iValue;
        settlementDetails.Scale_Details_ID = item;
        settlementDetails.Actual_Net_Weight = settlementDetails.Scale_Details_ID.NetWeight;

        //Get rate from PO Item
        if (item.Scale.Purchase_Order != null) {
          PurchaseOrderItem poItem = poItemLibrary.GetPOItemByItemCode(item.Scale.Purchase_Order.ID, item.Item_Received.ID, null);
          // If po item exits then update rate and amount.
          if (poItem != null) {
            settlementDetails.Rate = poItem.Price;
            settlementDetails.Item_UOM = poItem.Ordered_Qty_UOM;

            // If po item unit is not lbs.  
            if (!string.IsNullOrWhiteSpace(settlementDetails.Item_UOM) && settlementDetails.Item_UOM.ToLower() != "lbs") {
              UOMConversionLibrary uomConvLib = new UOMConversionLibrary();
              uomConvLib.Initialize(dbContextConnectionString);
              UOMConversion uomConv = uomConvLib.GetByUOM(poItem.Ordered_Qty_UOM, "LBS");
              // When convertion factor not exits.
              if (uomConv != null) {
                settlementDetails.Item_UOM_Conv_Fact = (decimal)uomConv.Factor;
                settlementDetails.Item_UOM_NetWeight = settlementDetails.Actual_Net_Weight / (decimal)uomConv.Factor;
                settlementDetails.Item_UOM_NetWeight = decimal.Round(settlementDetails.Item_UOM_NetWeight, 3, MidpointRounding.AwayFromZero);
              }
              else {
                settlementDetails.Item_UOM_Conv_Fact = 1;
                settlementDetails.Item_UOM_NetWeight = settlementDetails.Actual_Net_Weight;
              }
            }
            // When Unit is default. Set default unit.
            else {
              SetDefaultUnit(settlementDetails);
            }
            settlementDetails.Amount = settlementDetails.Item_UOM_NetWeight * poItem.Price;
            settlementDetails.Amount = decimal.Round(settlementDetails.Amount, 2, MidpointRounding.AwayFromZero);
          }
          else {
            // When PO Item not exists.Set default unit.
            SetDefaultUnit(settlementDetails);
          }
        }
        // When PO not exists.Set default unit.
        else {
          SetDefaultUnit(settlementDetails);
        }
        settlementDetails.Settlement_ID = new Settlement();
        settlementDetails.Price_List_ID = new PriceList();

        TempEntityList.Add(settlementDetails);
        iValue++;

      }

      return Display(command, id.ToString(), true);
    }

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<SettlementDetails> resultList;
      resultList = from data in TempEntityList where data.Scale_Details_ID.Scale.ID == int.Parse(id) select data;
      totalRows = resultList.Count();
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Insert(SettlementDetails data, GridCommand command, bool isNew = false) {
      ModelState.Clear();
      Validate(data);
      if (ModelState.IsValid) {
        ScaleDetailsLibrary ScaleDetailsLibrary = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        ScaleDetails scaleDetails = ScaleDetailsLibrary.GetByID(data.Scale_Details_ID.ID.ToString(), new string[] { "Scale", "Scale.Purchase_Order", "Item_Received", "Apply_To_Item" });
        data.Scale_Details_ID.Scale.ID = scaleDetails.Scale.ID;
        data.Scale_Details_ID.Apply_To_Item = scaleDetails.Apply_To_Item;
        data.Scale_Details_ID.Scale = scaleDetails.Scale;
        data.Scale_Details_ID.Contamination_Weight = scaleDetails.Contamination_Weight;
        data.Scale_Details_ID.Split_Value = scaleDetails.Split_Value;
        if (string.IsNullOrWhiteSpace(data.Item_UOM) || data.Item_UOM == "LBS") {
          data.Item_UOM = "LBS";
          data.Item_UOM_Conv_Fact = 1;
          data.Item_UOM_NetWeight = data.Actual_Net_Weight;
        }
        TempEntityList.SingleOrDefault(m => m.Scale_Details_ID.ID == data.Scale_Details_ID.ID).InjectFrom(data);
        ModelState.Clear();
      }
      return Display(command, data.Scale_Details_ID.Scale.ID.ToString(), isNew);
    }

    private void Validate(SettlementDetails entity) {
      if (entity.Amount == 0)
        ModelState.AddModelError("Amount", "Amount is required.");
      if (entity.Rate == 0)
        ModelState.AddModelError("Rate", "Rate is required.");
      if (entity.Actual_Net_Weight == 0)
        ModelState.AddModelError("weight", "weight is required.");
    }

    private void SetDefaultUnit(SettlementDetails settlementDetails) {
      settlementDetails.Item_UOM = "LBS";
      settlementDetails.Item_UOM_Conv_Fact = 1;
      settlementDetails.Item_UOM_NetWeight = settlementDetails.Actual_Net_Weight;
    }


    #endregion
  }
}
