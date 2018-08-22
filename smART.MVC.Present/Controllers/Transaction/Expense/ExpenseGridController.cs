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

namespace smART.MVC.Present.Controllers {
  public abstract class ExpenseGridController<TLibrary, TEntity, TParentEntity> : BaseGridController<TLibrary, TEntity>
    where TLibrary : ILibrary<TEntity>, IParentChildLibrary<TEntity>, new()
    where TEntity : ExpensesRequest, new()
    where TParentEntity : BaseEntity, new() {
    #region /* Constructors */
    public ExpenseGridController(string sessionName, string[] includePredicates = null)
      : base(sessionName, new string[] { "Paid_Party_To", "Invoice", "Payment" }, new string[] { "Paid_Party_To", "Invoice", "Payment" }) {
    }

    public ExpenseGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null)
      : base(dbContextConnectionString, sessionName, new string[] { "Paid_Party_To", "Invoice", "Payment" }, new string[] { "Paid_Party_To", "Invoice", "Payment" }) {
    }
    #endregion

    #region /* Supporting Actions - Display Actions */
    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<TEntity> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Paid_Party_To", "Invoice", "Payment" });
        //resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize, "", "Asc", IncludePredicates);
      }

      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, TEntity entity, bool isNew = false) {
      //if (entity.Reference != null && entity.Reference.ID != 0)
      if (entity.Reference_ID != 0)
        return Display(command, entity.Reference_ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }


    [HttpPost]
    public virtual ActionResult GetByParentID(string id) {
      IEnumerable<TEntity> resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByParentID(int.Parse(id));
      SelectList list = new SelectList(resultList, "ListValue", "ListText");
      return Json(list);
    }

    #endregion

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(TEntity data, GridCommand command, bool isNew = false) {
    //  try {
    //    Validate(data);

    //    if (ModelState.IsValid) {
    //      if (isNew) {

    //        TempEntityList.Add(data);
    //      }
    //      else {
    //        data = Library.Add(data);
    //      }
    //    }
    //  }
    //  catch (Exception ex) {
    //    ModelState.AddModelError("Error", ex.Message);
    //  }

    //  return Display(command, data, isNew);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Update(TEntity data, GridCommand command, bool isNew = false) {
    //  try {
    //    Validate(data);
    //    if (ModelState.IsValid) {
    //      if (isNew) {
    //        //TODO: Add logic to update in memory data
    //        TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //      }
    //      else {
    //        Library.Modify(data);
    //      }
    //    }
    //  }
    //  catch (Exception ex) {
    //    ModelState.AddModelError("Error", ex.Message);
    //  }
    //  return Display(command, data, isNew);
    //}

    protected override void ValidateEntity(TEntity entity) {
      ModelState.Clear();

      if (string.IsNullOrWhiteSpace(entity.EXPENSE_TYPE)) {
        ModelState.AddModelError("EXPENSE_TYPE", "Expense type is required.");
      }

      if (string.IsNullOrWhiteSpace(entity.Paid_By)) {
        ModelState.AddModelError("Paid_By", "Paid By is required.");
      }

      if (entity.Amount_Paid == 0) {
        ModelState.AddModelError("Amount_Paid", "Approved amount is required.");
      }
    }
    //private void Validate(ExpensesRequest entity) {

    //  ModelState.Clear();

    //  if (string.IsNullOrWhiteSpace(entity.EXPENSE_TYPE)) {
    //    ModelState.AddModelError("EXPENSE_TYPE", "Expense type is required.");
    //  }

    //  if (string.IsNullOrWhiteSpace(entity.Paid_By)) {
    //    ModelState.AddModelError("Paid_By", "Paid By is required.");
    //  }

    //  if (entity.Amount_Paid == 0) {
    //    ModelState.AddModelError("Amount_Paid", "Approved amount is required.");
    //  }

    //}

    //#region /* Ajax Controller Actions */

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Update(TEntity data, GridCommand command, bool isNew = false)
    //{


    //    if (isNew)
    //    {
    //        //TODO: Add logic to update in memory data
    //        TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //    }
    //    else
    //    {
    //        data.Expense = System.Web.HttpUtility.HtmlDecode(data.Expense);

    //        Library.Modify(data);
    //    }

    //    return Display(command, data, isNew);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(TEntity data, GridCommand command, bool isNew = false)
    //{            


    //    if (isNew)
    //    {
    //        data.Expense = System.Web.HttpUtility.HtmlDecode(data.Expense);
    //        TempEntityList.Add(data);
    //    }
    //    else
    //    {
    //        data.Expense = System.Web.HttpUtility.HtmlDecode(data.Expense);
    //        data = Library.Add(data);
    //    }

    //    return Display(command, data, isNew);
    //}



    //#endregion

  }
}