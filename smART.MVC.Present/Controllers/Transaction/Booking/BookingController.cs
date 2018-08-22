using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Helpers;
using AutoMapper;
using smART.Common;
using smART.Notification;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Transaction_Booking)]
  public class BookingController : BaseFormController<BookingLibrary, Booking> {
    #region Local Members

    private string[] _predicates = { "Sales_Order_No", "Sales_Order_No.Party", "Forwarder_Party_ID", "Shipping_Company", "Booking_Closed_By" };

    #endregion Local Members

    #region Constructor

    public BookingController()
      : base("~/Views/Transaction/Booking/_List.cshtml",
              new string[] { "Sales_Order_No", "Sales_Order_No.Party", "Forwarder_Party_ID", "Shipping_Company", "Booking_Closed_By" },
              new string[] { "Container", "BookingNotes", "BookingAttachments" },
              new string[] { "Sales_Order_No", "Forwarder_Party_ID", "Shipping_Company", "Booking_Closed_By" }) {
    }

    #endregion Constructor

    #region Override Methods

    protected override void SaveChildEntities(string[] childEntityList, Booking entity) {
      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */

          case "Container":
            if (Session[ChildEntity] != null) {
              ContainerLibrary containerLibrary = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Container> resultList = (IList<Container>)Session[ChildEntity];
              foreach (Container continer in resultList) {
                continer.Booking = new Booking {
                  ID = entity.ID
                };
                containerLibrary.Add(continer);
              }
            }
            break;

          case "BookingNotes":
            if (Session[ChildEntity] != null) {
              BookingNotesLibrary BookingNotesLibrary = new BookingNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<BookingNotes> resultList = (IList<BookingNotes>)Session[ChildEntity];
              foreach (BookingNotes BookingNote in resultList) {
                BookingNote.Parent = new Booking {
                  ID = entity.ID
                };
                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                BookingNotesLibrary.Add(BookingNote);
              }
            }
            break;

          case "BookingAttachments":
            if (Session[ChildEntity] != null) {
              BookingAttachmentsLibrary BookingLibrary = new BookingAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<BookingAttachments> resultList = (IList<BookingAttachments>)Session[ChildEntity];
              string destinationPath;
              string sourcePath;
              FilelHelper fileHelper = new FilelHelper();
              foreach (BookingAttachments Booking in resultList) {
                destinationPath = fileHelper.GetSourceDirByFileRefId(Booking.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), Booking.Document_RefId.ToString());
                sourcePath = fileHelper.GetTempSourceDirByFileRefId(Booking.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), Booking.Document_RefId.ToString());
                Booking.Document_Path = fileHelper.GetFilePath(sourcePath);
                fileHelper.MoveFile(Booking.Document_Name, sourcePath, destinationPath);

                Booking.Parent = new Booking {
                  ID = entity.ID
                };
                BookingLibrary.Add(Booking);
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

          case "Container":
            if (Convert.ToInt32(parentID) > 0) {
              ContainerLibrary containerLibrary = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Container> resultList = containerLibrary.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (Container continer in resultList) {
                string errorMsg;
                if (containerLibrary.IsRefExits(continer.ID, out errorMsg))
                  throw new Exception(errorMsg);
                containerLibrary.Delete(continer.ID.ToString());
              }
            }
            break;

          case "BookingNotes":
            if (Convert.ToInt32(parentID) > 0) {
              BookingNotesLibrary BookingNotesLibrary = new BookingNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<BookingNotes> resultList = BookingNotesLibrary.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (BookingNotes BookingNote in resultList) {

                BookingNotesLibrary.Delete(BookingNote.ID.ToString());
              }
            }
            break;

          case "BookingAttachments":
            if (Convert.ToInt32(parentID) > 0) {
              BookingAttachmentsLibrary bookingAttachmentLibrary = new BookingAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<BookingAttachments> resultList = bookingAttachmentLibrary.GetAllByParentID(Convert.ToInt32(parentID));

              foreach (BookingAttachments bookingAttachment in resultList) {
                bookingAttachmentLibrary.Delete(bookingAttachment.ID.ToString());
              }
            }
            break;

          #endregion
        }


      }
    }

    [HttpPost]
    public override ActionResult _GetJSon(string id) {
      Booking entity = Library.GetByID(id.ToString(), _predicates);
      if (entity.Shipping_Company == null) {
        entity.Shipping_Company = new Party();
      }
      return Json(entity);
    }

    protected override void ValidateEntity(Booking entity) {
      ModelState.Clear();

      if (string.IsNullOrEmpty(entity.Booking_Ref_No))
        ModelState.AddModelError("Booking_Ref_No", "Booking Ref No is required");

      if (entity.Sales_Order_No.ID == 0)
        ModelState.AddModelError("Sales_Order_No", "Sales Order No is required");

    }

    #endregion Override Methods

    #region Public Methods

    public ActionResult _OpenBookings() {
      int totalRows = 0;
      IEnumerable<Booking> resultList = ((ILibrary<Booking>)Library).GetAllByPaging(out totalRows, 1, ConfigurationHelper.GetsmARTLookupGridPageSize(), "", "Asc");

      return View("~/Views/Transaction/Booking/_OpenBookings.cshtml", resultList);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenBookings(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Booking> resultList = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenBookings(
                                                                                                      out totalRows, command.Page,
                                                                                                      command.PageSize,
                                                                                                      "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party", "Shipping_Company" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _OpenBrokerageBookings(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Booking> resultList = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenBrokerageBookings(
                                                                                                      out totalRows, command.Page,
                                                                                                      command.PageSize,
                                                                                                      "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party", "Shipping_Company" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    public ActionResult _GetBooking(int id) {
      return new JsonResult {
        Data = Helpers.PartyHelper.GetPartyByID(id.ToString())
      };
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _InvoicePendingBooking(GridCommand command) {
      int totalRows = 0;
      BookingLibrary lib = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Booking> resultListTemp = lib.PendingInvoiceBookings(out totalRows, command.Page, (command.PageSize == 0 ? 20 : command.PageSize), "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party" }, (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      return View(new GridModel {
        Data = resultListTemp,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _InvoicePendingBrokerageTypBooking(GridCommand command) {
      int totalRows = 0;
      BookingLibrary lib = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Booking> resultListTemp = lib.PendingInvoiceBrokerageTypeBookings(out totalRows, command.Page, (command.PageSize == 0 ? 20 : command.PageSize), "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party" }, (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
      return View(new GridModel {
        Data = resultListTemp,
        Total = totalRows
      });
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _NonTradingOpenBookings(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Booking> resultList = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).NonTradingOpenBookings(
                                                                                                      out totalRows, command.Page,
                                                                                                      command.PageSize,
                                                                                                      "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party", "Shipping_Company" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }
    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _UnPaidExpBookings(GridCommand command, string partyId)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Booking> resultList = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetUnPaidExpBookingsByParty(int.Parse(partyId),
                                                                                                      out totalRows, command.Page,
                                                                                                      command.PageSize,
                                                                                                      "", "Asc", new string[] { "Sales_Order_No", "Sales_Order_No.Party", "Shipping_Company" },
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {Data = resultList,Total = totalRows});
    }
    #endregion
    #region Deleted
    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Index(GridCommand command)
    //{
    //    int totalRows = 0;
    //    IEnumerable<Booking> resultList = ((ILibrary<Booking>)Library).GetAllByPaging(
    //                                                    out totalRows,
    //                                                    command.Page,
    //                                                    command.PageSize,
    //                                                    "",
    //                                                    "Asc",
    //                                                    _predicates,
    //                                                    (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));


    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public ActionResult _OpenBookings(GridCommand command)
    //{
    //    int totalRows = 0;

    //    IEnumerable<Booking> resultList = new BookingLibrary(Configuration.GetsmARTDBContextConnectionString()).GetOpenBookings();

    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    //public override ActionResult _Index()
    //{
    //    int totalRows = 0;
    //    IEnumerable<Booking> resultList = new BookingLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenBookings(out totalRows, 1, 20, "", "Asc");

    //    return View("~/Views/Transaction/Booking/_List.cshtml", resultList);
    //}

    //[HttpPost]
    //public override ActionResult Save(Booking entity) {
    //  ModelState.Clear();
    //  // Need to find an easier way

    //  if (string.IsNullOrEmpty(entity.Booking_Ref_No))
    //    ModelState.AddModelError("Booking_Ref_No", "Booking Ref No is required");

    //  if (entity.Sales_Order_No.ID == 0)
    //    ModelState.AddModelError("Sales_Order_No", "Sales Order No is required");


    //  if (ModelState.IsValid) {
    //    if (entity.ID == 0) {
    //      entity = Library.Add(entity);

    //      //Also save all relevant child records in database
    //      if (ChildEntityList != null) {
    //        SaveChildEntities(ChildEntityList, entity);
    //        ClearChildEntities(ChildEntityList);
    //      }


    //    }
    //    else {
    //      Library.Modify(entity, new string[] { "Sales_Order_No", "Forwarder_Party_ID", "Shipping_Company", "Booking_Closed_By" });
    //    }
    //    ModelState.Clear();
    //  }
    //  else
    //    return Display(entity);

    //  return Display(entity.ID.ToString());
    //}

    //public override ActionResult Index(int? id) {
    //  if (id.HasValue) {
    //    return Display(id.ToString());
    //  }
    //  else
    //    return RedirectToAction("New");
    //}

    //protected ActionResult Display(string id) {
    //  Booking result = Library.GetByID(id, _predicates);
    //  return View("New", result);
    //}

    //protected override ActionResult Display(Booking entity) {
    //  return View("New", entity);
    //}
    #endregion Deleted
  }
}

