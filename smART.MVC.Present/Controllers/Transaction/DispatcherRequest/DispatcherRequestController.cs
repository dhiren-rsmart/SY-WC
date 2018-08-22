using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Dispatcher)]
  public class DispatcherRequestController : BaseFormController<DispatcherRequestLibrary, DispatcherRequest> {
    #region Local Variables

    string[] _predicates = new string[] { "TruckingCompany", "Driver", "Booking_Ref_No.Sales_Order_No.Party", "Booking_Ref_No.Shipping_Company", "Purchase_Order_No", "Party", "Location", "Shipper", "Sales_Order_No","Asset","Container" };

    #endregion Local Varialbes

    #region Constructors

    public DispatcherRequestController()
      : base("~/Views/Transaction/DispatcherRequest/_List.cshtml",
             new string[] { "TruckingCompany", "Driver", "Booking_Ref_No.Sales_Order_No.Party", "Booking_Ref_No.Shipping_Company", "Purchase_Order_No", "Party", "Location","Asset","Container"},
             new string[] { "DispatcherRequestExpense" },
             new string[] { "TruckingCompany", "Driver", "Booking_Ref_No", "Purchase_Order_No", "Party", "Location", "Shipper", "Sales_Order_No","Asset","Container"}
            ) {
    }

    #endregion Constructors

    #region Override

    //[HttpPost]
    //public override ActionResult Save(DispatcherRequest entity)
    //{
    //    ModelState.Clear();
    //    Validate(entity);

    //    if (ModelState.IsValid)
    //    {
    //        if (entity.ID == 0)
    //        {
    //            entity = Library.Add(entity);

    //            // Also save all relevant child records in database
    //            if (ChildEntityList != null)
    //            {
    //                SaveChildEntities(ChildEntityList, entity);
    //                ClearChildEntities(ChildEntityList);
    //            }
    //        }
    //        else
    //        {
    //            Library.Modify(entity, new string[] { "TruckingCompany", "Driver", "Booking_Ref_No", "Purchase_Order_No", "Party", "Location", "Shipper", "Sales_Order_No" });

    //        }

    //        ModelState.Clear();
    //    }

    //    else
    //        return Display(entity);

    //    return Display(entity.ID.ToString());
    //}

    //public override ActionResult Index(int? id)
    //{
    //    if (id.HasValue)
    //    {
    //        DispatcherRequest entity = Library.GetByID(id.ToString(), _predicates);
    //        return Display(entity);
    //    }
    //    else
    //        return RedirectToAction("New");
    //}

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      DispatcherRequest entity = Library.GetByID(id.ToString(), _predicates);
      return Json(entity);
    }

    protected override void SaveChildEntities(string[] childEntityList, DispatcherRequest entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "DispatcherRequestExpense":
            if (Session[ChildEntity] != null) {
              DispatcherRequestExpenseLibrary lib = new DispatcherRequestExpenseLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<ExpensesRequest> resultList = (IList<ExpensesRequest>)Session[ChildEntity];
              foreach (ExpensesRequest exp in resultList) {
                exp.Reference = new DispatcherRequest {
                  ID = entity.ID
                };
                exp.Reference_Table = entity.GetType().Name;
                exp.Reference_ID = entity.ID;
                lib.Add(exp);
              }
            }
            break;

          #endregion
        }
      }
    }
    #endregion Override

    #region Public

    [HttpPost]
    public ActionResult _GetDispatcherRequest(int id) {
      return new JsonResult {
        Data = Helpers.PartyHelper.GetPartyByID(id.ToString())
      };
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _DispatcherRequestByCategory(GridCommand command, string category) {
      int totalRows = 0;
      DispatcherRequestLibrary dispathcerLib = new DispatcherRequestLibrary();
      dispathcerLib.Initialize(ConfigurationHelper.GetsmARTDBContextConnectionString());
      //IEnumerable<DispatcherRequest> resultList = dispathcerLib.GetOpenDispatcherByCategory(category, new string[] { "TruckingCompany", "Driver" });
      IEnumerable<DispatcherRequest> resultList = dispathcerLib.GetOpenDispatcherByCategoryWithPagging(category,
                                                                                                       out totalRows,
                                                                                                       command.Page,
                                                                                                       command.PageSize,
                                                                                                       command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                       command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                       _predicates,
                                                                                                       (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
                                                                                                      
      return View(new GridModel {Data = resultList,Total = totalRows});
    }

    public  ActionResult Display(string id) {
      DispatcherRequest result = Library.GetByID(id, _predicates);
      return View("New", result);
    }

    [HttpGet]
    public ActionResult SelectContainerItem(int? id) {
      if (id.HasValue) {
        Container container = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id.ToString(), new string[] { "Booking.Sales_Order_No.Party", "Booking.Shipping_Company" });
        if (container != null) {
          DispatcherRequest result = new DispatcherRequest();
          result.RequestCategory = "Container";
          result.RequestType = "Drop off only";
          result.Booking_Ref_No = new Booking();
          result.Booking_Ref_No = container.Booking;
          result.Container_No = container.Container_No;
          result.Shipper = container.Booking.Shipping_Company;
          result.Sales_Order_No = container.Booking.Sales_Order_No;
          ViewBag.IsFromLoadedContainer = true;
          result.ID = 0;
          return Display(result);
        }
      }

      return RedirectToAction("New");
    }

    #endregion Public

    #region Private

    protected override void ValidateEntity(DispatcherRequest entity) {
      ModelState.Clear();

      if (entity.TruckingCompany.ID == 0)
        ModelState.AddModelError("Trucking Company", "Trucking company is required");
      if (entity.Driver.ID == 0)
        ModelState.AddModelError("Driver", "Driver is required");
      if (entity.RequestCategory.Contains("Select"))
        ModelState.AddModelError("Request Category", "Request Category is required");
      if (entity.RequestStatus.Contains("Select"))
        ModelState.AddModelError("Request Status", "Request Status is required");
      if (entity.RequestType.Contains("Select"))
        ModelState.AddModelError("Request Type", "Request Type is required");

      if (entity.RequestCategory.ToLower().Equals("container") && entity.Booking_Ref_No.ID == 0)
        ModelState.AddModelError("Booking_Ref_No", "Booking Ref No is required");

      if (entity.RequestCategory.ToLower().Equals("container") && entity.RequestStatus.ToLower().Equals("close") && ((entity.Container_No == null) || string.IsNullOrEmpty(entity.Container_No)))
        ModelState.AddModelError("Container", "Container No is required");

      //if ((entity.RequestCategory.ToLower().Equals("bin") && entity.RequestStatus.ToLower().Equals("close") && ((entity.Bin_No == null) || string.IsNullOrEmpty(entity.Bin_No.Trim()))))
      //  ModelState.AddModelError("BinNO", "Bin No is required");

      if ((entity.RequestCategory.ToLower().Equals("bin") && entity.RequestStatus.ToLower().Equals("close") && string.IsNullOrEmpty(entity.Asset.Asset_No)))
        ModelState.AddModelError("BinNO", "Bin No is required");

      if ((entity.RequestCategory.ToLower().Equals("bin") && entity.RequestStatus.ToLower().Equals("close") && (entity.Location == null || entity.Location.ID<=0)))
        ModelState.AddModelError("Location", "Location is required");

      if (entity.RequestStatus.ToLower().Equals("close") && (!IsLineItemExits(entity.ID)))
        ModelState.AddModelError("DispatcherExp", "Expense item is required.");

      //if (entity.RequestCategory.ToLower().Equals("bin") && entity.RequestStatus.ToLower().Equals("close") && (entity.Amount_Supplier <= 0))
      //    ModelState.AddModelError("AmountToBePaid", "Amount To Be Pad is required");

      //if (entity.RequestCategory.ToLower().Equals("container") && entity.RequestStatus.ToLower().Equals("close") && (entity.Amount_Buyer <= 0))
      //    ModelState.AddModelError("AmountToBePaid", "Amount To Be Pad is required");

      if ((entity.Purchase_Order_No != null && entity.Purchase_Order_No.ID > 0) && (entity.Party.ID == 0)) {
        PurchaseOrder po = new PurchaseOrderLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(entity.Purchase_Order_No.ID.ToString(), new[] { "Party" });
        entity.Party = po.Party;
      }

      //if (entity.RequestCategory.ToLower().Equals("container") && ((entity.Container_No == null) || string.IsNullOrEmpty(entity.Container_No.Trim())))
      //    ModelState.AddModelError("Container", "Container No is required");

    }

    private bool IsLineItemExits(int Id) {
      bool exits = true;
      IEnumerable<ExpensesRequest> resultList;

      if (Id <= 0) {
        resultList = (IList<ExpensesRequest>)Session["DispatcherRequestExpense"];
      }
      else {
        DispatcherRequestExpenseLibrary lib = new DispatcherRequestExpenseLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        resultList = lib.GetAllByParentID(Id);
      }

      if (resultList == null || resultList.Count() <= 0) exits = false;

      return exits;
    }

    #endregion Private

    #region Deleted

    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<DispatcherRequest> resultList = ((ILibrary<DispatcherRequest>)Library).GetAllByPaging(out totalRows, 1, 20, "ID", "Desc", _predicates);

    //    return View("~/Views/Transaction/DispatcherRequest/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{

    //    int totalRows = 0;
    //    IEnumerable<DispatcherRequest> resultList = ((ILibrary<DispatcherRequest>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
    //                                                    command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
    //                                                    _predicates,
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    #endregion Deleted
  }
}
