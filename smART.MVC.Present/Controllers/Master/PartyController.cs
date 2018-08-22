using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;
using Telerik.Web.Mvc;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Master_Party)]
  public class PartyController : BaseFormController<PartyLibrary, Party> {

    #region Constructor

    public PartyController()
      : base("~/Views/Master/Party/_List.cshtml", null, new string[] { "Contact", "AddressBook", "Bank", "Bin", "Note" }) {
    }

    #endregion Constructor

    #region Override Methods

    protected override void ValidateEntity(smART.ViewModel.Party entity) {
      if (string.IsNullOrEmpty(entity.Party_Name))
        ModelState.AddModelError("Name", "The Party Name field is required.");
      if (string.IsNullOrEmpty(entity.Party_Short_Name))
        ModelState.AddModelError("ShortName", "The Short Name field is required.");
      if (string.IsNullOrWhiteSpace(entity.Party_Type))
        ModelState.AddModelError("ShortName", "The Party Type Name field is required.");
      if (entity.ID == 0) {
        IEnumerable<AddressBook> resultList = (IList<AddressBook>)Session["AddressBook"];
        if (resultList == null || resultList.Count() <= 0)
          ModelState.AddModelError("Address", "The Party address is required.");
      }
      if (entity.Party_Type == "Individual" && string.IsNullOrEmpty(entity.License_No))
        ModelState.AddModelError("License", "License# is required.");
    }

    protected override void SaveChildEntities(string[] childEntityList, Party entity) {

      foreach (string ChildEntity in childEntityList) {
        switch (ChildEntity) {
          #region /* Case Statements - All child grids */
          case "AddressBook":
            if (Session[ChildEntity] != null) {
              ILibrary<AddressBook> contactLibrary = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AddressBook> resultList = (IList<AddressBook>)Session[ChildEntity];
              //if (resultList.Count() == 1 || resultList.Count(o => o.Primary_Flag == true) <= 0)
              //  resultList.FirstOrDefault().Primary_Flag = true;

              foreach (AddressBook contact in resultList) {
                contact.Party = new Party() {
                  ID = entity.ID
                };
                contactLibrary.Add(contact);
              }
            }
            break;

          case "Bank":
            if (Session[ChildEntity] != null) {
              ILibrary<Bank> contactLibrary = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Bank> resultList = (IList<Bank>)Session[ChildEntity];
              foreach (Bank contact in resultList) {
                contact.Party = new Party() {
                  ID = entity.ID
                };
                contactLibrary.Add(contact);
              }
            }
            break;

          case "Bin":
            if (Session[ChildEntity] != null) {
              ILibrary<Bin> contactLibrary = new BinLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Bin> resultList = (IList<Bin>)Session[ChildEntity];
              foreach (Bin contact in resultList) {
                contact.Party = new Party() {
                  ID = entity.ID
                };
                contactLibrary.Add(contact);
              }
            }
            break;

          case "Contact":
            if (Session[ChildEntity] != null) {
              ILibrary<Contact> contactLibrary = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Contact> resultList = (IList<Contact>)Session[ChildEntity];
              foreach (Contact contact in resultList) {
                contact.Party = new Party() {
                  ID = entity.ID
                };
                contactLibrary.Add(contact);
              }
            }
            break;

          case "Note":
            if (Session[ChildEntity] != null) {
              ILibrary<Note> contactLibrary = new NoteLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Note> resultList = (IList<Note>)Session[ChildEntity];
              foreach (Note contact in resultList) {
                contact.Party = new Party() {
                  ID = entity.ID
                };
                contactLibrary.Add(contact);
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

          case "AddressBook":
            if (Convert.ToInt32(parentID) > 0) {
              AddressBookLibrary library = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<AddressBook> resultList = library.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (AddressBook entity in resultList) {
                library.Delete(entity.ID.ToString());
              }
            }
            break;

          case "Bank":
            if (Convert.ToInt32(parentID) > 0) {
              BankLibrary library = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Bank> resultList = library.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (Bank entity in resultList) {
                library.Delete(entity.ID.ToString());
              }
            }
            break;

          case "Bin":
            if (Convert.ToInt32(parentID) > 0) {
              BinLibrary lib = new BinLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Bin> resultList = lib.GetAllByParentID(Convert.ToInt32(parentID));

              foreach (Bin bin in resultList) {
                lib.Delete(bin.ID.ToString());
              }
            }
            break;

          case "Contact":
            if (Convert.ToInt32(parentID) > 0) {
              ContactLibrary lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Contact> resultList = lib.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (Contact contact in resultList) {
                lib.Delete(contact.ID.ToString());
              }
            }
            break;
          case "Note":
            if (Convert.ToInt32(parentID) > 0) {
              NoteLibrary lib = new NoteLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
              IEnumerable<Note> resultList = lib.GetAllByParentID(Convert.ToInt32(parentID));
              foreach (Note note in resultList) {
                lib.Delete(note.ID.ToString());
              }
            }
            break;
          #endregion
        }


      }
    }

    #endregion Override Methods

    #region Public Methods

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _PartyByType(GridCommand command, string partytype) {
      int totalRows = 0;
      IEnumerable<Party> resultList = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByTypeWithPaging(partytype,
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", null,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _PartyByTypes(GridCommand command, string partyTypes)  //Need to improve this to correct totalrows value. after filter.
    {
      string[] partyTypesArr = partyTypes.Split(',');

      string partyType1 = (partyTypesArr.Length > 0 ? partyTypesArr[0] : "");
      string partyType2 = (partyTypesArr.Length > 1 ? partyTypesArr[1] : "");

      int totalRows = 0;
      IEnumerable<Party> resultList = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByTypesWithPaging(partyType1, partyType2,
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", null,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _PartyForPayment(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Party> resultList = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetPartiesForPaymentsWithPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", null,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _PartyForReceipt(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<Party> resultList = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetPartiesForReceiptWithPaging(
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", null,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    [HttpGet]
    public JsonResult _PartyById(string id) {
      int intID;
      bool isParse = int.TryParse(id, out intID);
      Party party = null;
      AddressBook address = null;

      if (isParse) {
        party = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id);
        address = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetPrimaryAddressesByPartyId(intID);
      }
      int partyId = 0;
      string partyName, photoRefId, thumb1RefId, thumb2RefId, signatureRefId, licenseRefId, cashCardRefId;
      partyName = photoRefId = thumb1RefId = thumb2RefId = signatureRefId = licenseRefId = cashCardRefId = "";
      string state = " ";
      string make = "";
      string model = "";
      string vehiclePlateNo = "";
      string vehicleYear = "";
      string vehicleSate = "";
      string color = "";

      string address1, city, country, zip, addressType, dob, acLicenseId, licenseNo;
      address1 = city = country = zip = addressType = dob = acLicenseId = licenseNo = "";

      if (party != null) {
        partyName = party.Party_Name;
        partyId = party.ID;
        state = party.State == null ? " " : party.State;
        dob = Convert.ToString(party.Party_DOB);
        acLicenseId = Convert.ToString(party.ACLicense_ID);
        licenseNo = Convert.ToString(party.License_No);
        photoRefId = Convert.ToString(party.PhotoRefId);
        thumb1RefId = Convert.ToString(party.ThumbImage1RefId);
        thumb2RefId = Convert.ToString(party.ThumbImage2RefId);
        signatureRefId = Convert.ToString(party.LicenseImageRefId);
        licenseRefId = Convert.ToString(party.SignatureImageRefId);
        cashCardRefId = Convert.ToString(party.CashCardImageRefId);

        Scale scale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetLastTicketByPartyId(party.ID, new string[] { "Party_ID" });
        if (scale != null) {
          make = scale.Make;
          model = scale.Model;
          vehiclePlateNo = scale.Vehicle_Plate_No;
          vehicleYear = scale.Vehicle_Year;
          vehicleSate = scale.Plate_State;
          color = scale.Color;
        }
      }
      if (address != null) {
        address1 = address.Address1;
        city = address.City;
        country = address.Country;
        zip = address.Zip_Code;
        addressType = address.Address_Type;
       // state = address.State == null ? " " : address.State;

      }
      var data = new {
        ID = partyId,
        Name = partyName,
        State = state,
        LicenseNo = licenseNo,
        Address1 = address1,
        City = city,
        Country = country,
        Zip = zip,
        AddressType = addressType,
        DOB = dob,
        ACLicenseId = acLicenseId,
        PhotoRefId = photoRefId,
        Thumb1RefId = thumb1RefId,
        Thumb2RefId = thumb2RefId,
        SignatureRefId = signatureRefId,
        LicenseRefId = licenseRefId,
        CashCardRefId = cashCardRefId,
        Make = make,
        Model = model,
        VehiclePlateNo = vehiclePlateNo,
        VehicleYear = vehicleYear,
        Color = color,
        VehicleLicenseState = vehicleSate
      };
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult _PartySearchById(string id) {
      int intID;
      bool isParse = int.TryParse(id, out intID);
      Party party = null;


      if (!string.IsNullOrEmpty(id) && isParse) {
        party = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id);
      }
      int partyId = 0;
      string partyName = "";
      string licenseNo = "";

      if (party != null) {
        partyName = party.Party_Name;
        partyId = party.ID;
        licenseNo = Convert.ToString(party.License_No);
      }

      var data = new {
        ID = partyId,
        Name = partyName,
        LicenseNo = licenseNo
      };
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    //public Party SaveParty(string licenseNo, string name,string state,string loginUser,string imageRefId) {
    public Party SaveParty(Party party, AddressBook address) {
      Party entity = null;
      // Save Party 
      if (party != null) {
        PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        entity = partyLib.GetByID(party.ID.ToString());
        if (entity == null || entity.ID <= 0)
          entity = new Party();
        entity.License_No = party.License_No;
        entity.Party_Name = party.Party_Name;
        entity.Party_Short_Name = party.Party_Short_Name;
        entity.State = party.State;
        entity.Created_By = party.Created_By;
        entity.Created_Date = DateTime.Now;
        entity.Active_Ind = true;
        entity.IsActive = true;
        entity.Updated_By = party.Updated_By;
        entity.Last_Updated_Date = DateTime.Now;
        entity.Party_Type = "Individual";
        entity.LicenseImageRefId = party.LicenseImageRefId;
        entity.Party_DOB = party.Party_DOB;
        entity.ACLicense_ID = party.ACLicense_ID;
        entity.ThumbImage1RefId = party.ThumbImage1RefId;
        entity.ThumbImage2RefId = party.ThumbImage2RefId;
        entity.PhotoRefId = party.PhotoRefId;
        entity.SignatureImageRefId = party.SignatureImageRefId;
        entity.VehicleImageRegId = party.VehicleImageRegId;
        entity.CashCardImageRefId = party.CashCardImageRefId;

        if (entity == null || entity.ID <= 0)
          entity = partyLib.Add(entity);
        else
          entity = partyLib.Modify(entity);


        // Save Address
        if (address != null) {
          AddressBookLibrary addressLib = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          AddressBook addressBook = addressLib.GetPrimaryAddressesByPartyId(entity.ID);
          if (addressBook == null || addressBook.ID <= 0)
            addressBook = new AddressBook();

          addressBook.Party = entity;
          addressBook.Address1 = address.Address1;
          addressBook.City = address.City;
          addressBook.State = party.State;
          addressBook.Zip_Code = address.Zip_Code;
          addressBook.Country = address.Country;
          addressBook.Primary_Flag = true;
          addressBook.Address_Type = address.Address_Type;

          addressBook.Created_By = party.Created_By;
          addressBook.Created_Date = DateTime.Now;
          addressBook.Active_Ind = true;
          addressBook.Updated_By = party.Updated_By;
          addressBook.Last_Updated_Date = DateTime.Now;

          if (addressBook == null || addressBook.ID <= 0)
            addressLib.Add(addressBook);
          else
            addressLib.Modify(addressBook);
        }
      }
      return entity;
    }

    #endregion Public Methods

    #region Deleted
    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public ActionResult _PartyByTypes(GridCommand command, string partyTypes)  //Need to improve this to correct totalrows value. after filter.
    //{
    //    string[] partyTypesArr = partyTypes.Split(',');
    //    int totalRows = 0;

    //    //IEnumerable<Party> resultList = ((ILibrary<Party>)Library).GetAllByPaging(out totalRows, command.Page, (command.PageSize == 0 ? 20 : command.PageSize), "", "Asc", null, (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
    //    IEnumerable<Party> resultList = ((ILibrary<Party>)Library).GetAll();
    //    if (partyTypesArr != null)
    //    {
    //        resultList = resultList.Where(o => partyTypesArr.Contains(o.Party_Type));

    //    }
    //    return View(new GridModel { Data = resultList, Total = totalRows });
    //}

    //public override ActionResult Index(int? id) {
    //  if (id.HasValue) {
    //    return Display(id.ToString());
    //  }
    //  else
    //    return RedirectToAction("New");
    //}

    //protected ActionResult Display(string id) {
    //  Party result = Library.GetByID(id);

    //  return View("New", result);
    //}

    //[HttpPost]
    //public override ActionResult Save(Party entity) {
    //  try {
    //    ModelState.Clear();
    //    ValidateEntity(entity);

    //    if (ModelState.IsValid) {
    //      if (entity.ID == 0) {
    //        entity = Library.Add(entity);

    //        // Also save all relevant child records in database
    //        if (ChildEntityList != null) {
    //          SaveChildEntities(ChildEntityList, entity);
    //          ClearChildEntities(ChildEntityList);
    //        }
    //      }
    //      else {
    //        Library.Modify(entity);
    //      }
    //      ModelState.Clear();
    //    }
    //    else
    //      return Display(entity);
    //  }
    //  catch (Exception ex) {
    //   ModelState.AddModelError("Error",ex.Message);
    //   return Display(entity);
    //  }
    //  return Display(entity.ID.ToString());
    //}

    //public bool IsDuplicateParty(Party entity) {
    //  PartyLibrary partyLibrary = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    //  return partyLibrary.IsDuplicate(entity);
    //}
    #endregion Deleted

  }
}
