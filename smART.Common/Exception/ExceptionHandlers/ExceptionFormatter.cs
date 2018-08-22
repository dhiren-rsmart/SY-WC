using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace smART.Integration.QB {

  /// <summary>
  /// This class provides exception formatting methods.
  /// </summary>
  public class ExceptionFormatter {

    /// <summary>
    /// This method provide formatted exception message. 
    /// </summary>
    /// <param name="message">Custom exception message.</param>
    /// <param name="e">Exception</param>
    /// <returns></returns>
    public string Format(string message, System.Exception e) {

      if (e.GetBaseException() != null)
        e = e.GetBaseException();

      // Display an error to the "poor" user
      string s = string.Format("Date: {0}", DateTime.Now);

      s += System.Environment.NewLine + string.Format("Title: {0}", string.IsNullOrEmpty(message) ? "An internal error has occurred and has been logged. Please contact your system administrator for help." : message);

      s += System.Environment.NewLine + String.Format("Exception: {0}", e.ToString());

      s += System.Environment.NewLine + String.Format("Stack Trace: {0}", e.StackTrace.ToString());

      StackTrace objStackTrace = new StackTrace(e, true);
      string methodName = string.Empty;
      int lineNo = 0;
      if (objStackTrace.FrameCount > 0) {
        StackFrame stackFrame = objStackTrace.GetFrame(objStackTrace.FrameCount - 1);
        methodName = stackFrame.GetMethod().Name;
        lineNo = stackFrame.GetFileLineNumber();
      }

      if (!string.IsNullOrEmpty(methodName))
        s += System.Environment.NewLine + string.Format("Method Name: {0}", methodName);

      if (lineNo > 0)
        s += System.Environment.NewLine + string.Format("Line No.: {0}", lineNo);
   
      s += System.Environment.NewLine + "===============================================================================================================================";

      s += System.Environment.NewLine + System.Environment.NewLine;

      return s;
    }

  }
}
