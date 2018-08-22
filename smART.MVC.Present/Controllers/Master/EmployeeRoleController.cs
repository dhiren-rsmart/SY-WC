using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Master_RoleFeature)]
    public class EmployeeRoleController : BaseGridController<EmployeeRoleLibrary, EmployeeRole>
    {

        public EmployeeRoleController() : base("EmployeeRole", new string[] { "Employee", "Role" }) { }


        #region /* Supporting Actions - Display Actions */

        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<EmployeeRole> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

            if (isNew || id=="0")
            {
                resultList = TempEntityList;
                totalRows = TempEntityList.Count;
            }
            else
            {
                resultList = (Library.GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates));
                //resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
            }

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override ActionResult Display(GridCommand command, EmployeeRole entity, bool isNew = false)
        {
            if (entity.Role != null && entity.Employee.ID != 0)
                return Display(command, entity.Employee.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }

        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<EmployeeRole> resultList = Library.GetAllByParentID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }

        #endregion

    }
}