using smART.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using VModel = smART.ViewModel;
using System.IO;
using smART.ViewModel;
namespace smART.MVC.Service.Controllers
{
    public class DeviceSettingController : BaseController<DeviceSettingLibrary, DeviceSettings>
    {

        public DeviceSettingController()
        {
        }

        [HttpGet]
        [ActionName("GetSettingByDeviceID")]        
        public VModel.DeviceSettings GetSettingByDeviceID(string deviceID)
        {
            try
            {
                // string decDeviceID = Decrypt(deviceID);

                DeviceSettingLibrary lib = new DeviceSettingLibrary(base.ConString);
                return lib.GetByDeviceID(deviceID);
            }
            catch (Exception ex)
            {
                string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
                smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
                return null;
            }
        }

        [HttpPut]
        public void ModifyMaxTicketIDByDeviceID(string deviceID, int maxID)
        {
            try
            {
                DeviceSettingLibrary lib = new DeviceSettingLibrary(base.ConString);
                DeviceSettings deviceSetting = lib.GetByDeviceID(deviceID);
                deviceSetting.MaxTicket_ID = maxID;
                lib.UpdateMaxTicketID(d=>d.ID== deviceSetting.ID,deviceSetting);
            }
            catch (Exception ex)
            {
                string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateBankBalance", ex.Message, ex.StackTrace.ToString());
                smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
            }
        }

    }
}
