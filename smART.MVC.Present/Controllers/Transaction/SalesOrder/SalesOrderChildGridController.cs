using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;

namespace smART.MVC.Present.Controllers
{
    public abstract class SalesOrderChildGridController<TLibrary, TEntity> : BaseGridController<TLibrary, TEntity>
        where TLibrary : ILibrary<TEntity>, ISalesOrderChildLibrary<TEntity>, new()
        where TEntity : SalesOrderChildEntity, new()
    {
        #region /* Constructors */
      public SalesOrderChildGridController(string sessionName, string[] includePredicates = null, string[] includeModifyPredicates = null)
        : base(sessionName, includePredicates, includeModifyPredicates)
        {
        }

      public SalesOrderChildGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null, string[] includeModifyPredicates = null)
        : base(dbContextConnectionString, sessionName, includePredicates, includeModifyPredicates)
        {
        }
        #endregion

        #region /* Supporting Actions - Display Actions */
        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<TEntity> resultList;    // = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

            if (isNew)
            {
                resultList = TempEntityList;
                totalRows = TempEntityList.Count;
            }
            else
            {
                resultList = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
                //resultList = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllByPagingBySalesOrderID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
            }

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override ActionResult Display(GridCommand command, TEntity entity, bool isNew = false)
        {
            if (entity.SalesOrder != null && entity.SalesOrder.ID != 0)
                return Display(command, entity.SalesOrder.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }

        #endregion

        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<TEntity> resultList = ((ISalesOrderChildLibrary<TEntity>)Library).GetAllBySalesOrderID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }
    }
}
