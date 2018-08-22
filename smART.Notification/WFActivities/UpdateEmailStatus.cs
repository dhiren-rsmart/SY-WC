using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using smART.Common;
using smART.Library;
using smART.ViewModel;

namespace smART.Notification.WFActivities {

  public sealed class UpdateEmailStatus : CodeActivity {

    // Define an activity input argument of type string
    public InArgument<int> NotificationType { get; set; }

    // Define an activity input argument of type string
    public InArgument<string> EntityID { get; set; }

    // If your activity returns a value, derive from CodeActivity<TResult>
    // and return the value from the Execute method.
    protected override void Execute(CodeActivityContext context) {
      try {
        int entityType = (int)NotificationType.Get(context);
        if (entityType == (int)EnumNotificationEntity.Scale) {
          string conString = System.Configuration.ConfigurationManager.ConnectionStrings["smARTDBContext"].ConnectionString;
          ScaleLibrary scaleLib = new ScaleLibrary(conString);
          Scale scale = scaleLib.GetByID(EntityID.Get(context), new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No", "Party_Address", "Sales_Order", "Invoice" });
          scale.Send_Mail = true;
          scaleLib.Modify(scale, new string[] { "Dispatch_Request_No", "Party_ID", "Purchase_Order", "Container_No", "Party_Address", "Sales_Order", "Invoice" });
        }
      }
      catch (Exception ex) {
        ExceptionFormatter formater = new ExceptionFormatter();
        string formatedException = formater.Format("Send mail update log error", ex);
        Common.MessageLogger.Instance.LogMessage(ex, formatedException, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Critical, "WF Email Error", "Email");
        throw ex;
      }

    }
  }
}
