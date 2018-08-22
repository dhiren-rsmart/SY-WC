using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using System.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Helpers {
  public class ContactHelper {
    public static SelectList ContactList(object selectedItem) {
      ILibrary<Contact> lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Contact> result = lib.GetAll();
      SelectList sList = new SelectList(result, "ID", "First_Name", selectedItem);

      return sList;
    }

    public static Contact GetAddressByID(string id) {
      int ID = 0;
      int.TryParse(id, out ID);

      ILibrary<Contact> lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetByID(ID.ToString());
    }

    public static IEnumerable<Contact> GetEmailContactsByPartyId(int partyId) {
      ContactLibrary lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Contact> result = lib.GetEmailContactsByPartyId(partyId);
      return result;
    }
  }
}