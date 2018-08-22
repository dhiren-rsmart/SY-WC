using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace smART.Integration.Scale.Service
{
    public class WeightService : IWeightService
    {
        public delegate void delegateLogMessage(string Message);
        public event delegateLogMessage LogMessage;

        public WeightData GetWeight(string scaleIdentifier)
        {
            return new WeightData() { Weight = this.GetWeight().ToString() };
        }

        public WeightData GetWeightJSON()
        {
            return new WeightData() { Weight = this.GetWeight().ToString() };
        }

        private long GetWeight()
        {
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

            return -1;
          }

          byte[] buffer = new byte[1024];

          Log(string.Format("Connected to {0}", ipAddress));
          Log(string.Format("About to read ..."));

          stream.Read(buffer, 0, 1024);
          string result = Encoding.ASCII.GetString(buffer);

          stream.Close();
          client.Close();

          Log(result);

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

          return weight;         
        }

        private void Log(string Message)
        {
            if (LogMessage != null)
                LogMessage(Message);
        }
    }
}
