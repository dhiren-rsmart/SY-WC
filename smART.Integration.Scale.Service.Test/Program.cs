using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace smART.Integration.Scale.Service.Test {
  class Program {
    static void Main(string[] args) {
      try {
        string ipAddress = ConfigurationManager.AppSettings["ipaddress"];
        int port = int.Parse(ConfigurationManager.AppSettings["port"]);

        TcpClient client = new TcpClient();
        NetworkStream stream = null;

        Log(string.Format("Trying to connect to {0}:{1}", ipAddress, port));

        try {
          client.Connect(IPAddress.Parse(ipAddress), port);
          stream = client.GetStream();
        }
        catch (Exception ex) {
          Log(ex.Message);
          Log(ex.StackTrace);

          if (client != null && client.Connected)
            client.Close();

          Log("Weight:-1") ;
        }

        byte[] buffer = new byte[1024];

        Log(string.Format("Connected to {0}", ipAddress));
        Log(string.Format("About to read ..."));

        stream.Read(buffer, 0, 1024);
        //for (int i = 0; i < buffer.Length-1; i++) {
        //   Console .WriteLine(buffer[0].ToString ());
        //}
        string result = Encoding.ASCII.GetString(buffer);

        Console.WriteLine(result);
        Console.ReadLine();

        stream.Close();
        client.Close();


        int indexOfStart = -1;
        indexOfStart = result.IndexOf((char) 2, indexOfStart + 1);
        Log(string.Format("Start Index {0}", indexOfStart));
        long weight = -1;
        while (indexOfStart != -1) {

          if (result.Substring(indexOfStart).Length > 13)
            long.TryParse(result.Substring(indexOfStart + 1, 12).Replace("LG", ""), out weight);

          Log(weight.ToString());

          indexOfStart = result.IndexOf((char) 2, indexOfStart + 1);
          Log(string.Format("Start Index {0}", indexOfStart));
        }


      }
      catch (Exception ex) {
        Console.WriteLine("Message: " + ex.Message + Environment.NewLine + "Stact Trace: " + ex.StackTrace);
      }
      Console.ReadLine();
    }

    private static void Log(string message) {
      Console.WriteLine(message);
    }
  }
}
