using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace smART.MVC.Service
{

  public class TextFileLogger {

    public static void LogError(string message, string fileName = "ServiceLog.txt") {
      StreamWriter objReader = default(StreamWriter);
      using (objReader = new StreamWriter(CreateFile(fileName), true)) {
        objReader.Write(message);
        objReader.Close();
      }
    }

    public static void LogMessage(string message, string fileName = "ServiceLog.txt")
    {
      MessageFormatter msgF = new MessageFormatter();
      message = msgF.Format(message);
      StreamWriter objReader = default(StreamWriter);
      using (objReader = new StreamWriter(CreateFile(fileName), true)) {
        objReader.Write(message);
        objReader.Close();
      }
    }

    private static string GetFilePath(string fileName) {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");// Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      //path = Path.Combine(path, "Log");
      if (!System.IO.Directory.Exists(path))
        System.IO.Directory.CreateDirectory(path);
      string filePath = Path.Combine(path,fileName);
      return filePath;
    }

    private static string CreateFile(string fileName) {
      string logFile = string.Format("{1}_{0}", fileName, DateTime.Now.ToString("yyyyMMdd"));
      string filePath = GetFilePath(logFile);
      if (!System.IO.File.Exists(logFile)) {
        System.IO.File.Create(logFile);
      }
      return filePath;
    }
  }
}
