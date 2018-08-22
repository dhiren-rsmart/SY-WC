using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;


using Model = smART.Model;
using VModel = smART.ViewModel;
using System.Data;

using smART.Library;
using System.Linq.Expressions;
using System;



namespace smART.Library
{
    public class DeviceSettingLibrary : GenericLibrary<VModel.DeviceSettings, Model.DeviceSettings>
    {
        public DeviceSettingLibrary() : base() { }
        public DeviceSettingLibrary(string dbContextConnectionString)
            : base(dbContextConnectionString)
        {

        }
        public VModel.DeviceSettings GetByDeviceID(string deviceID)
        {
            VModel.DeviceSettings vmodDeviceSetting = GetSingleByExpression(o => o.Device_ID == deviceID);
            return vmodDeviceSetting;
        }

        public VModel.DeviceSettings GetByUniueID(int uniqueID)
        {
            VModel.DeviceSettings vmodDeviceSetting = GetSingleByExpression(o => o.Unique_ID == uniqueID);
            return vmodDeviceSetting;
        }

        public VModel.DeviceSettings GetBySiteIdAndDeviceId(int siteId, string deviceId)
        {
            VModel.DeviceSettings vmodDeviceSetting = GetSingleByExpression(o => o.Site_Org_ID == siteId && o.Device_ID == deviceId);
            return vmodDeviceSetting;
        }

        public void UpdateMaxTicketID(Expression<Func<Model.DeviceSettings, bool>> predicate, VModel.DeviceSettings modObject, string[] includePredicate = null)
        {
            try
            {
                Model.DeviceSettings newModObject = Mapper.Map<VModel.DeviceSettings, Model.DeviceSettings>(modObject);
                _repository.Modify<Model.DeviceSettings>(predicate, newModObject, includePredicate);
                _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
                if (rethrow)
                    throw ex;
            }

        }
    }
}
