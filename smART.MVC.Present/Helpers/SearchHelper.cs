using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using System.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Helpers {

  public class SearchHelper {

    public static IEnumerable<FocusArea> GetFocusArea() {
      ILibrary<FocusArea> lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<FocusArea> result = lib.GetAll();
      return result;
    }

    public static FocusArea GetFocusArea(string id) {
      ILibrary<FocusArea> lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      FocusArea result = lib.GetByID(id);
      return result;
    }

    public static IEnumerable<FocusArea> GetFocusAreaByType(string type) {
      FocusAreaLibrary lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      IEnumerable<FocusArea> result = lib.GetByType(type);
      return result;
    }

    public static FocusArea GetFocusAreaByViewName(string viewName) {
      FocusAreaLibrary lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      FocusArea result = lib.GetByViewName(viewName);
      return result;
    }

    public static IList<Telerik.Web.Mvc.UI.TreeViewItem> FocusAreaTreeViewItems(string type) {
      IList<Telerik.Web.Mvc.UI.TreeViewItem> focusAreaTreeViewItems = new List<Telerik.Web.Mvc.UI.TreeViewItem>();

      IEnumerable<FocusArea> focusAreas = GetFocusAreaByType(type);

      var distinctFocusAreas = from f in focusAreas.GroupBy(g => g.Focus_Area)
                               select f.FirstOrDefault();

      foreach (var focusArea in distinctFocusAreas) {
        Telerik.Web.Mvc.UI.TreeViewItem parentNode = new Telerik.Web.Mvc.UI.TreeViewItem() {       
          Enabled = false,
          Expanded = true,          
          Text = focusArea.Focus_Area,
          Value = focusArea.ID.ToString(),
        };

        var subFocusAreas = from sf in focusAreas
                            where sf.Focus_Area == focusArea.Focus_Area
                            select sf;
        foreach (var subFocusArea in subFocusAreas) {
          Telerik.Web.Mvc.UI.TreeViewItem childNode = new Telerik.Web.Mvc.UI.TreeViewItem() {
            Enabled = true,
            Expanded = true,
            Text = subFocusArea.Sub_Focus_Area,
            Value = subFocusArea.ID.ToString(),
          };
          parentNode.Items.Add(childNode);
        }
        focusAreaTreeViewItems.Add(parentNode);

      }
      return focusAreaTreeViewItems;
    }

    public static IList<Telerik.Web.Mvc.UI.TreeViewItem> FocusAreaTreeViewItems() {
      IList<Telerik.Web.Mvc.UI.TreeViewItem> focusAreaTreeViewItems = new List<Telerik.Web.Mvc.UI.TreeViewItem>();

      IEnumerable<FocusArea> focusAreas = GetFocusArea();

      var distinctFocusAreas = from f in focusAreas.GroupBy(g => g.Focus_Area) select f.FirstOrDefault();

      foreach (var focusArea in distinctFocusAreas) {
        Telerik.Web.Mvc.UI.TreeViewItem parentNode = new Telerik.Web.Mvc.UI.TreeViewItem() {
          //ActionName = "Search",
          //ControllerName = "GlobalSearch",
          Enabled = false,
          Expanded = true,
          //LoadOnDemand = true,
          Text = focusArea.Focus_Area,
          Value = focusArea.View_Name,
          
          //Url = "/GlobalSearch/Search?viewName=" + focusArea.View_Name
        };

        var subFocusAreas = from sf in focusAreas where sf.Focus_Area == focusArea.Focus_Area select sf;
        foreach (var subFocusArea in subFocusAreas) {
          Telerik.Web.Mvc.UI.TreeViewItem childNode = new Telerik.Web.Mvc.UI.TreeViewItem() {
            //ActionName = "Search",
            //ControllerName = "GlobalSearch",
            Enabled = true,
            Expanded = true,
            //LoadOnDemand = true,
            Text = subFocusArea.Sub_Focus_Area,
            Value = subFocusArea.View_Name,
            //Url = "/GlobalSearch/Search?viewName=" + subFocusArea.View_Name
          };
          parentNode.Items.Add(childNode);
        }
        focusAreaTreeViewItems.Add(parentNode);

      }
      return focusAreaTreeViewItems;
    }
  }
}