using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using System.Web.Mvc;
using smART.Common;
using System.Configuration;

namespace smART.MVC.Present.Helpers {
  public class PartyHelper {
      public static SelectList PartyList(object selectedItem)
      {
          Party selectDropDown = new Party()
          {
              ID = 0,
              Party_Name = " -- Select Value ---"
          };
          IEnumerable<Party> selectList = new Party[] { selectDropDown };

          ILibrary<Party> lib = new PartyLibrary( ConfigurationHelper.GetsmARTDBContextConnectionString());
          IEnumerable<Party> result = lib.GetAll();
          selectList = selectList.Concat<Party>(result);
          SelectList sList = new SelectList(selectList, "ID", "Party_Name", selectedItem);
          return sList;
      }

    public static SelectList PartyList(string PartyType, object selectedItem) {
      ILibrary<Party> lib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Party> result = ((PartyLibrary)lib).GetByType(PartyType);
      SelectList sList = new SelectList(result, "ID", "Party_Name", selectedItem);

      return sList;
    }

    public static SelectList IndividualPartyList(object selectedItem) {
      ILibrary<Party> lib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<Party> result = ((PartyLibrary) lib).GetByType("Individual");
      SelectList sList = new SelectList(result, "ID", "License_No", selectedItem);
      return sList;
    }


    public static SelectList IndividualReportPartyList(object selectedItem)
    {
        Party selectDropDown = new Party()
        {
            ID = 0,
            Party_Name = " -- Select Value ---"
        };
        IEnumerable<Party> selectList = new Party[] { selectDropDown };

        ILibrary<Party> lib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<Party> result = ((PartyLibrary)lib).GetByType("Individual");
        selectList = selectList.Concat<Party>(result);
        SelectList sList = new SelectList(selectList, "ID", "Party_Name", selectedItem);
        return sList;
    }

    public static Party GetPartyByID(string id) {
      int partyID = 0;
      int.TryParse(id, out partyID);

      ILibrary<Party> lib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetByID(partyID.ToString());
    }

    public static string GetOrganizationName() {
      PartyLibrary partyLib = new PartyLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      Party orgParty = partyLib.GetByType("Organization").FirstOrDefault();
      return orgParty.Party_Name;
    }
  }
}