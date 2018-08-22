using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace smART.Common {

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
      // Display an error to the "poor" user
      string s = "An internal error has occurred and has been logged. Please contact your system administrator for help.";

      string s1 = "";
      if (!String.IsNullOrEmpty(message))
        //s1 += String.Format("Message: {0}", message) + System.Environment.NewLine;
        s1 += message + System.Environment.NewLine;
      if (e != null)
        s1 += String.Format("Exception: {0}", e.ToString());

      StackTrace objStackTrace = new StackTrace(e, true);
      string methodName = string.Empty;
      int lineNo = 0;

      if (objStackTrace.FrameCount > 0) {
        StackFrame stackFrame = objStackTrace.GetFrame(objStackTrace.FrameCount - 1);
        methodName = stackFrame.GetMethod().Name;
        lineNo = stackFrame.GetFileLineNumber();
      }

      if (!string.IsNullOrEmpty(methodName))
        s1 += System.Environment.NewLine + string.Format("Method Name: {0}", methodName);

      if (lineNo > 0)
        s1 += System.Environment.NewLine + string.Format("Line No.: {0}", lineNo);

      s = s + System.Environment.NewLine + System.Environment.NewLine + s1;
      return s;
    }

  }
}
