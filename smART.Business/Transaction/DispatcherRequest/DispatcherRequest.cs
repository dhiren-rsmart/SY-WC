using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules {

  public class DispatcherRequest {

    public void Added(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      //UpdateContainer(businessEntity, modelEntity, dbContext);
      UpdateAssetAuditLog(businessEntity, modelEntity, dbContext);
    }

    public void Modified(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      //UpdateContainer(businessEntity, modelEntity, dbContext);
      UpdateAssetAuditLog(businessEntity, modelEntity, dbContext);
    }

    public void Adding(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateContainer(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    public void Modifying(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel) {
      UpdateContainer(businessEntity, modelEntity, dbContext);
      cancel = false;
    }

    public void GotSingle(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.Booking_Ref_No != null) {
        businessEntity.Sales_Order_No = businessEntity.Booking_Ref_No.Sales_Order_No;
        businessEntity.Shipper = businessEntity.Booking_Ref_No.Shipping_Company;
        businessEntity.Container_No = businessEntity.Container != null ? businessEntity.Container.Container_No : businessEntity.Container_No;
      }
    }

    public void UpdateContainer(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      try {
        if (modelEntity.RequestCategory.ToLower() == "container" && !string.IsNullOrEmpty(businessEntity.Container_No)) {
          Model.Booking modBooking = dbContext.T_Booking_Ref.FirstOrDefault(o => o.ID.Equals(modelEntity.Booking_Ref_No.ID));
          if (modBooking != null) {
            Model.Container modContainer = dbContext.T_Container_Ref.Include("Booking").FirstOrDefault(o => o.Container_No == businessEntity.Container_No && o.Active_Ind == true);
            if (modContainer == null) {
              smART.Model.Container modNewcontainer = new smART.Model.Container();
              modNewcontainer.Booking = modBooking;
              modNewcontainer.Container_No = businessEntity.Container_No;
              modNewcontainer.Created_Date = modelEntity.Time;
              modNewcontainer.Last_Updated_Date = modelEntity.Time;
              modNewcontainer.Created_By = modelEntity.Created_By;
              modNewcontainer.Updated_By = modelEntity.Updated_By;
              modNewcontainer.Date_In = DateTime.Now;
              modNewcontainer.Status = GetContainerStatus(modBooking, modelEntity, dbContext);
              modNewcontainer.Active_Ind = true;
              Model.Container insertedObject = dbContext.T_Container_Ref.Add(modNewcontainer);
              modContainer = insertedObject;
              dbContext.SaveChanges();
              modelEntity.Container = modContainer;
            }
            else {
              modContainer.Last_Updated_Date = DateTime.Now;
              modContainer.Updated_By = modelEntity.Updated_By;
              modelEntity.Container = modContainer;
              modelEntity.Booking_Ref_No = dbContext.T_Booking_Ref.FirstOrDefault(o => o.ID == modContainer.Booking.ID);
              modContainer.Status = GetContainerStatus(modelEntity.Booking_Ref_No, modelEntity, dbContext);
              dbContext.SaveChanges();
            }    
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }

    }

    public string GetContainerStatus(smART.Model.Booking modBooking, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      string status = string.Empty;
      try {
        smART.Model.Scale modScale = modelEntity.Container != null ?  dbContext.T_Scale.FirstOrDefault(s => s.Container_No.ID == modelEntity.Container.ID && s.Container_No.Active_Ind == true) : null;

        if (modBooking.Invoice_Generated_Flag == true) {
          status = "Closed";
        }
        else if (modScale != null && modScale.Ticket_Status.ToLower() == "open") {
          status = "WIP";
        }
        else if (modScale != null && modScale.Ticket_Status.ToLower() == "close") {
          if (modelEntity.RequestType.ToLower() == "drop off only") {
            status = "Shipped";
          }

          else {
            status = "Open-Loaded";
          }

        }
        else if (string.IsNullOrWhiteSpace(modelEntity.RequestType) || modelEntity.RequestType.ToLower() == "pickup only") {
          status = "Open-Empty";
        }
        else if (modelEntity.RequestType.ToLower() == "drop off only") {
          if (modScale != null) {
            status = "Shipped";
          }
          else {
            status = "Open-Empty";
          }
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, modelEntity.ID.ToString());
        if (rethrow)
          throw ex;
      }
      return status;
    }

    public void UpdateAssetAuditLog(smART.ViewModel.DispatcherRequest businessEntity, smART.Model.DispatcherRequest modelEntity, smART.Model.smARTDBContext dbContext) {
      if (businessEntity.RequestCategory.ToLower().Equals("bin") && businessEntity.RequestStatus.ToLower() == "close") {
        smART.Model.AssetAudit modelAssetAudit = new Model.AssetAudit();
        modelAssetAudit.Dispatcher_Request = dbContext.T_Dispatcher.First(o => o.ID == modelEntity.ID);
        modelAssetAudit.Asset_Current_Location_Flg = true;
        modelAssetAudit.Asset = modelEntity.Asset;
        modelAssetAudit.Active_Ind = true;
        modelAssetAudit.Created_By = modelEntity.Updated_By;
        modelAssetAudit.Updated_By = modelEntity.Updated_By;
        modelAssetAudit.Created_Date = modelEntity.Last_Updated_Date;
        modelAssetAudit.Last_Updated_Date = modelEntity.Last_Updated_Date;
        modelAssetAudit.Date = modelEntity.Time;
        if (businessEntity.RequestType.ToLower() == "drop off only") {
          modelAssetAudit.Party = dbContext.M_Party.FirstOrDefault(o => o.ID == modelEntity.Party.ID);
          modelAssetAudit.Location = dbContext.M_Address.FirstOrDefault(o => o.ID == modelEntity.Location.ID);
          new smART.Business.Rules.AssetAudit().AddNewLocation(modelAssetAudit, dbContext);
        }
        else if (businessEntity.RequestType.ToLower() == "pickup only") {
          Model.Party modelParty = dbContext.M_Party.Where(o => o.Party_Type.ToLower() == "organization").FirstOrDefault();
          Model.AddressBook modelAddress = dbContext.M_Address.Where(o => o.Party.ID == modelParty.ID && o.Primary_Flag == true).FirstOrDefault();
          modelAssetAudit.Party = modelParty;
          modelAssetAudit.Location = modelAddress;
          new smART.Business.Rules.AssetAudit().AddNewLocation(modelAssetAudit, dbContext);
        }
      }

    }
  }
}
