using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Helpers;
using smART.Common;
using System.Transactions;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Settlement)]
  public class SettlementController : BaseFormController<SettlementLibrary, Settlement> {
    #region /* Constructors */

    public SettlementController()
      : base("", null, new string[] { "SettlementDetails" }) {
    }

    #endregion

    #region Override Methods

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Index(GridCommand command) {
      return Display(command);
    }

    [GridAction(EnableCustomBinding = true)]
    public ActionResult Display(GridCommand command) {
      int totalRows = 0;

      foreach (FilterDescriptor filterDesc in command.FilterDescriptors) {
        if (filterDesc.Member == "Scale.ID")
          filterDesc.Member = "ID";
        else if (filterDesc.Member == "Scale.Party_ID.ListText")
          filterDesc.Member = "Party_ID.Party_Name";
        else if (filterDesc.Member == "Scale.Created_Date")
          filterDesc.Member = "Created_Date";
      }

      //Get all unsettled scale tickets
      string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      ScaleLibrary scaleLibrary = new smART.Library.ScaleLibrary();
      scaleLibrary.Initialize(dbContextConnectionString);
      IEnumerable<Scale> scaleList = scaleLibrary.GetUnSettledRecAndBrokAndTradScaleByPagging(
                                                      out totalRows,
                                                      command.Page,
                                                      command.PageSize,
                                                      "ID",
                                                      "Asc",
                                                       new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No.Booking.Sales_Order_No.Party" },
                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)

                                                    );

      IList<Settlement> resultList = new List<Settlement>();

      // Create settlement collection by scale tickets
      int iValue = 1;
      foreach (var item in scaleList) {
        Settlement settlement = new Settlement();
        settlement.ID = iValue;
        settlement.Scale = item;
        if (settlement.Scale.Purchase_Order == null) {
          settlement.Scale.Purchase_Order = new PurchaseOrder();
          settlement.Scale.Purchase_Order = item.Purchase_Order;
        }
        settlement.Amount = 0;
        settlement.Actual_Net_Weight = item.Net_Weight;
        settlement.Ready_For_Payment = false;
        resultList.Add(settlement);
        iValue++;
      }
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    public ActionResult _SaveSelected(List<string> list) {
      string msg = string.Empty;
      try {
        foreach (var item in list) {
          string[] values = item.Split(',');
          int scaleId = Convert.ToInt32(values[0]);
          decimal amt = Convert.ToDecimal(values[1]);
          decimal netWt = Convert.ToDecimal(values[2]);
          if (scaleId > 0 && amt > 0 && netWt > 0) {
            //Get all unsettled scale tickets
            string dbContextConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
            ScaleLibrary scaleLibrary = new smART.Library.ScaleLibrary();
            scaleLibrary.Initialize(dbContextConnectionString);
            Scale scale = scaleLibrary.GetUnSettledRecAndBrokAndTradScaleById(scaleId, new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No.Booking.Sales_Order_No.Party" });

            // Start transaction.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
              IsolationLevel = IsolationLevel.ReadCommitted
            })) {
              //Save settlement 
              Settlement settlement = SaveSettlement(scale, amt, netWt);

              //Save Settlement details.              
              bool saveDetails = SaveSettlementDetails(ChildEntityList, settlement);

              // If settlement details not saved or there are no items in details.  
              if (saveDetails == false)
                throw new Exception("Settlement details items not found.");

              msg += msg.Length > 0 ? "," + scaleId.ToString() : scaleId.ToString();
              scope.Complete();
            }
          }
        }
        if (string.IsNullOrEmpty(msg))
          return Json(msg);
        ModelState.Clear();
        ClearChildEntities(ChildEntityList);
        return Json(string.Format("Ticket Number: {0} is settled sucessfully.", msg));
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
        return Json(string.Format("An error occured during settlement."));
      }
    }

    [HttpPost]
    public ActionResult _ClearChildEntity() {
      ClearChildEntities(ChildEntityList);
      return null;
    }

    public Settlement SaveSettlement(Scale scale, decimal amount, decimal actual_Net_Weight) {
      Settlement settlement = new Settlement() {
        Scale = scale,
        //Created_By = HttpContext.User.Identity.Name,
        //Created_Date = DateTime.Now,
        //Active_Ind = true,
        //Updated_By = HttpContext.User.Identity.Name,
        //Last_Updated_Date = DateTime.Now,
        Actual_Net_Weight = actual_Net_Weight,
        Amount = amount,
        Ready_For_Payment = true
      };
      settlement = Library.Add(settlement);
      return settlement;
    }

    public bool SaveSettlementDetails(List<SettlementDetails> settlementDetails) {
      bool isSaved = false;

      SettlementDetailsLibrary settlementDetailsLibrary = new SettlementDetailsLibrary();
      settlementDetailsLibrary.Initialize(ConfigurationHelper.GetsmARTDBContextConnectionString());

      foreach (SettlementDetails settlementDet in settlementDetails) {
        settlementDetailsLibrary.Add(settlementDet);
        isSaved = true;
      }
      return isSaved;
    }

    public bool SaveSettlementDetails(string[] childEntityList, Settlement settlement) {
      bool isSaved = false;
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */

          case "SettlementDetails":
            if (Session[ChildEntity] != null) {
              SettlementDetailsLibrary settlementDetailsLibrary = new SettlementDetailsLibrary();
              settlementDetailsLibrary.Initialize(ConfigurationHelper.GetsmARTDBContextConnectionString());

              IEnumerable<SettlementDetails> resultList = (IList<SettlementDetails>) Session[ChildEntity];
              resultList = resultList.Where(o => o.Scale_Details_ID.Scale.ID == settlement.Scale.ID);
              foreach (SettlementDetails settlementDetails in resultList) {
                settlementDetails.Settlement_ID = new Settlement {
                  ID = settlement.ID
                };
                //settlementDetails.Created_By = HttpContext.User.Identity.Name;
                //settlementDetails.Created_Date = DateTime.Now;
                //settlementDetails.Active_Ind = true;
                //settlementDetails.Updated_By = HttpContext.User.Identity.Name;
                //settlementDetails.Last_Updated_Date = DateTime.Now;
                settlementDetailsLibrary.Add(settlementDetails);
                isSaved = true;
              }
            }
            break;
          #endregion
        }
      }
      return isSaved;
    }

    //public decimal GetlNetWtAndAmountTotal(Scale scale, out decimal netWtTotal, out decimal amountTotal)
    //{
    //    netWtTotal = 0;
    //    amountTotal = 0;

    //    if (scale != null && ChildEntityList != null)
    //    {
    //        foreach (string ChildEntity in ChildEntityList)
    //        {
    //            switch (ChildEntity)
    //            {
    //                #region /* Case Statements - All child grids */

    //                case "SettlementDetails":
    //                    if (Session[ChildEntity] != null)
    //                    {
    //                        IEnumerable<SettlementDetails> resultList = (IList<SettlementDetails>)Session[ChildEntity];
    //                        resultList = resultList.Where(o => o.Scale_Details_ID.Scale.ID == scale.ID);
    //                        foreach (SettlementDetails settlementDetails in resultList)
    //                        {
    //                            netWtTotal += settlementDetails.Actual_Net_Weight + settlementDetails.Scale_Details_ID.Contamination_Weight;
    //                            amountTotal += settlementDetails.Amount;
    //                        }
    //                    }
    //                    break;
    //                #endregion
    //            }
    //        }
    //    }
    //    return netWtTotal;
    //}

    public ActionResult _UnPaidSettledTickets() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      IEnumerable<Settlement> resultList = new SettlementLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetUnPaidTicketsWithPaging(
                                                                                      out totalRows,
                                                                                      1,
                                                                                      ViewBag.PageSize,
                                                                                      "",
                                                                                      "Asc"
                                                                                      );
      return View("~/Views/Transaction/Settlement/UnPaidSettledTickets.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _UnPaidSettledTickets(GridCommand command) {
      int totalRows = 0;
      IEnumerable<Settlement> resultList = new SettlementLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetUnPaidTicketsWithPaging(
                                                                                                          out totalRows,
                                                                                                          command.Page,
                                                                                                          command.PageSize,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                          new string[] { "Scale", "Scale.Party_ID", "Scale.Purchase_Order" },
                                                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                          );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }


    public ActionResult Undo(int? settlementId) {
      if (settlementId.HasValue) {
        try {
          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
            IsolationLevel = IsolationLevel.ReadCommitted
          })) {
            Library.Delete(settlementId.ToString(), new string[] { "Scale" });
            DeleteChildEntities(ChildEntityList, settlementId.ToString());
            scope.Complete();
          }
        }
        catch (Exception ex) {
          ModelState.AddModelError("Error", ex.Message);
        }
      }
      return RedirectToAction("New", "Payment");
    }

    protected override void DeleteChildEntities(string[] childEntityList, string parentID) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "SettlementDetails":
            if (Convert.ToInt32(parentID) > 0) {
              SettlementDetailsLibrary lib = new SettlementDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<SettlementDetails> resultList = lib.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (SettlementDetails result in resultList) {
                lib.Delete(result.ID.ToString(), new string[] { "Scale_Details_ID" });
              }
            }
            break;
          #endregion
        }
      }
    }
    #endregion Override Methods

  }
}
