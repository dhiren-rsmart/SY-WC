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
    public abstract class PriceListChildGridController<TLibrary, TEntity> : BaseGridController<TLibrary, TEntity>
        where TLibrary : ILibrary<TEntity>, IParentChildLibrary<TEntity>, new()
        where TEntity : PriceListChildEntity, new()
    {
        #region /* Constructors */
        public PriceListChildGridController(string sessionName, string[] includePredicates = null)
        : base(sessionName, includePredicates, new string[] { "PriceList", "Item" })
        {
        }

        public PriceListChildGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null)
          : base(dbContextConnectionString, sessionName, includePredicates, new string[] { "PriceList", "Item" })
        {
        }
        #endregion

        #region /* Supporting Actions - Display Actions */
        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<TEntity> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

            if (isNew)
            {
                resultList = TempEntityList;
                totalRows = TempEntityList.Count;
            }
            else
            {
                resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
                //resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
            }

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override ActionResult Display(GridCommand command, TEntity entity, bool isNew = false)
        {
            if (entity.PriceList != null && entity.PriceList.ID != 0)
                return Display(command, entity.PriceList.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }
        
        #endregion

        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<TEntity> resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByParentID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }
    }
}
