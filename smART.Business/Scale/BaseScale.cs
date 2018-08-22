using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace smART.Business.Rules {

  public class BaseScale {

    internal virtual void AddInventory(smART.Model.ScaleDetails modelScaleDetails, smART.Model.smARTDBContext dbContext) {
      try {
        //If status is close then add inventory record
        if (modelScaleDetails.Scale != null && modelScaleDetails.Scale.Ticket_Status.ToLower().Equals("close") && modelScaleDetails.Apply_To_Item != null) {
          decimal qty = 0;
          decimal oldNetWeight = 0;
          decimal newCurrentBalance = 0;

          //Get all scale inventory item by ticket id & item id
          IQueryable<smART.Model.Inventory> inventories = dbContext.Set<smART.Model.Inventory>().AsQueryable().Where(i => i.Scale_Ticket_ID.ID == modelScaleDetails.Scale.ID
                                                                                  && i.Item_ID.ID == modelScaleDetails.Apply_To_Item.ID
                                                                                  && i.Trans_Type == "Scale ticket"
                                                                                  && i.Active_Ind == true);

          //Get current balance from Item Master
          smART.Model.Item item = dbContext.M_Item.FirstOrDefault(i => i.ID == modelScaleDetails.Apply_To_Item.ID);

          oldNetWeight = modelScaleDetails.Old_Net_Weight;

          if (item != null) {

            newCurrentBalance = item.Current_Balance;
            smART.Model.Inventory newInventory = new Model.Inventory();

            //If item inventory exists then add diffrence inventory entry 
            if (inventories != null && inventories.Count() >= 1) {
              //Check item inventory already exist with same scale ticket.
              smART.Model.Inventory inventory = inventories.FirstOrDefault();

              //Same item update with same qty.
              if (modelScaleDetails.NetWeight == oldNetWeight) {
                return;
              }

              if (modelScaleDetails.NetWeight > oldNetWeight) {
                newInventory.Impact = "Add";
                qty = modelScaleDetails.NetWeight - oldNetWeight;
                newCurrentBalance += qty;
              }
              else {
                newInventory.Impact = "Subtract";
                qty = oldNetWeight - modelScaleDetails.NetWeight;
                newCurrentBalance -= qty;
              }
            }

            //If item inventory not exists then add item inventory entry 
            else {
              qty = modelScaleDetails.NetWeight;

              if (modelScaleDetails.Scale.Ticket_Type.ToLower().Equals("receiving ticket")) {
                newInventory.Impact = "Add";
                newCurrentBalance += qty;
              }
              else {
                newInventory.Impact = "Subtract";
                newCurrentBalance -= qty;
              }
            }

            newInventory.Created_By = modelScaleDetails.Created_By;
            newInventory.Created_Date = modelScaleDetails.Created_Date;
            newInventory.Item_ID = modelScaleDetails.Apply_To_Item;
            newInventory.Party_ID = modelScaleDetails.Scale.Party_ID;
            newInventory.Quantity = qty;
            newInventory.Scale_Ticket_ID = modelScaleDetails.Scale;
            newInventory.Trans_Date = DateTime.Now;
            newInventory.Trans_Type = "Scale ticket";
            newInventory.Active_Ind = true;
            newInventory.Balance = newCurrentBalance;
            dbContext.T_Inventory.Add(newInventory);

            //Update Item current balance in Item Master
            item.Current_Balance = newCurrentBalance;
            item.Last_Updated_Date = DateTime.Now;
            item.Updated_By = modelScaleDetails.Updated_By;
            dbContext.Entry<smART.Model.Item>(item);
          }

        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelScaleDetails.Updated_By, modelScaleDetails.GetType().Name, modelScaleDetails.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    internal virtual void DeleteInventory(smART.Model.ScaleDetails modelScaleDetails, smART.Model.smARTDBContext dbContext) {
      try {
        //If status is close then add inventory record
        if (modelScaleDetails.Scale != null && modelScaleDetails.Scale.Ticket_Status.ToLower().Equals("close") && modelScaleDetails.Apply_To_Item != null) {

          //Get all scale inventory item by ticket id & item id
          IQueryable<smART.Model.Inventory> inventories = dbContext.Set<smART.Model.Inventory>().AsQueryable().Where(i => i.Scale_Ticket_ID.ID == modelScaleDetails.Scale.ID
                                                                                  && i.Item_ID.ID == modelScaleDetails.Apply_To_Item.ID
                                                                                  && i.Trans_Type == "Scale ticket"
                                                                                  && i.Active_Ind == true);

          //Get current balance from Item Master
          smART.Model.Item item = dbContext.M_Item.FirstOrDefault(i => i.ID == modelScaleDetails.Apply_To_Item.ID);

          if (item != null && inventories != null && inventories.Count() > 0) {
            foreach (var inventory in inventories) {
              // Update Invetory Active_Ind to false.
              inventory.Updated_By = modelScaleDetails.Updated_By;
              inventory.Last_Updated_Date = modelScaleDetails.Last_Updated_Date;
              inventory.Active_Ind = false;
              dbContext.Entry<smART.Model.Inventory>(inventory);
            }

            decimal itemCurrentBalance = item.Current_Balance;
            decimal scaleItemCurrentBalance = modelScaleDetails.NetWeight;

            if (modelScaleDetails.Scale.Ticket_Type.ToLower().Equals("receiving ticket")) {
              itemCurrentBalance -= scaleItemCurrentBalance;
            }
            else {
              itemCurrentBalance += scaleItemCurrentBalance;
            }

            //Update Item current balance in Item Master
            item.Current_Balance = itemCurrentBalance;
            item.Last_Updated_Date = DateTime.Now;
            item.Updated_By = modelScaleDetails.Updated_By;
            dbContext.Entry<smART.Model.Item>(item);
            dbContext.SaveChanges();
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelScaleDetails.Updated_By, modelScaleDetails.GetType().Name, modelScaleDetails.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

  }
}
