using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace smART.Integration.LeadsOnline
{

  public class TextFileLogger {

    public static void Log(string message, string fileName = "Log.txt") {
      StreamWriter objReader = default(StreamWriter);
      using (objReader = new StreamWriter(CreateFile(fileName), true)) {
        objReader.Write(message);
        objReader.Close();
      }
    }

    private static string GetFilePath(string fileName) {
      string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      path = Path.Combine(path, "Log");
      if (!System.IO.Directory.Exists(path))
        System.IO.Directory.CreateDirectory(path);
      string filePath = Path.Combine(path, fileName);
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
