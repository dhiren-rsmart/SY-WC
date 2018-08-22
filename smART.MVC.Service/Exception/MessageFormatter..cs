using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace smART.MVC.Service
{


  public class MessageFormatter {

    /// <summary>
    /// This method provide formatted exception message. 
    /// </summary>
    /// <param name="message">Custom exception message.</param>
    /// <param name="e">Exception</param>
    /// <returns></returns>
    public string Format(string message) {

      // Display an error to the "poor" user
      string s = string.Format("Date: {0}", DateTime.Now);

      s += System.Environment.NewLine + string.Format("Title: {0}", string.IsNullOrEmpty(message) ? "An internal error has occurred and has been logged. Please contact your system administrator for help." : message);
      
      s += System.Environment.NewLine + "===============================================================================================================================";

      s += System.Environment.NewLine + System.Environment.NewLine;

      return s;
    }
  }
}
