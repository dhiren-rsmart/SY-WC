using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace smART.Common {

  public class ExceptionHandler {

    /// <summary>
    /// This method is used to hanlde exception according to given category.
    /// </summary>
    /// <param name="ex">Exeption.</param>    
    /// <param name="severity">Severity.</param>
    /// <param name="message">Message.</param>      
    /// <param name="policyName">Policy Name.</param>
    /// <returns>If True, Exception is handle.Otherwise false.</returns>
    public static bool HandleException(ref System.Exception ex, TraceEventType severity, string message, string policyName) {
      bool rethrow = false;
      // Add additional message.
      ExceptionUtils.SetAdditionalMsg(ex, message);
      // Set Severity.
      ExceptionUtils.SetSeverity(ex, severity);

      rethrow = PolicyHandler.Instance.Handle(policyName, ref ex);
      if (rethrow) {
        throw ex;
      }

      return rethrow;
    }
  }
}
