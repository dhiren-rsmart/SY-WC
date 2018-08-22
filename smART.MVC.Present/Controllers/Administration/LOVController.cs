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
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Master_LOV)]
    public class LOVController : BaseGridController<LOVLibrary, LOV>
    {

      public LOVController() : base("LOV", new string[] { "LOVType", "Parent" }, new string[] { "LOVType", "Parent" }) {
      }
                     

        #region /* Supporting Actions - Display Actions */

        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<LOV> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);            

            if (Convert.ToInt32(id)>0)
              ViewBag.Parent_Type_ID = id;

            if (isNew || id == "0")
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

        protected override ActionResult Display(GridCommand command, LOV entity, bool isNew = false)
        {
            if (entity.LOVType != null && entity.LOVType.ID != 0)
                return Display(command, entity.LOVType.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }
        
        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<LOV> resultList = Library.GetAllByParentID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }


        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _GetAllLOV(GridCommand command) {
          int totalRows = 0;
          IEnumerable<LOV> resultList = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetAllByPaging(
                                                                                                          out totalRows, command.Page,
                                                                                                          (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                          "", "Asc", new string[] { "LOVType" },
                                                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                          );
          return View(new GridModel {
            Data = resultList,
            Total = totalRows
          });
        }

        protected override void ValidateEntity(smART.ViewModel.LOV entity) {
          ModelState.Clear();
          if (string.IsNullOrEmpty(entity.LOV_Value))
            ModelState.AddModelError("LOV_Value", "LOV Value field is required.");
        }

        [HttpGet]
        public  JsonResult _GetByParentType(string parentType) {
         return Json(LOVHelper.LOVList("Model","",parentType), JsonRequestBehavior.AllowGet);
          
          //return Josn  {
          //  Data = new SelectList(LOVHelper.LOVList("Model", "", parentType).ToList(), "LOV_Value", "LOV_Display_Value")
          //};

        }

        #endregion

    }
}