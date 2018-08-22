using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Notification {

  /// <summary>
  /// Specifies the different possible values of FormatType.
  /// </summary>
  public enum EnumFormatType {

    /// <summary>
    /// Specifies plain text format.
    /// </summary>
    PlainText = 1,

    /// <summary>
    /// Specifies HTML format.
    /// </summary>
    HTML = 2
  }

  /// <summary>
  /// Specifies the different possible values of notification delivery type.
  /// </summary>
  public enum EnumNotificationDeliveryType {

    /// <summary>
    /// Specifies that notification send as email.
    /// </summary>
    Email = 1,

    /// <summary>
    /// Specifies that notification send as SMS.
    /// </summary>
    SMS = 2

  }

  public enum EnumNotificationEntity {
    Scale,
    Booking
  }

}
