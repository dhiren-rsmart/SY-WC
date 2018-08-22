using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Activities;
using smART.Common;
using smART.Library;
using smART.Notification;
using smART.Notification.Workflow;
using System.Threading;

namespace smART.Integration.Email {

  public class NotificationHelper {

    public delegate void NotificationWFCompleted(WorkflowApplicationCompletedEventArgs e);

    // Start the notification workflow to send notification for provided list of events.
    public static void StartNotificationWF(string id, string partyName, string compName, string userName, NotificationDefinition notificationDef, string xslPath, NotificationWFCompleted handler,string bookingID="",string soID="") {

      IDictionary<string, object> input = new Dictionary<string, object>
        {                
          {"CustomerName", partyName },
          {"CompanyName", compName},
          {"UserName", userName},
          {"EntityID",id.ToString() },   
          {"NotificationDef", notificationDef},            
          {"XSLPath", xslPath},
          {"NotificationType", (int) EnumNotificationEntity.Scale},
          {"BookingID", bookingID},
          {"SOID", soID}
        };

      // Start workflow.
      StartWorkflow(input, handler);
    }

    //// This method called on notification workflow completion.
    //internal static void NotificationWFCompleted(WorkflowApplicationCompletedEventArgs e) {
    //  switch (e.CompletionState) {
    //    // If workflow complated without any error.
    //    case ActivityInstanceState.Closed:
    //      #if DEBUG
    //      Console.WriteLine("Workflow Completed.");
    //      #endif
    //    break;
    //    // If any error occurred during workflow execution, will be handled here.
    //    case ActivityInstanceState.Faulted:
    //      Exception ex = e.TerminationException;
    //      ExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error, "Error occured in Notification Workflow", "PresentPolicy");       
    //      break;
    //  }
    //}

    // Start notification workflow with provided input parameters.
    internal static void StartWorkflow(IDictionary<string, object> inputParam, NotificationWFCompleted handler) {
      // Initialize the notification workflow instance.
      NotificationWF notificationWF = new NotificationWF();

      // Initialize workflow application instance with notification workflow and 
      // with required input argument dictionary.
      WorkflowApplication wfApp = new WorkflowApplication(notificationWF, inputParam);
      wfApp.SynchronizationContext = SynchronizationContext.Current;
      // Attaching a method with workflow completed event.
      wfApp.Completed = new Action<WorkflowApplicationCompletedEventArgs>(handler);

      //// Start the workflow execution asynchronously.
      wfApp.Run();
    }

    // Retrieves the transformed content from the xslt template.
    internal static string XSLTransform(string xsltPath, string customerName, string scaleTicketNo, string companyName, string userName,string bookingID,string SOID) {

      string body;
      IDictionary<string, string> xsltParam = new Dictionary<string, string>();
      xsltParam.Add("CustomerName", customerName);
      xsltParam.Add("EntityID", scaleTicketNo);
      xsltParam.Add("CompanyName", companyName);
      xsltParam.Add("UserName", userName);
      xsltParam.Add("BookingID", bookingID);
      xsltParam.Add("SOID", SOID);

      XslCompiledTransform compiler = new XslCompiledTransform(true);
      XsltSettings xslSetting = new XsltSettings(true, true);

      XmlUrlResolver resolver = new XmlUrlResolver();
      compiler.Load(xsltPath);

      XmlDocument xmldoc = new XmlDocument();
      xmldoc.AppendChild(xmldoc.CreateElement("DocumentRoot"));
      XPathNavigator xpathnav = xmldoc.CreateNavigator();

      StringBuilder emailBody = new StringBuilder();
      XmlTextWriter xmlwriter = new XmlTextWriter(new System.IO.StringWriter(emailBody));

      XsltArgumentList xslarg = new XsltArgumentList();

      if (xsltParam != null) {
        // Add additional parameters to xsl arugument list.
        foreach (KeyValuePair<string, string> param in xsltParam) {
          xslarg.AddParam(param.Key, "", param.Value);
        }
      }

      compiler.Transform(xpathnav, xslarg, xmlwriter, null);

      body = emailBody.ToString();
      if (body.Length > 0) {
        body = body.Replace("&amp;", "&");
      }

      if (body.Length > 0) {
        body = body.Replace("&gt;", ">");
      }

      if (body.Length > 0) {
        body = body.Replace("&lt;", "<");
      }

      if (body.Length > 0) {
        body = body.Replace("%3a;", ":");
      }

      if (body.Length > 0) {
        body = body.Replace("%2f;", "/");
      }
      return body;

    }

  }
}
