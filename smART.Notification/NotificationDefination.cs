
using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace smART.Notification {

  // NotificationWF
  /// <summary>
  /// Its a intermediate class represents a notification alert with all required information.
  /// </summary>
  /// <remarks>This class is for internal use only.</remarks>
  public class NotificationDefinition {

    /// <summary>
    /// Default constructor.
    /// </summary>
    public NotificationDefinition() {
    }

    /// <summary>
    /// First part of notification like Email Subject.
    /// </summary>
    public string Subject {
      get;
      set;
    }

    /// <summary>
    /// Second part of notification like Email Body.
    /// </summary>
    public string Body {
      get;
      set;
    }

    /// <summary>
    /// From user name.
    /// </summary>
    public MailAddress Sender { get; set; }


    /// <summary>
    /// Target recipient user name.
    /// </summary>
    public MailAddressCollection ToRecipients {
      get;
      set;
    }

    /// <summary>
    /// Target recipient user name.
    /// </summary>
    public MailAddressCollection CCRecipients {
      get;
      set;
    }

    /// <summary>
    /// Target recipient user name.
    /// </summary>
    public MailAddressCollection BCCRecipients {
      get;
      set;
    }

    /// <summary>
    ///  Mail Attachment collection
    /// </summary>
    public List<Attachment> Attachments {
      get;
      set;
    }


    /// <summary>
    /// Notification delivery type.
    /// Its value is predefined set of enum item, describe delivery type of 
    /// notification like Email, SMS etc.
    /// </summary>
    public EnumNotificationDeliveryType DeliveryType {
      get;
      set;
    }

    /// <summary>
    /// Notification format type
    /// Its value is predefined set of enum item, describe format type of 
    /// notification like HTML, Plain Text.
    /// </summary>
    public EnumFormatType FormatType {
      get;
      set;
    }

    /// <summary>
    /// SMTP Server Address.
    /// </summary>
    public string SMTPServer { get; set; }

    /// <summary>
    /// SMTP User  ID.
    /// </summary>
    public string SMTPServerCredentialID { get; set; }

    /// <summary>
    /// SMTP User Password.
    /// </summary>
    public string SMTPServerCredentialPwd { get; set; }


  }
}
