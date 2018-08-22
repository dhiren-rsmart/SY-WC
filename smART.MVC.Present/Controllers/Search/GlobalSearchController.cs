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
using System.Data;
using Telerik.Web.Mvc.UI;
using System.Linq.Expressions;
using System.Collections;
using System.IO;
using System.Text;

namespace smART.MVC.Present.Controllers {

  [Feature(EnumFeatures.Searches_Search)]
  public class GlobalSearchController : Controller {

    // Default method to bind global search view.
    public ActionResult Search() {
      IList<Telerik.Web.Mvc.UI.TreeViewItem> model = SearchHelper.FocusAreaTreeViewItems();
      return View("~/Views/Search/GlobalSearch.cshtml", model);
    }

    // A Method to return partial view for focus area.
    public ActionResult FocusArea() {
      IList<Telerik.Web.Mvc.UI.TreeViewItem> model = new List<Telerik.Web.Mvc.UI.TreeViewItem>();
      IEnumerable<FocusArea> focusAreas = Helpers.SearchHelper.GetFocusAreaByType("Search");

      var distinctFocusAreas = from f in focusAreas.GroupBy(g => g.Focus_Area) select f.FirstOrDefault();

      foreach (var focusArea in distinctFocusAreas) {
        Telerik.Web.Mvc.UI.TreeViewItem parentNode = new Telerik.Web.Mvc.UI.TreeViewItem() {
          //ActionName = "Search",
          //ControllerName = "GlobalSearch",
          Enabled = false,
          Expanded = false,
          //LoadOnDemand = true,
          Text = focusArea.Focus_Area,
          Value = focusArea.View_Name
          //Url = "/GlobalSearch/SearchContent?viewName=" + focusArea.View_Name
        };

        var subFocusAreas = from sf in focusAreas where sf.Focus_Area == focusArea.Focus_Area select sf;
        foreach (var subFocusArea in subFocusAreas) {
          bool select = subFocusArea.Sub_Focus_Area == "Party Master" ? true : false;
          Telerik.Web.Mvc.UI.TreeViewItem childNode = new Telerik.Web.Mvc.UI.TreeViewItem() {
            ActionName = "SearchContent",
            ControllerName = "GlobalSearch",
            Enabled = true,
            Expanded = true,
            Selected = true,
            //LoadOnDemand = true,
            Text = subFocusArea.Sub_Focus_Area,
            Value = subFocusArea.View_Name
            
            //HtmlAttributes(new { data_url = Url.Action("SearchContent", "GlobalSearch") }        
            //Url = "/GlobalSearch/SearchContent?viewName=" + subFocusArea.View_Name
          };
          parentNode.Items.Add(childNode);
        }
        model.Add(parentNode);

      }
      return PartialView("~/Views/Search/FocusArea.cshtml", model);
    }

    // The method to return partial view to bind search content.
    public ActionResult SearchContent(string viewName) {
      FocusAreaLibrary lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      FocusArea globalSearch=  lib.GetByViewName(viewName);
     string orderByClause = globalSearch!= null ? Convert.ToString(globalSearch.OrderBy_Clause) :string .Empty ;
     DataTable dt = lib.GetAllAsDt(viewName, orderByClause);
      //DataTable dt = lib.GetAllWithPagingAsDt(
      //                                        viewName,
      //                                        out  totalRows,
      //                                        1,
      //                                        ViewBag.PageSize,
      //                                        "",
      //                                        "Asc"
      //                                      );

      //ViewData["total"] = totalRows;
      //DataTableViewModel model = new DataTableViewModel();
      //model.Data = dt;
      //model.Columns = columns(dt);
      //model.Total = totalRows;
      ViewBag.PageSize = 18;
      return PartialView("~/Views/Search/SearchContent.cshtml", dt);
    }

    // Ajax method to call bind search content
    [GridAction(EnableCustomBinding = false)]
    public ActionResult _SearchContent(GridCommand command, string viewName) {
      FocusAreaLibrary lib = new FocusAreaLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      FocusArea globalSearch = lib.GetByViewName(viewName);
      string orderByClause = globalSearch != null ? Convert.ToString(globalSearch.OrderBy_Clause) : string.Empty;
      DataTable dt = lib.GetAllAsDt(viewName, orderByClause);

      //DataTable dt = lib.GetAllWithPagingAsDt(viewName,
      //                                          out totalRows,
      //                                          command.Page,
      //                                          command.PageSize,
      //                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
      //                                          command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
      //                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
      //                                        );

      //DataTableViewModel model = new DataTableViewModel();
      //model.Data = dt;
      //model.Columns = columns(dt);
      //model.Total = totalRows;
      //ViewBag.PageSize = 18;
      //ViewData["total"] = totalRows;
      ViewBag.Total = dt.Rows.Count;
      ViewBag.PageSize = 18;
      return View(new GridModel(dt));
    }

    #region Deleted

    //  public GridColumnSettings[] columns(DataTable dt) {
    //    GridColumnSettings[] columns = new[]{
    //        foreach (System.Data.DataColumn temp in dt.Columns) {

    //                                       //columns.Bound(temp.DataType, temp.ColumnName);

    //                                    } ;

    //    }
    //foreach (System.Data.DataColumn temp in dt.Columns) {

    //                                       //columns.Bound(temp.DataType, temp.ColumnName);

    //                                    }  

    //              {  
    //                  new GridColumnSettings {Member = "Number", ClientTemplate="<input type='checkbox' name='checkedRecords' value='<#= Number #>' />", Filterable=false, Groupable=false, Title=""},
    //                  new GridColumnSettings { Member = "Number", Title="Number"}, 
    //                  new GridColumnSettings {Member="Abr", ClientTemplate="<a href='SAMPLE/SAMPLE'><#= Abr #></a>", Title="Abr"},
    //                  new GridColumnSettings { Member = "Country",Title="Country"}, 
    //                  new GridColumnSettings {  Member = "Date", Title="Date", Format="{0:MM/dd/yy}" }
    //              };
    //    return columns;
    //  }

    public GridColumnSettings[] columns(DataTable dt) {
      List<GridColumnSettings> gCols = new List<GridColumnSettings>();

      foreach (DataColumn dc in dt.Columns) {
        gCols.Add(new GridColumnSettings { Member = dc.ColumnName, Title = dc.ColumnName });
      }

      return gCols.ToArray();

      //    GridColumnSettings[] columns = new[]  
      //              {  
      //                  //new GridColumnSettings {Member = "ID", ClientTemplate="<input type='checkbox' name='checkedRecords' value='<#= Number #>' />", Filterable=false, Groupable=false, Title=""},
      //                  new GridColumnSettings { Member = "ID", Title="ID"} ,
      //new GridColumnSettings { Member = "Created_By", Title="Created_By"} 
      //                  //new GridColumnSettings {Member="Abr", ClientTemplate="<a href='SAMPLE/SAMPLE'><#= Abr #></a>", Title="Abr"},
      //                  //new GridColumnSettings { Member = "Country",Title="Country"}, 
      //                  //new GridColumnSettings {  Member = "Date", Title="Date", Format="{0:MM/dd/yy}" }
      //              };
      //    return columns;
    }

    public ActionResult ExportExcel(int page, string orderBy, string filter, string viewName) {
      //GlobalSearchLibrary lib = new GlobalSearchLibrary(smART.MVC.Present.ConfigurationHelper.GetsmARTDBContextConnectionString());
      //IEnumerable orders = lib.GetAllByViewNameAsDt(viewName).AsEnumerable().ToGridModel(page, 10, orderBy, string.Empty, filter).Data;
      //MemoryStream output = new MemoryStream();
      //StreamWriter writer = new StreamWriter(output, Encoding.UTF8);
      //writer.Write("OrderID,");
      //writer.Write("ContactName,");
      //writer.Write("ShipAddress,");
      //writer.Write("OrderDate");
      //writer.WriteLine();
      //foreach (Order order in orders) {
      //  writer.Write(order.OrderID);
      //  writer.Write(",");
      //  writer.Write("\"");
      //  writer.Write(order.Customer.ContactName);
      //  writer.Write("\"");
      //  writer.Write(",");
      //  writer.Write("\"");
      //  writer.Write(order.ShipAddress);
      //  writer.Write("\"");
      //  writer.Write(",");
      //  writer.Write(order.OrderDate.Value.ToShortDateString());
      //  writer.WriteLine();
      //}
      //writer.Flush();
      //output.Position = 0;
      //return File(output, "text/comma-separated-values", "Orders.csv");
      return null;
    }

    #endregion

  }
}
