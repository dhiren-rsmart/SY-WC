using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using System.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Helpers
{
    public class AddressBookHelper
    {
        public static SelectList AddressBookList(object selectedItem)
        {
            ILibrary<AddressBook> lib = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<AddressBook> result = lib.GetAll();
            SelectList sList = new SelectList(result, "ID", "Address1", selectedItem);

            return sList;
        }

        public static AddressBook GetAddressByID(string id)
        {
            int addressID = 0;
            int.TryParse(id, out addressID);

            ILibrary<AddressBook> lib = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetByID(addressID.ToString());
        }

    }
}