using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;
using Omu.ValueInjecter;

namespace smART.MVC.Present.Controllers
{
    public abstract class NotesGridController<TLibrary, TEntity, TParentEntity> : BaseGridController<TLibrary, TEntity>
        where TLibrary : ILibrary<TEntity>, IParentChildLibrary<TEntity>, new()
        where TEntity : NotesEntity<TParentEntity>, new()
        where TParentEntity : BaseEntity, new()
    {
        #region /* Constructors */
        public NotesGridController(string sessionName, string[] includePredicates = null)
            : base(sessionName, includePredicates)
        {
        }

        public NotesGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null)
            : base(dbContextConnectionString, sessionName, includePredicates)
        {
        }
        #endregion

        #region /* Supporting Actions - Display Actions */
        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<TEntity> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

            if (isNew || id == "0")
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
            if (entity.Parent != null && entity.Parent.ID != 0)
                return Display(command, entity.Parent.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }


        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<TEntity> resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByParentID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }

        #endregion

    }
}