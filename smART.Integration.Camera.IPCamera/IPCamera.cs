using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace smART.Integration.Camera.IPCamera
{
    public abstract class IPCamera: ICamera
    {
        protected string BaseURL { get; set; }
        protected string SnapShotURL { get; set; }
        protected string PanTiltUrl { get; set; }
        protected string ZoomUrl { get; set; }
        protected string CameraID { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public delegate void delegateLogMessage(string message);
        public event delegateLogMessage LogMessage;

        protected void WriteLogMessage(string message)
        {
            if(LogMessage != null)
                LogMessage(message);
        }

        public IPCamera(string baseURL, string username, string password, string snapShotURL, string zoomURL, string panTiltURL, string cameraID)
        {
            this.CameraID = cameraID;
            this.BaseURL = baseURL;
            this.SnapShotURL = snapShotURL;
            this.PanTiltUrl = panTiltURL;
            this.ZoomUrl = zoomURL;
            this.Username = username;
            this.Password = password;

            WriteLogMessage(string.Format("Initialized camera settings: CameraID:{0}, BaseURL:{1}, SnapShotURL:{2}, PanTiltURL:{3}, ZoomURL:{4}, Username:{5}, Password:{6}", 
                this.CameraID, this.BaseURL, this.SnapShotURL, this.PanTiltUrl, this.ZoomUrl, this.Username, this.Password));
        }

        public virtual SnapShot GetSnapShot()
        {
            SnapShot snapShot = new SnapShot();
            snapShot.BinaryImage = new byte[1000];
            return snapShot;
        }

        public virtual SnapShot GetSnapShotWithResolutionAndCompression(string resolution, string compression)
        {
            return GetSnapShot();
        }

        public virtual bool Zoom(double zoomFactor)
        {
            return true;
        }

        public virtual bool PanTilt(string panTiltType, double panTiltFactor)
        {
            return true;
        }

        public virtual ICredentials GetCredentials()
        {
            NetworkCredential nc = new NetworkCredential(this.Username, this.Password);
            return nc;
        }

        protected string GetStringResponseForRequest(string requestUrl, byte[] data)
        {
            // declare objects
            string responseData = String.Empty;
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            StreamReader strmReader = null;

            try
            {
                WriteLogMessage(string.Format("Inside GetStringResponseForRequest: Getting response for {0}", requestUrl));

                req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                req.Credentials = GetCredentials();

                // set HttpWebRequest properties here (Method, ContentType, etc)
                // some code

                // in case of POST you need to post data
                if ((data != null) && (data.Length > 0))
                {
                    using (Stream strm = req.GetRequestStream())
                    {
                        strm.Write(data, 0, data.Length);
                    }
                }

                resp = (HttpWebResponse)req.GetResponse();
                strmReader = new StreamReader(resp.GetResponseStream());
                responseData = strmReader.ReadToEnd().Trim();
            }
            catch (Exception)
            {
               // throw ex;
            }
            finally
            {
                if (req != null)
                {
                    req = null;
                }

                if (resp != null)
                {
                    resp.Close();
                    resp = null;
                }
            }

            return responseData;
        }

        protected byte[] GetByterArrayResponseForRequest(string requestUrl, byte[] data)
        {
            // declare objects
            string responseData = String.Empty;
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                req.Credentials = GetCredentials();

                WriteLogMessage(string.Format("Getting response for {0}", requestUrl));
                // set HttpWebRequest properties here (Method, ContentType, etc)
                // some code

                // in case of POST you need to post data
                // in case of POST you need to post data
                if ((data != null) && (data.Length > 0))
                {
                    using (Stream strm = req.GetRequestStream())
                    {
                        strm.Write(data, 0, data.Length);
                    }
                }

                resp = (HttpWebResponse)req.GetResponse();
                using (Stream responseStream = resp.GetResponseStream())
                {
                    byte[] buffer = new byte[10240];
                    int bytes;
                    while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (req != null)
                {
                    req = null;
                }

                if (resp != null)
                {
                    resp.Close();
                    resp = null;
                }
            }

            return memoryStream.ToArray();
        }
    }
}
