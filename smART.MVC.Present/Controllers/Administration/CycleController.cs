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

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Administration_Cycle)]
  public class CycleController : BaseFormController<CycleLibrary, Cycle> {

    #region Local Members

    private string[] _predicates = { "CycleDetails" };
    private CycleLibrary _cycleOps;

    #endregion Local Members

    #region Constructor

    public CycleController()
      : base("~/Views/Administration/Cycle/_List.cshtml",
              null,
              new string[] { "CycleDetails" },
              null
            ) {
      _cycleOps = new CycleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    }

    #endregion Constructor

    #region Override Methods

    //public override ActionResult _Index() {
    //  int totalRows = 0;
    //  ViewBag.PageSize = Configuration.GetsmARTLookupGridPageSize();
    //  CycleLibrary lib = new CycleLibrary(Configuration.GetsmARTDBContextConnectionString());
    //  IEnumerable<Cycle> resultList = lib.GetPaymentsByPagging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities);
    //  return View(_listViewUrl, resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command) {
    //  int totalRows = 0;

    //  FilterDescriptor filterDesc = new FilterDescriptor("Active_Ind", FilterOperator.IsNotEqualTo, "false");
    //  command.FilterDescriptors.Add(filterDesc);
    //  PaymentReceiptLibrary lib = new PaymentReceiptLibrary(Configuration.GetsmARTDBContextConnectionString());
    //  IEnumerable<PaymentReceipt> resultList = lib.GetPaymentsByPagging(out totalRows,
    //                                                      command.Page,
    //                                                      command.PageSize,
    //                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
    //                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
    //                                                      _includeEntities,
    //                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
    //  return View(new GridModel {
    //    Data = resultList,
    //    Total = totalRows
    //  });
    //}

    //public override ActionResult Index(int? id)
    //{
    //    if (id.HasValue)
    //    {
    //        return Display(id.ToString());
    //    }
    //    else
    //        return RedirectToAction("New");
    //}

    //protected override ActionResult Display(string id)
    //{
    //    PaymentReceipt result = Library.GetByID(id, _predicates);

    //    return View("New", result);
    //}

    //[HttpPost]
    //public override ActionResult Save(PaymentReceipt entity)
    //{
    //    ModelState.Clear();

    //    //// Need to find an easier way
    //    if (entity.Party.ID == 0)
    //        ModelState.AddModelError("Party", "Party is required.");
    //    if (entity.Applied_Amount_To_Be != 0)
    //        ModelState.AddModelError("Applied_Amount_To_Be", "Payment details amount mismatch to total amount.");
    //    if (!IsPaymentDetailsExists() && entity.ID == 0)
    //        ModelState.AddModelError("Applied_Amount", "At least one ticket required to save payment.");
    //    if (entity.Cash_Amount == 0 && entity.Bank_Amount == 0)
    //        ModelState.AddModelError("Amount", "Cash/Bank Amount is required.");

    //    if (ModelState.IsValid)
    //    {
    //        entity.Transaction_Type = EnumTransactionType.Payment.ToString();

    //        if (entity.ID == 0)
    //        {
    //            entity = Library.Add(entity);

    //            //Also save all relevant child records in database
    //            if (ChildEntityList != null)
    //            {
    //                SaveChildEntities(ChildEntityList, entity);
    //                ClearChildEntities(ChildEntityList);
    //            }

    //        }
    //        else
    //        {
    //            Library.Modify(entity, _predicates);
    //        }
    //        ModelState.Clear();
    //    }
    //    else
    //        return Display(entity);

    //    return Display(entity.ID.ToString());
    //}

    protected override void ValidateEntity(Cycle entity) {
      ModelState.Clear();

      //// Need to find an easier way
      if (entity.Start_Date.Date == entity.End_Date.Date)
        ModelState.AddModelError("Equal", "Sart Date and End Date can not be same.");
      if (entity.Start_Date.Date > entity.End_Date.Date)
        ModelState.AddModelError("Greater", "Sart Date can not be greater then End Date.");
      if (_cycleOps.IsOverlap(entity.ID, entity.Start_Date, entity.End_Date))
        ModelState.AddModelError("Overlap", "Current cycel date is overlapping to previous cycle.");

      //if (entity.Applied_Amount_To_Be != 0)
      //  ModelState.AddModelError("Applied_Amount_To_Be", "Payment details amount mismatch to total amount.");
      //if (!IsPaymentDetailsExists() && entity.ID == 0)
      //  ModelState.AddModelError("Applied_Amount", "At least one ticket required to save payment.");
      //if (entity.Cash_Amount == 0 && entity.Bank_Amount == 0)
      //  ModelState.AddModelError("Amount", "Cash/Bank Amount is required.");
      //if (entity.Net_Amt < 0)
      //  ModelState.AddModelError("Net_Amt", "Negative net payable amount can't be allowed.");
      //if (string.IsNullOrEmpty(entity.Check_Wire_Transfer))
      //  ModelState.AddModelError("Net_Amt", "Check#/Wire Transfer# is required.");
    }

    //private bool IsPaymentDetailsExists() {
    //  IEnumerable<PaymentReceiptDetails> resultList = (IList<PaymentReceiptDetails>) Session["PaymentDetails"];
    //  return resultList != null && resultList.Count() > 0;
    //}

    protected override void SaveChildEntities(string[] childEntityList, Cycle entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "CycleDetails":
            if (Session[ChildEntity] != null) {
              CycleDetailsLibrary library = new CycleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<CycleDetails> resultList = (IList<CycleDetails>) Session[ChildEntity];
              foreach (CycleDetails cycleDetails in resultList) {
                cycleDetails.Cycle = new Cycle {ID = entity.ID};
                cycleDetails.Date = entity.Start_Date;
                library.Add(cycleDetails);
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
          case "CycleDetails":
            if (Convert.ToInt32(parentID) > 0) {
              CycleDetailsLibrary library = new CycleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<CycleDetails> resultList = library.GetAllByParentID(Convert.ToInt32(parentID), new string[] { "Item", "Purchase_ID" });
              foreach (CycleDetails cycleDetails in resultList) {
                library.Delete(cycleDetails.ID.ToString());
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
