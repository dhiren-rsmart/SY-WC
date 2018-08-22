using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;
using System.Linq.Expressions;


namespace smART.Business.Rules
{

    public class Scale : BaseScale
    {

        public void Adding(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel)
        {
            modelEntity.Lead = false;
            if (!businessEntity.QScale)
                UpdateScaleNetWeight(businessEntity, modelEntity);
            cancel = false;
        }

        public void Modifying(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel)
        {           
            if (!businessEntity.QScale)
                UpdateScaleNetWeight(businessEntity, modelEntity);
            cancel = false;
        }

        public void Added(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext)
        {
            UpdateContainer(businessEntity, modelEntity, dbContext);
            //AuditLog(businessEntity, dbContext);
            //dbContext.SaveChanges();
        }

        public void Modified(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext)
        {
            UpdateContainer(businessEntity, modelEntity, dbContext);
            if (!businessEntity.QScale)
                UpdateScaleDetailNetWeight(modelEntity, dbContext);
            //Inventory(modelEntity, dbContext);
            //AuditLog(businessEntity, dbContext);
            //dbContext.SaveChanges();
        }

        public void GotSingle(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext)
        {
            if (businessEntity != null)
            {
                if (businessEntity.Ticket_Type.Equals("Brokerage", StringComparison.InvariantCultureIgnoreCase))
                {
                    businessEntity.Brokerage_Party = businessEntity.Party_ID;
                    businessEntity.Brokerage_Purchase_Order = businessEntity.Purchase_Order;
                }
                if (businessEntity.QScale)
                {
                    IEnumerable<smART.Model.ScaleDetails> details = dbContext.T_Scale_Details.Where(s => s.Scale.ID == businessEntity.ID);
                    if (details != null)
                        businessEntity.Item_Amount = details.Sum(s => s.NetWeight * s.Rate);
                }
            }
        }

        public void Inventory(smART.Model.Scale modelScale, smART.Model.smARTDBContext dbContext)
        {
            try
            {
                IQueryable<smART.Model.ScaleDetails> scaleDetails = dbContext.Set<smART.Model.ScaleDetails>().AsQueryable().Where(o => o.Scale.ID == modelScale.ID);

                if (scaleDetails != null)
                {

                    scaleDetails = scaleDetails.Include("Apply_To_Item");

                    foreach (var modelEntity in scaleDetails)
                    {
                        modelEntity.Old_Net_Weight = modelEntity.NetWeight;
                        modelEntity.GrossWeight = modelScale.Gross_Weight * (modelEntity.Split_Value / 100);
                        modelEntity.TareWeight = modelScale.Tare_Weight * (modelEntity.Split_Value / 100);
                        modelEntity.NetWeight = modelEntity.GrossWeight - modelEntity.TareWeight - modelEntity.Contamination_Weight + modelEntity.Settlement_Diff_NetWeight;

                        if (modelEntity.Scale != null && modelEntity.Scale.Ticket_Status.ToLower().Equals("close") && modelEntity.Apply_To_Item != null)
                        {
                            AddInventory(modelEntity, dbContext);
                        }

                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelScale.Updated_By, modelScale.GetType().Name, modelScale.ID.ToString());
                if (rethrow)
                    throw ex;
            }

        }

        public void UpdateScaleNetWeight(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity)
        {
            modelEntity.Net_Weight = modelEntity.Gross_Weight - modelEntity.Tare_Weight + modelEntity.Settlement_Diff_NetWeight;
        }

        public void UpdateScaleDetailNetWeight(smART.Model.Scale modelScale, smART.Model.smARTDBContext dbContext)
        {
            try
            {
                IEnumerable<smART.Model.ScaleDetails> scaleDetails = from scaledetail in dbContext.T_Scale_Details
                                                                     where scaledetail.Scale.ID == modelScale.ID
                                                                     select scaledetail;

                if (scaleDetails != null)
                {
                    foreach (var modelEntity in scaleDetails)
                    {
                        modelEntity.GrossWeight = modelScale.Gross_Weight * (modelEntity.Split_Value / 100);
                        modelEntity.TareWeight = modelScale.Tare_Weight * (modelEntity.Split_Value / 100);
                        modelEntity.NetWeight = modelEntity.GrossWeight - modelEntity.TareWeight - modelEntity.Contamination_Weight + modelEntity.Settlement_Diff_NetWeight;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelScale.Updated_By, modelScale.GetType().Name, modelScale.ID.ToString());
                if (rethrow)
                    throw ex;
            }

        }


        public void UpdateContainer(smART.ViewModel.Scale businessEntity, smART.Model.Scale modelEntity, smART.Model.smARTDBContext dbContext)
        {
            try
            {
                if (modelEntity.Ticket_Type.ToLower() == "shipping ticket")
                {
                    smART.Model.Container container = dbContext.T_Container_Ref.Include("Booking").FirstOrDefault(m => m.ID == modelEntity.Container_No.ID);
                    container.Seal1_No = modelEntity.Seal_No;
                    container.Chasis_No = modelEntity.Trailer_Chasis_No;
                    container.Gross_Weight = modelEntity.Gross_Weight;
                    container.Tare_Weight = modelEntity.Tare_Weight;
                    container.Net_Weight = modelEntity.Net_Weight;
                    container.Status = GetContainerStatus(modelEntity, container, dbContext);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
                if (rethrow)
                    throw ex;
            }
        }

        public string GetContainerStatus(smART.Model.Scale modelEntity, smART.Model.Container container, smART.Model.smARTDBContext dbContext)
        {
            string status = string.Empty;
            try
            {
                Model.Booking modBooking = container.Booking;
                smART.Model.DispatcherRequest modDispatcher = dbContext.T_Dispatcher.FirstOrDefault(m => m.Booking_Ref_No.ID == container.Booking.ID);

                if (modBooking != null && modBooking.Invoice_Generated_Flag == true)
                {
                    status = "Closed";
                }
                else if (modelEntity != null && modelEntity.Ticket_Status.ToLower() == "open")
                {
                    status = "WIP";
                }
                else if (modelEntity != null && modelEntity.Ticket_Status.ToLower() == "close")
                {
                    if (modDispatcher != null && modDispatcher.RequestType.ToLower() == "drop off only")
                    {
                        status = "Shipped";
                    }
                    else if (container.Status.ToLower() == "wip" || container.Status.ToLower() == "open-empty")
                    {
                        status = "Open-Loaded";
                    }
                    else
                        status = container.Status;

                }
                else if (modDispatcher != null && modDispatcher.RequestType.ToLower() == "pickup only")
                {
                    status = "Open-Empty";
                }
                else if (modDispatcher != null && modDispatcher.RequestType.ToLower() == "drop off only")
                {
                    if (modelEntity != null)
                    {
                        status = "Shipped";
                    }
                    else
                    {
                        status = "Open-Empty";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, modelEntity.Updated_By, modelEntity.GetType().Name, modelEntity.ID.ToString());
                if (rethrow)
                    throw ex;
            }

            return status;
        }

        private void AuditLog(smART.ViewModel.Scale businessEntity, smART.Model.smARTDBContext dbContext)
        {
            try
            {
                smART.Model.AuditLog newModObject = new Model.AuditLog()
                {
                    Entity_Name = "Scale",
                    Entity_ID = businessEntity.ID,
                    Field_Name = "Scale_Reading",
                    Old_Value = businessEntity.Scale_Reading.ToString(),
                    New_Value = businessEntity.Scale_Reading.ToString(),
                    Active_Ind = true,
                    Created_By = businessEntity.Updated_By,
                    Updated_By = businessEntity.Updated_By,
                    Created_Date = businessEntity.Last_Updated_Date,
                    Last_Updated_Date = businessEntity.Last_Updated_Date
                };
                dbContext.T_Audit_Log.Add(newModObject);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = BusinessRuleExceptionHandler.HandleException(ref ex, businessEntity.Updated_By, businessEntity.GetType().Name, businessEntity.ID.ToString());
                if (rethrow)
                    throw ex;
            }
        }

    }
}
