using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace smART.Common {

  /// <summary>
  /// This class impliment IException interface.
  /// It provide mehtod to handle exception.
  /// </summary>
  [ConfigurationElementType(typeof(CustomHandlerData))]
  public class ExceptionLoggingHandler : IExceptionHandler {

    #region Local Members

    private string _title = string.Empty;
    private string _category = string.Empty;

    #endregion Local Members

    #region Constructor

    /// <summary>
    /// Initialize a new instance of ExceptionLoggingHandler class.    
    /// </summary>
    /// <param name="collection">It contains properties of ExceptionHandler in the form of key and value.</param>
    public ExceptionLoggingHandler(NameValueCollection collection) {
      if (collection.AllKeys.Contains("Title"))
        _title = collection.Get("Title");
      if (collection.AllKeys.Contains("Category"))
        _category = collection.Get("Category");
    }

    #endregion constructor

    #region IExceptionHandler Members

    /// <inheritDoc/>
    public Exception HandleException(Exception exception, Guid handlingInstanceId) {

      if (!ExceptionUtils.IsLogged(exception)) {

        // Get addition msg if exits in exception data object.
        string message = ExceptionUtils.GetAdditionalMsg(exception);

        // Format the exception message.
        ExceptionFormatter formater = new ExceptionFormatter();
        string formatedException = formater.Format(message, exception);

        // If severity is critical then it also send error to config mail address.
        MessageLogger.Instance.LogMessage(exception, formatedException, Priority.High, 0, TraceEventType.Error, _title, _category);

        // Set as logged.
        ExceptionUtils.MarkAsLogged(exception);
      }
      return exception;
    }

    #endregion

  }
}
