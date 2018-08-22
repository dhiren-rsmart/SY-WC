using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;
using Telerik.Web.Mvc;
using Omu.ValueInjecter;
using System.Transactions;

namespace smART.MVC.Present.Controllers.Master {

  [Feature(EnumFeatures.Master_AddressBook)]
  public class AddressBookController : PartyChildGridController<AddressBookLibrary, AddressBook> {


    public AddressBookController()
      : base("AddressBook", new string[] { "Party" }) {
    }


    [GridAction(EnableCustomBinding = true)]
    public ActionResult _PartyAddresses(GridCommand command, int partyId)  //Need to improve this to correct totalrows value. after filter.
    {
      int totalRows = 0;
      IEnumerable<AddressBook> resultList = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAddressesByPartyIdWithPaging(partyId,
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
    public override ActionResult _GetJSon(string id) {
      AddressBook entity = Library.GetByID(id.ToString());
      return Json(entity);
    }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
      AddressBook entity;

      if (isNew) {
        //TODO: Delete entity with id
        entity = TempEntityList.FirstOrDefault(m => m.ID == int.Parse(id));
      }
      else {
        entity = Library.GetByID(id);
      }

      if (entity.Primary_Flag == true)
        ModelState.AddModelError("delete", "Can't delete primary address");

      return base._Delete(id, command, MasterID, isNew);
    }


    protected override void ValidateEntity(AddressBook entity) {
      ModelState.Clear();
      if (string.IsNullOrEmpty(entity.Address1))
        ModelState.AddModelError("Address1", "The Address1 field is required.");
      if (string.IsNullOrEmpty(entity.City))
        ModelState.AddModelError("City", "The City field is required.");
      if (string.IsNullOrEmpty(entity.City))
        ModelState.AddModelError("State", "The State field is required.");
      if (string.IsNullOrEmpty(entity.Country))
        ModelState.AddModelError("Country", "The Country field is required.");
      if (string.IsNullOrEmpty(entity.Zip_Code))
        ModelState.AddModelError("Zip_Code", "The Zip_Code field is required.");
      ValidatePrimaryAddress(entity);
    }

    private void ValidatePrimaryAddress(AddressBook entity) {
      IEnumerable<AddressBook> resultList = null;
      if (entity.Primary_Flag == false) {
        if (entity.Party.ID > 0) {
          resultList = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(entity.Party.ID);
        }
        else {
          resultList = (IList<AddressBook>) Session["AddressBook"];
        }
        var oldPrimaryAdd = resultList == null ? null : resultList.Where(o => o.ID == entity.ID).FirstOrDefault();

        if (oldPrimaryAdd != null && oldPrimaryAdd.Primary_Flag == true && entity.Primary_Flag == false)
          ModelState.AddModelError("Primary", "Can't remove primary flag.");
      }
    }

    protected override void ChildGrid_OnAdding(AddressBook entity) {
      SetPrimaryAddress(entity);
    }

    protected override void ChildGrid_OnModifying(AddressBook entity) {
      SetPrimaryAddress(entity);
    }

    private void SetPrimaryAddress(AddressBook entity) {
      IEnumerable<AddressBook> resultList = null;
      if (entity.Party.ID > 0) {
        resultList = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByParentID(entity.Party.ID);
      }
      else {
        resultList = (IList<AddressBook>) Session["AddressBook"];
      }
      if (entity.Primary_Flag == true) {
        var oldPrimaryAdd = resultList == null ? null : resultList.Where(o => o.Primary_Flag == true && o.ID != entity.ID).FirstOrDefault();

        if (oldPrimaryAdd != null) {
          oldPrimaryAdd.Primary_Flag = false;
          if (entity.Party.ID > 0) Library.Modify(oldPrimaryAdd); else TempEntityList.SingleOrDefault(m => m.ID == oldPrimaryAdd.ID).InjectFrom(oldPrimaryAdd);
        }
      }
      else {
        var oldPrimaryAdd = resultList == null ? null : resultList.Where(o => o.Primary_Flag == true).FirstOrDefault();
        if (oldPrimaryAdd == null) {
          entity.Primary_Flag = true;
        }
      }

    }
  }
}