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
    public class ItemHelper
    {      

        public static IEnumerable<Item> ItemList() {
          ItemLibrary lib = new ItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
          IEnumerable<Item> result = lib.GetActiveItems().OrderBy(o => o.Short_Name);
          return result;
        }

        public static Item GeItemByID(string id)
        {
            int itemID = 0;
            int.TryParse(id, out itemID);

            ILibrary<Item> lib = new ItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetByID(itemID.ToString());
        }

        public static SelectList ItemList(object selectedItem) {
          Item selectItem = new Item() {
           ID  = 0,  Short_Name = " -- Select Value ---"
          };
          IEnumerable<Item> selectList = new Item[] { selectItem };          
          IEnumerable<Item> result =ItemList();
          selectList = selectList.Concat<Item>(result);
          SelectList sList = new SelectList(selectList, "ID", "Short_Name", selectedItem);
          return sList;
        }

     }
}