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
  [Feature(EnumFeatures.Transaction_Receipt)]
  public class ReceiptController : BaseFormController<PaymentReceiptLibrary, PaymentReceipt> {
    #region Local Members

    private string[] _predicates = { "Party", "Account_Name" };

    #endregion Local Members

    #region Constructor

    public ReceiptController()
      : base("~/Views/Transaction/Receipt/_List.cshtml",
             new string[] { "Party", "Account_Name" },
             new string[] { "ReceiptDetails", "ReceiptNotes", "ReceiptAttachments", "ReceiptExpense" },
              new string[] { "Party", "Account_Name" }
            ) { }

    #endregion Constructor

    #region Override Methods

    public override ActionResult _Index() {
      int totalRows = 0;
      ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
      //PaymentReceiptLibrary lib = new PaymentReceiptLibrary(Configuration.GetsmARTDBContextConnectionString());
      //IEnumerable<PaymentReceipt> resultList = lib.GetReceiptsByPagging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities);
      PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<PaymentReceiptDetails> resultList = lib.GetReceiptDetailsByPagging(out totalRows, 1, ViewBag.PageSize, "", "Asc", new string[] { "Invoice", "PaymentReceipt.Party" });
      return View(_listViewUrl, resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Index(GridCommand command) {
      int totalRows = 0;

      FilterDescriptor filterDesc = new FilterDescriptor("Active_Ind", FilterOperator.IsNotEqualTo, "false");
      command.FilterDescriptors.Add(filterDesc);
      //PaymentReceiptLibrary lib = new PaymentReceiptLibrary(Configuration.GetsmARTDBContextConnectionString());
      //IEnumerable<PaymentReceipt> resultList = lib.GetReceiptsByPagging(out totalRows,
      //                                                    command.Page,
      //                                                    command.PageSize,
      //                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
      //                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
      //                                                    _includeEntities,
      //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      PaymentReceiptDetailsLibrary lib = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<PaymentReceiptDetails> resultList = lib.GetReceiptDetailsByPagging(out totalRows,
                                                          command.Page,
                                                          command.PageSize,
                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                          new string[] { "Invoice", "PaymentReceipt.Party" },
                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      return View(new GridModel {Data = resultList,Total = totalRows });
    }

    //public override ActionResult Index(int? id)
    //{
    //    if (id.HasValue)
    //    {
    //        return Display(id.ToString());
    //    }
    //    else
    //        return RedirectToAction("New");
    //}

    //public override ActionResult New(string id)
    //{
    //    // Create new object
    //    ClearChildEntities(ChildEntityList);
    //    PaymentReceipt entity = new PaymentReceipt();

    //    return Display(entity);
    //}
    //protected ActionResult Display(string id)
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
    //        ModelState.AddModelError("Applied_Amount", "Receipt details amount mismatch to total amount.");
    //    if (!IsReceiptDetailsExists() && entity.ID == 0)
    //        ModelState.AddModelError("Applied_Amount", "At least one ticket required to save payment.");
    //    if (entity.Cash_Amount == 0 && entity.Bank_Amount==0)
    //        ModelState.AddModelError("Amount", "Cash/Bank Amount is required.");

    //    if (ModelState.IsValid)
    //    {
    //        entity.Transaction_Type = EnumTransactionType.Receipt.ToString();
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

    protected override void ValidateEntity(PaymentReceipt entity) {
      ModelState.Clear();
      entity.Transaction_Type = EnumTransactionType.Receipt.ToString();
      //// Need to find an easier way
      if (entity.Party.ID == 0)
        ModelState.AddModelError("Party", "Party is required.");
      if (entity.Applied_Amount_To_Be != 0)
        ModelState.AddModelError("Applied_Amount", "Receipt details amount mismatch to total amount.");
      if (!IsReceiptDetailsExists() && entity.ID == 0)
        ModelState.AddModelError("Applied_Amount", "At least one ticket required to save payment.");
      if (entity.Cash_Amount == 0 && entity.Bank_Amount == 0)
        ModelState.AddModelError("Amount", "Cash/Bank Amount is required.");
      if (entity.Net_Amt < 0)
          ModelState.AddModelError("Net_Amt", "Negative net payable amount can't be allowed.");     
    }

    private bool IsReceiptDetailsExists() {
      IEnumerable<PaymentReceiptDetails> resultList = (IList<PaymentReceiptDetails>)Session["ReceiptDetails"];
      return resultList != null && resultList.Count() > 0;
    }

    protected override void SaveChildEntities(string[] childEntityList, PaymentReceipt entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */

          case "ReceiptDetails":
            if (Session[ChildEntity] != null) {
              PaymentReceiptDetailsLibrary library = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptDetails> resultList = (IList<PaymentReceiptDetails>)Session[ChildEntity];
              foreach (PaymentReceiptDetails PaymentReceiptDetails in resultList) {
                if (PaymentReceiptDetails.Apply_Amount > 0) {
                  PaymentReceiptDetails.PaymentReceipt = new PaymentReceipt { ID = entity.ID };
                  library.Add(PaymentReceiptDetails);
                }
              }
            }
            break;

          case "ReceiptNotes":
            if (Session[ChildEntity] != null) {
              PaymentReceiptNotesLibrary PaymentReceiptNotesLibrary = new PaymentReceiptNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptNotes> resultList = (IList<PaymentReceiptNotes>)Session[ChildEntity];
              foreach (PaymentReceiptNotes PaymentReceiptNote in resultList) {
                PaymentReceiptNote.Parent = new PaymentReceipt { ID = entity.ID };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                PaymentReceiptNotesLibrary.Add(PaymentReceiptNote);
              }
            }
            break;

          case "ReceiptAttachments":
            if (Session[ChildEntity] != null) {
              PaymentReceiptAttachmentsLibrary PaymentReceiptLibrary = new PaymentReceiptAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptAttachments> resultList = (IList<PaymentReceiptAttachments>)Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (PaymentReceiptAttachments PaymentReceipt in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(PaymentReceipt.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), PaymentReceipt.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(PaymentReceipt.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), PaymentReceipt.Document_RefId.ToString());
                PaymentReceipt.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(PaymentReceipt.Document_Name, sourcePath, destinationPath);

                PaymentReceipt.Parent = new PaymentReceipt { ID = entity.ID };
                PaymentReceiptLibrary.Add(PaymentReceipt);
              }
            }
            break;
          case "ReceiptExpense":
            if (Session[ChildEntity] != null) {
              ReceiptExpenseLibrary lib = new ReceiptExpenseLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ExpensesRequest> resultList = (IList<ExpensesRequest>)Session[ChildEntity];
              foreach (ExpensesRequest exp in resultList) {
                exp.Reference = new PaymentReceipt { ID = entity.ID };
                exp.Reference_Table = EnumTransactionType.Receipt.ToString();
                exp.Reference_ID = entity.ID;
                lib.Add(exp);
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

          case "ReceiptDetails":
            if (Convert.ToInt32(parentID) > 0) {
              PaymentReceiptDetailsLibrary PaymentReceiptDetailsLibrary = new PaymentReceiptDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptDetails> resultList = PaymentReceiptDetailsLibrary.GetAllByParentID(Convert.ToInt32(parentID), new string[] { "Invoice", "PaymentReceipt" });
              foreach (PaymentReceiptDetails PaymentReceiptDetails in resultList) {

                PaymentReceiptDetailsLibrary.Delete(PaymentReceiptDetails.ID.ToString());
              }
            }
            break;

          case "ReceiptNotes":
            if (Convert.ToInt32(parentID) > 0) {
              PaymentReceiptNotesLibrary PaymentReceiptNotesLibrary = new PaymentReceiptNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptNotes> resultList = PaymentReceiptNotesLibrary.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (PaymentReceiptNotes PaymentReceiptNote in resultList) {

                PaymentReceiptNotesLibrary.Delete(PaymentReceiptNote.ID.ToString());
              }
            }
            break;

          case "ReceiptAttachments":
            if (Convert.ToInt32(parentID) > 0) {
              PaymentReceiptAttachmentsLibrary PaymentReceiptLibrary = new PaymentReceiptAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<PaymentReceiptAttachments> resultList = PaymentReceiptLibrary.GetAllByParentID(Convert.ToInt32(parentID));

              foreach (PaymentReceiptAttachments PaymentReceiptAttachment in resultList) {

                PaymentReceiptLibrary.Delete(PaymentReceiptAttachment.ID.ToString());
              }
            }
            break;

          #endregion
        }
      }
    }

    protected override void Form_OnModified(PaymentReceipt entity) {
      AddInQB(entity);
    }

    protected override void Form_OnAdded(PaymentReceipt entity) {
      AddInQB(entity);
    }

    private void AddInQB(PaymentReceipt entity) {
      if (entity.ID > 0 && entity.Transaction_Status == "Closed") {
        QuickBookLibrary qbLibrary = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        qbLibrary.AddPaymentReceiptIntoQBLog(entity.ID);
      }
    }   

    #endregion Override Methods

    #region Deleted
    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<PaymentReceipt> resultList = ((ILibrary<PaymentReceipt>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc", _predicates);

    //    return View("~/Views/Transaction/Payment/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<PaymentReceipt> resultList = ((ILibrary<PaymentReceipt>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc",
    //                                                    _predicates,
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));


    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    #endregion Deleted
  }
}