using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;
using Omu.ValueInjecter;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers.Master
{
    [Feature(EnumFeatures.Master_Employee)]
    public class EmployeeController : BaseFormController<EmployeeLibrary, Employee>
    {
        #region Constructor

        public EmployeeController() : base("~/Views/Master/Employee/_List.cshtml", null, new string[] { "EmployeeRole" }) { }

        #endregion Constructor

        #region Override Methods

        //public override ActionResult New(string id)
        //{
        //    // Create new object
        //    ClearChildEntities(ChildEntityList);
        //    Employee entity = new Employee();

        //    return Display(entity);
        //}  

        //[HttpPost]
        //public override ActionResult Save(Employee entity)
        //{
        //    ModelState.Clear();
        //    // Need to find an easier way
        //    if (string.IsNullOrEmpty(entity.User_ID))
        //        ModelState.AddModelError("User ID", "User ID is required");
        //    if (string.IsNullOrEmpty(entity.Emp_Name))
        //        ModelState.AddModelError("Emp Name", "User name is required");
        //    if (entity.ID == 0 && string.IsNullOrEmpty(entity.Password))
        //        ModelState.AddModelError("Password", "Password is required");
        //    if (entity.ID != 0 && string.IsNullOrEmpty(entity.Password))
        //    {
        //        Employee oldEntity = Library.GetByID(entity.ID.ToString());
        //        entity.Password = oldEntity.Password;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        if (entity.ID == 0)
        //        {
        //            entity = Library.Add(entity);

        //            // Also save all relevant child records in database
        //            if (ChildEntityList != null)
        //            {
        //                SaveChildEntities(ChildEntityList, entity);
        //                ClearChildEntities(ChildEntityList);
        //            }
        //        }
        //        else
        //        {
        //            Library.Modify(entity);
        //        }

        //        //return Display(entity);
        //        return RedirectToAction("Index", new { id = entity.ID });
        //    }
        //    else
        //    {
        //        ClearChildEntities(ChildEntityList);
        //        Employee entityx = new Employee();

        //        if (entity != null)
        //        {
        //            entityx.User_ID = entity.User_ID;
        //            entityx.Password = entity.Password;
        //            entityx.Emp_Name = entity.Emp_Name;
        //        };

        //        return Display(entityx);
        //    }
        //}

        //public override ActionResult Index(int? id)
        //{
        //    if (id.HasValue)
        //    {
        //        Employee entity = Library.GetByID(id.ToString());
        //        return Display(entity);
        //    }
        //    else
        //        return RedirectToAction("New");
        //}

        protected override void SaveChildEntities(string[] childEntityList, Employee entity)
        {
            foreach (string ChildEntity in childEntityList)
            {
                switch (ChildEntity)
                {
                    #region /* Case Statements - All child grids */

                    case "EmployeeRole":
                        if (Session[ChildEntity] != null)
                        {
                            EmployeeRoleLibrary rLibrary = new EmployeeRoleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                            IEnumerable<EmployeeRole> resultList = (IList<EmployeeRole>)Session[ChildEntity];
                            foreach (EmployeeRole data in resultList)
                            {
                                data.Employee = new Employee { ID = entity.ID };
                                rLibrary.Add(data);
                            }
                        }
                        break;

                    #endregion
                }
            }
        }

        public override ActionResult _Index()
        {
            int totalRows = 0;
            ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Employee> resultList = lib.GetAllByPaging(out totalRows, 1, ViewBag.PageSize, "", "Asc", _includeEntities);
            resultList = resultList.Where(e => e.User_ID.ToLower() != "admin");
            return View(_listViewUrl, resultList);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public override ActionResult _Index(GridCommand command)
        {
            int totalRows = 0;

            FilterDescriptor filterDesc = new FilterDescriptor("Active_Ind", FilterOperator.IsNotEqualTo, "false");
            command.FilterDescriptors.Add(filterDesc);
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Employee> resultList = lib.GetAllByPaging(out totalRows,
                                                                command.Page,
                                                                command.PageSize,
                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                _includeEntities,
                                                                (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
            resultList = resultList.Where(e => e.User_ID.ToLower() != "admin");
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override void ValidateEntity(Employee entity) {
          ModelState.Clear();
          // Need to find an easier way
          if (string.IsNullOrEmpty(entity.User_ID))
            ModelState.AddModelError("User ID", "User ID is required");
          if (string.IsNullOrEmpty(entity.Emp_Name))
            ModelState.AddModelError("Emp Name", "User name is required");
          if (entity.ID == 0 && string.IsNullOrEmpty(entity.Password))
            ModelState.AddModelError("Password", "Password is required");
          if (entity.ID != 0 && string.IsNullOrEmpty(entity.Password)) {
            Employee oldEntity = Library.GetByID(entity.ID.ToString());
            entity.Password = oldEntity.Password;
          }
        }

        #endregion Override Methods

        #region Deleted

        #endregion Deleted

    }
}
