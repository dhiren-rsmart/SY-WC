using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using smART.MVC.Present.Helpers;
using Omu.ValueInjecter;
using smART.Common;

namespace smART.MVC.Present.Controllers.Transaction {
  [Feature(EnumFeatures.Transaction_ScaleDetails)]
  public class ScaleDetailsController : BaseGridController<ScaleDetailsLibrary, ScaleDetails> {

    #region /* Constructors */

    public ScaleDetailsController()
      : base("ScaleDetails", new string[] { "Scale" }, new string[] { "Item_Received", "Apply_To_Item" }) {
    }

    #endregion


    #region /* Supporting Actions - Display Actions */

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Insert(ScaleDetails data, GridCommand command, bool isNew = false) {
    //  ModelState.Clear();
    //  // Set ApplyToItem to ItemReceived when ApplyToItem is null.
    //  if (data.Apply_To_Item.ID == 0) {
    //    data.Apply_To_Item = data.Item_Received;
    //  }

    //  // Validate the entity.
    //  Validate(data);

    //  if (ModelState.IsValid) {
    //    // Add new record
    //    if (isNew) {
    //      TempEntityList.Add(data);
    //    }
    //    // Modify record.
    //    else {
    //      data = Library.Add(data);
    //    }
    //  }
    //  return Display(command, data, isNew);
    //}

    //[HttpPost]
    //[GridAction(EnableCustomBinding = true)]
    //public override ActionResult _Update(ScaleDetails data, GridCommand command, bool isNew = false) {
    //  ModelState.Clear();

    //  // Validate entity.
    //  Validate(data);

    //  if (ModelState.IsValid) {
    //    if (isNew) {
    //      //TODO: Add logic to update in memory data
    //      TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
    //    }
    //    else {
    //      Library.Modify(data, new string[] { "Item_Received", "Apply_To_Item" });
    //    }
    //  }
    //  return Display(command, data, isNew);
    //}

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<ScaleDetails> resultList;
      
      if (isNew || id == "0") {      
        resultList = TempEntityList;
        totalRows = resultList.Count(); // TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<ScaleDetails>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", new string[] { "Scale","Item_Received", "Apply_To_Item" });

      }
      return View(new GridModel {
        Data = resultList,
        Total = totalRows
      });
    }

    protected override ActionResult Display(GridCommand command, ScaleDetails entity, bool isNew = false) {
      if (entity.Scale != null && entity.Scale.ID != 0)
        return Display(command, entity.Scale.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);
    }

    protected override void ValidateEntity(ScaleDetails entity) {

      ModelState.Clear();
      // Set ApplyToItem to ItemReceived when ApplyToItem is null.
      if (entity.Apply_To_Item.ID == 0) {
        entity.Apply_To_Item = entity.Item_Received;
      }

      if (entity.Item_Received == null || entity.Item_Received.ID == 0) {
        ModelState.AddModelError("ItemRecived", "Received Item is required");
      }

      if (entity.Apply_To_Item == null || entity.Apply_To_Item.ID == 0) {
        ModelState.AddModelError("ItemApply", "Apply To Item is required");
      }

      ValidateDuplicateItem(entity);

      ValidItem(entity);
    }

    public void ValidateDuplicateItem(ScaleDetails scaleDetails) {
      IEnumerable<ScaleDetails> resultList;

      if (scaleDetails.Scale.ID == 0) {
        resultList = (IList<ScaleDetails>)Session["ScaleDetails"];
      }
      else {
        ScaleDetailsLibrary ScaleDetailsLibrary = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        resultList = ScaleDetailsLibrary.GetAllByParentID(scaleDetails.Scale.ID, new string[] { "Item_Received", "Apply_To_Item" });
        resultList = from res in resultList where res.ID != scaleDetails.ID select res;
      }

      if (resultList != null && resultList.Count(i => i.Item_Received.ID == scaleDetails.Item_Received.ID && i.ID != scaleDetails.ID) > 0)
        ModelState.AddModelError("DuplicateItem", "Duplicate received item not allowed.");

      if (resultList != null && resultList.Count(i => i.Apply_To_Item.ID == scaleDetails.Apply_To_Item.ID && i.ID != scaleDetails.ID) > 0)
        ModelState.AddModelError("DuplicateItem", "Duplicate apply to inventory item not allowed.");
    }

    public void ValidItem(ScaleDetails entity) {
      if (entity != null && entity.Scale != null && entity.Scale.ID > 0) {
        Scale scale = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(entity.Scale.ID.ToString(), new string[] { "Purchase_Order","Sales_Order", "Container_No.Booking.Sales_Order_No" ,"Booking.Sales_Order_No" });

        // Receiving ticket.
        if (scale.Ticket_Type != null && new string[] { "receiving ticket", "trading" }.Any(s => s == scale.Ticket_Type.ToLower()))
          ValidatePOItem(scale, entity);

        // Shipping ticket.
        else if (scale.Ticket_Type != null && new string[] { "shipping ticket", "trading" }.Any(s => s == scale.Ticket_Type.ToLower()) && scale.Container_No != null && scale.Container_No.Booking != null && scale.Container_No.Booking.Sales_Order_No != null)
          ValidateSOItem(scale.Container_No.Booking.Sales_Order_No.ID, entity);

        // Local Sales.
        else if (scale.Ticket_Type != null && scale.Ticket_Type.ToLower().Contains("local sale") && scale.Sales_Order != null)
          ValidateSOItem(scale.Sales_Order.ID, entity);

        // Brokerage.
        else if (scale.Ticket_Type != null && scale.Ticket_Type.ToLower().Contains("brokerage")) {

          // Validate SO
          if (scale.Booking != null && scale.Booking.Sales_Order_No != null)
            ValidateSOItem(scale.Booking.Sales_Order_No.ID, entity);

          // Validate PO
          ValidatePOItem(scale, entity);
        }

      }
    }

    private void ValidateSOItem(int soId, ScaleDetails scaleItem) {
      if (soId > 0) {
        SalesOrderItemLibrary soItemLib = new SalesOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<SalesOrderItem> soItems = soItemLib.GetAllBySalesOrderID(soId, new string[] { "Item" });
        var isSOItem = (from i in soItems
                        where i.Item.ID == scaleItem.Item_Received.ID
                        select i).FirstOrDefault();
        if (isSOItem == null)
          ModelState.AddModelError("Item_Received", string.Format("Scale details item {0} mismatch to selected sales order items.", scaleItem.Item_Received.Short_Name));
      }
    }

    private void ValidatePOItem(Scale scale, ScaleDetails scaleItem) {
      if (scale.Purchase_Order != null && scale.Purchase_Order.ID > 0) {
        PurchaseOrderItemLibrary poItemLib = new PurchaseOrderItemLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        IEnumerable<PurchaseOrderItem> poItems = poItemLib.GetAllByParentID(scale.Purchase_Order.ID, new string[] { "Item" });
        var isPOItem = (from i in poItems
                        where i.Item.ID == scaleItem.Item_Received.ID
                        select i).FirstOrDefault();
        if (isPOItem == null)
          ModelState.AddModelError("Item_Received", string.Format("Scale details item {0} mismatch to selected purchase order items.", scaleItem.Item_Received.Short_Name));
      }
    }
     

    #endregion

  }
}
