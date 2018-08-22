using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Integration.Camera.IPCamera.Axis
{
    public class AxisIPCamera: IPCamera
    {
        public AxisIPCamera(string baseURL, string username, string password, string snapShotURL, string zoomURL, string panTiltURL, string cameraID)
            : base(baseURL, username, password, snapShotURL, zoomURL, panTiltURL, cameraID)
        { }

        public AxisIPCamera(string baseURL, string username, string password)
            : base(baseURL, username, password, "jpg/image.cgi", "com/ptz.cgi", "com/ptz.cgi", "1")
        { }

        public AxisIPCamera(string baseURL)
            : base(baseURL, "root", "admin", "jpg/image.cgi", "com/ptz.cgi", "com/ptz.cgi", "1")
        { }

        public override SnapShot GetSnapShot()
        {
            string requestUrl = string.Format("{0}/{1}?camera={2}", this.BaseURL, this.SnapShotURL, this.CameraID);
            
            SnapShot snapShot = new SnapShot();
            snapShot.BinaryImage = GetByterArrayResponseForRequest(requestUrl, null);

            return snapShot;
        }

        public override SnapShot GetSnapShotWithResolutionAndCompression(string resolution, string compression)
        {
            string requestUrl = string.Format("{0}/{1}?resolution={2}&compression={3}&camera={4}", this.BaseURL, this.SnapShotURL, resolution, compression, this.CameraID);

            SnapShot snapShot = new SnapShot();
            snapShot.BinaryImage = GetByterArrayResponseForRequest(requestUrl, null);

            return snapShot;
        }

        public override bool Zoom(double zoomFactor)
        {
            string requestUrl = string.Format("{0}/{1}?camera={2}&zoom={3}", this.BaseURL, this.ZoomUrl, this.CameraID, (int)zoomFactor);
            GetStringResponseForRequest(requestUrl, null);
            return true;
        }

        public override bool PanTilt(string panTiltType, double panTiltFactor)
        {
            WriteLogMessage(string.Format("Perform PanTilt, PanTiltType {0}, PanTiltFactore {1}", panTiltType, panTiltFactor));
            string moveCommand = string.Empty;
            switch (panTiltType)
            {
                case "left":
                    moveCommand = "left";
                    break;
                case "right":
                    moveCommand = "right";
                    break;
                case "top":
                case "up":
                    moveCommand = "up";
                    break;
                case "bottom":
                case "down":
                    moveCommand = "down";
                    break;
            }

            int ipanTiltFactor = (int)panTiltFactor;

            string requestUrl = string.Format("{0}/{1}?camera={2}&move={3}", this.BaseURL, this.ZoomUrl, this.CameraID, moveCommand);
            WriteLogMessage(string.Format("Getting string response for {0}", requestUrl));
            for(int i=0;i<ipanTiltFactor;i++)
                GetStringResponseForRequest(requestUrl, null);

            requestUrl = string.Format("{0}/{1}?camera={2}&move={3}", this.BaseURL, this.ZoomUrl, this.CameraID, "stop");
            WriteLogMessage(string.Format("Getting string response for {0}", requestUrl));
            GetStringResponseForRequest(requestUrl, null);

            return true;
        }
    }
}
