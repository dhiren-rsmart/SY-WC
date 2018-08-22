using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Integration.QB {

  public class ExceptionHandler {

    private const string ErrorLogFileName = "ErrorLog.txt";
    static public void HandleException(System.Exception ex, string message = "An errror occured in scheduler", bool ignoreException = true) {

      // Format the exception message.
      ExceptionFormatter formater = new ExceptionFormatter();
      string formatedException = formater.Format(message, ex);

      // Log error message in production category.
      // If severity is critical then it also send error to config mail address.
      TextFileLogger.LogError(formatedException, ErrorLogFileName);

      if (!ignoreException) {
        throw ex;
      }
    }
  }
}
