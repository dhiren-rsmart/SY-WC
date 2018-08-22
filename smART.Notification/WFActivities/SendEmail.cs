using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Net.Mail;
using System.Net;
using smART.ViewModel;
using smART.Common;
using smART.Integration.Email;

namespace smART.Notification.WFActivities {

  public sealed class SendEmail : CodeActivity {

    public InArgument<string> CustomerName {
      get;
      set;
    }

    public InArgument<string> CompanyName {
      get;
      set;
    }

    public InArgument<string> UserName {
      get;
      set;
    }

    public InArgument<string> EntityID {
      get;
      set;
    }

    public InArgument<NotificationDefinition> NotificationDef {
      get;
      set;
    }

    public InArgument<string> XSLPath {
      get;
      set;
    }

    public OutArgument<string> Result {
      get;
      set;
    }

    public InArgument<string> BookingID {
      get;
      set;
    }

    public InArgument<string> SOID {
      get;
      set;
    }


    // If your activity returns a value, derive from CodeActivity<TResult>
    // and return the value from the Execute method.
    protected override void Execute(CodeActivityContext context) {
      try {
        // Can read this information from config file.

        NotificationDefinition message = NotificationDef.Get(context) as NotificationDefinition;
        SmtpClient SmtpServer = ReadSMTPConfigurationInfo(message);


        MailMessage mail = new MailMessage();

        // Sender
        mail.From = message.Sender;

        // Add attachments
        foreach (var attachment in message.Attachments) {
          mail.Attachments.Add(attachment);
        }

        // To Receipients
        foreach (var toRecept in message.ToRecipients) {
          mail.To.Add(toRecept);
        }

        // Cc Receipients 
        if (message.CCRecipients != null) {
          foreach (var ccRecept in message.CCRecipients) {
            mail.To.Add(ccRecept);
          }
        }

        // Bcc Receipients
        if (message.BCCRecipients != null) {
          foreach (var bccRecept in message.BCCRecipients) {
            mail.To.Add(bccRecept);
          }
        }

        // Subject.
        mail.Subject = message.Subject;

        // Body.
        mail.Body = NotificationHelper.XSLTransform(XSLPath.Get(context), CustomerName.Get(context), EntityID.Get(context), CompanyName.Get(context),UserName.Get(context), BookingID.Get(context), SOID.Get(context));

        // Body Type
        mail.IsBodyHtml = message.FormatType == EnumFormatType.HTML;

        // Send email.
        SmtpServer.Send(mail);

        Result.Set(context, EntityID.Get(context));

      }
      catch (Exception ex) {
        ExceptionFormatter formater = new ExceptionFormatter();
        string formatedException = formater.Format("Send mail error", ex);
        Common.MessageLogger.Instance.LogMessage(ex, formatedException, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Critical, "WF Email Error", "Email");
        throw ex;
      }
    }

    // Read smtp server configuration from config file.
    internal SmtpClient ReadSMTPConfigurationInfo(NotificationDefinition message) {
      SmtpClient SmtpServer = new SmtpClient(message.SMTPServer);
      SmtpServer.Credentials = new NetworkCredential(message.SMTPServerCredentialID, message.SMTPServerCredentialPwd);
      SmtpServer.Timeout = 20000000;
      SmtpServer.EnableSsl = false;
      return SmtpServer;
    }
  }
}
