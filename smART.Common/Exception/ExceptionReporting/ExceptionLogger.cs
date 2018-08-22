using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace smART.Common {

  /// <summary>
  /// This class provide the methods for message logging using enterprise library logging block.
  /// The application create singleton object of this type at runtime.
  /// </summary>
  public class MessageLogger {

    #region Local Varialbes

    /// <summary>
    /// The LogWriter property represent's the reference of log writer information.
    /// </summary>
    private LogWriter LogWriter {
      get;
      set;
    }

    #endregion Local Varialbes

    #region Singleton Instance

    private static MessageLogger _instance = null;
    /// <summary>
    /// Provide singleton instance of MessageLogger class.
    /// </summary>   
    public static MessageLogger Instance {
      get {
        if (_instance == null)
          _instance = new MessageLogger();
        return _instance;
      }
    }

    #endregion Singleton Instance

    #region Constructors

    /// <summary>
    /// Initialize a new instance of MessageLogger class.
    /// </summary>
    private MessageLogger() {
      Initialize();
    }

    #endregion Constructors

    #region Private

    /// <summary>
    /// Initialize the variables.
    /// </summary>
    private void Initialize() {
      var logWriterFactory = new LogWriterFactory();
      LogWriter = logWriterFactory.Create();
    }

    /// <summary>
   
    private void AddCustomProperties(LogEntry logEntry, Exception ex) {      
      string userId = Common.ExceptionUtils.GetUserId(ex);
      string transType = Common.ExceptionUtils.GetTransactionType(ex);
      string transId = Common.ExceptionUtils.GetTransactionId(ex);

      if (!string.IsNullOrEmpty(userId))
        logEntry.ExtendedProperties.Add("User Id", userId);

      if (!string.IsNullOrEmpty(transType))
        logEntry.ExtendedProperties.Add("Transaction Type", transType);

      if (!string.IsNullOrEmpty(transId))
        logEntry.ExtendedProperties.Add("Transaction Id", transId);
     
    }

    #endregion Private

    #region Public

    /// <summary>
    /// This method used to write message in configured target.
    /// </summary>
    /// <param name="logEntry"></param>
    /// <param name="includeSessionProperties">Is reuired to include session property in message.</param>        
    /// <returns>If Yes, messaged is logged otherwise it is false.</returns>
    public bool Write(LogEntry logEntry) {
        LogWriter.Write(logEntry);
      return true;
    }

    public bool LogMessage(string wrapperMessage, Priority priority,
            int eventId, System.Diagnostics.TraceEventType severity,
            string title, string category) {
      LogEntry entry = new LogEntry();
      entry.Message = wrapperMessage;
      entry.Categories.Add("Present");
      entry.Priority = (int) priority;
      entry.EventId = eventId;
      entry.Severity = severity;
      entry.Title = title;      
      return Write(entry);
    }

    
    public bool LogMessage(Exception ex, string wrapperMessage, Priority priority,
             int eventId, System.Diagnostics.TraceEventType severity,
             string title,string category) {
      LogEntry entry = new LogEntry();

      System.Exception wrapper = String.IsNullOrEmpty(wrapperMessage) ? ex : new System.Exception(wrapperMessage, ex);

      entry.Message = wrapper.Message;
      entry.Categories.Add(category);
      entry.Priority = (int)priority;
      entry.EventId = eventId;
      entry.Severity = severity;
      entry.Title = title;
      AddCustomProperties(entry ,ex);
      return Write(entry);
    }
    #endregion Public

  }
}
