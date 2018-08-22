using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Diagnostics;
using smART.Common;

namespace smART.Business {

  public class BusinessRuleExceptionHandler {

    #region Static

    /// <summary>
    /// This method is used to handle all data module exception.
    /// </summary>
    /// <param name="ex">Exception.</param> 
    /// <param name="severity">Severity.</param>
    /// <param name="message">Message.It is an optional parameter.</param>
    /// <returns></returns>
    public static bool HandleException(ref System.Exception ex, TraceEventType severity = TraceEventType.Error, string message = "") {
      return ExceptionHandler.HandleException(ref ex, severity, message, Constants.BusinessRulePolicyKey);
    }


    /// <summary>
    /// This method is used to handle all data module exception.
    /// </summary>
    /// <param name="ex">Exception.</param> 
    /// <param name="severity">Severity.</param>
    /// <param name="message">Message.It is an optional parameter.</param>
    /// <returns></returns>
    public static bool HandleException(ref System.Exception ex, string userId, string transType, string transId, TraceEventType severity = TraceEventType.Error, string message = "") {
      Common.ExceptionUtils.SetUserId(ex, userId);
      Common.ExceptionUtils.SetTransactionType(ex, transType);
      Common.ExceptionUtils.SetTransactionId(ex, transId);
      return ExceptionHandler.HandleException(ref ex, severity, message, Constants.BusinessRulePolicyKey);
    }

    #endregion Static
  }
}
