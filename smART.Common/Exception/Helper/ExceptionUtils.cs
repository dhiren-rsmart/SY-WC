using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net;

namespace smART.Common {

  /// <summary>
  /// This class provide utilitty methods for exception handling.
  /// </summary>
  public class ExceptionUtils {

    /// <summary>
    /// This method is used to check for logged.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <returns>If true, this exception is logged else false.</returns>
    public static bool IsLogged(Exception ex) {
      return Convert.ToBoolean(GetData(ex, Constants.IsLoggedKey));
    }

    /// <summary>
    /// This method is used to set pass through flag in a exception.
    /// </summary>
    /// <param name="ex">Exception.</param>   
    public static void MarkAsLogged(Exception ex) {
      SetData(ex, Constants.IsLoggedKey, true);
    }

    /// <summary>
    /// This method provide to add additional message in given exception.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="msg">Message.</param>
    public static void SetAdditionalMsg(Exception ex, string msg) {
      if (!ex.Data.Contains(Constants.AdditionalMsgKey))
        ex.Data.Add(Constants.AdditionalMsgKey, msg);
    }


    /// <summary>
    /// This method  provide to get additional message from given  exception.
    /// </summary>
    /// <param name="ex">Ex</param>
    /// <returns></returns>
    public static string GetAdditionalMsg(Exception ex) {
      return Convert.ToString(GetData(ex, Constants.AdditionalMsgKey));
    }
    /// <summary>
    /// This method provide to set severity of exception.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="severity">Severity.</param>
    public static void SetSeverity(Exception ex, System.Diagnostics.TraceEventType severity) {
      if (!ex.Data.Contains(Constants.SeverityKey))
        ex.Data.Add(Constants.SeverityKey, severity.ToString());
    }

    /// <summary>
    /// This method provide to get Severity of given exception.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static TraceEventType GetSeverity(Exception ex) {
      string strSeverity = Convert.ToString(GetData(ex, Constants.SeverityKey));
      TraceEventType severity = (TraceEventType)Enum.Parse(typeof(TraceEventType), strSeverity, true);
      return severity;
    }

    /// <summary>
    /// This method  provide to get Userid from given  exception.
    /// </summary>
    /// <param name="ex">Ex</param>
    /// <returns></returns>
    public static string GetUserId(Exception ex) {
      return Convert.ToString(GetData(ex, Constants.UserIdKey));
    }
    /// <summary>
    /// This method provide to set Login Userid.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="userId">user Id.</param>
    public static void SetUserId(Exception ex, string userId) {
      if (!ex.Data.Contains(Constants.UserIdKey))
        ex.Data.Add(Constants.UserIdKey, userId);
    }

    /// <summary>
    /// This method  provide to get Userid from given  exception.
    /// </summary>
    /// <param name="ex">Ex</param>
    /// <returns></returns>
    public static string GetTransactionType(Exception ex) {
      return Convert.ToString(GetData(ex, Constants.TransactionTypeKey));
    }
    /// <summary>
    /// This method provide to set TransactionType
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="transactionType">TransactionType</param>
    public static void SetTransactionType(Exception ex, string transactionType) {
      if (!ex.Data.Contains(Constants.TransactionTypeKey))
        ex.Data.Add(Constants.TransactionTypeKey, transactionType);
    }

    /// <summary>
    /// This method  provide to get Userid from given  exception.
    /// </summary>
    /// <param name="ex">Ex</param>
    /// <returns></returns>
    public static string GetTransactionId(Exception ex) {
      return Convert.ToString(GetData(ex, Constants.TransactionIDKey));
    }
    /// <summary>
    /// This method provide to set transactionId
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="transactionId">transactionId</param>
    public static void SetTransactionId(Exception ex, string transactionId) {
      if (!ex.Data.Contains(Constants.TransactionIDKey))
        ex.Data.Add(Constants.TransactionIDKey, transactionId);
    }
    /// <summary>
    /// Add extra information in excetpion.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="key">Extra information key.</param>
    /// <param name="data">Extra information of.</param>    
    public static void SetData(Exception ex, string key, object data) {
      // This test is done to protect from the error of re-addition of a key.     
      if (ex.Data.Contains(key))
        ex.Data[key] = data;
      else
        ex.Data.Add(key, data);
    }

    /// <summary>
    /// Get extra information from exception.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="key">Extra information key.</param>
    /// <returns></returns>
    public static object GetData(Exception ex, string key) {
      object data = null;
      if (ex != null && ex.Data.Contains(key))
        data = ex.Data[key];
      return data;
    }
  }
}
