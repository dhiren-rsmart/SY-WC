using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Windows.Media.Imaging;
using ImageTools.IO.Png;
using System.IO;
using System.Text;
using ImageTools;
using System.Windows.Browser;
using System.Json;

namespace smART.MVC.Silverlight
{
    public partial class StreamingViewerControl : UserControl
    {
        private VideoCaptureDevice myWebCam;
        //private CaptureSource myCaptureSource;
        //private VideoBrush myVideoBrush;
        private ImageBrush mySnapshotBrush;
        //private WriteableBitmap myImage;
        private int ReferenceID;
        private string CameraInitiator;
        private string CameraAddress;

        string UriGetCameraDetails = "../Camera/_GetCameraDetails";
        string UriSaveCameraImage = "../Camera/_SaveCameraImage";
        string UriZoom = "../Camera/_CameraZoom";
        string UriPanTilt = "../Camera/_CameraPanTilt";
        string UriSaveSnapShot = "../Camera/_GetSnapShot";

        public StreamingViewerControl(string cameraInitiator, int id, string cameraAddress)
        {
            this.ReferenceID = id;
            this.CameraInitiator = cameraInitiator;
            this.CameraAddress = cameraAddress;

            InitializeComponent();
            if (!string.IsNullOrEmpty(this.CameraAddress))
            {
                this.MediaElement.Source = new Uri(this.CameraAddress);
            }
            MediaElement.BufferingTime = new TimeSpan(0, 0, 0);

            mySnapshotBrush = new ImageBrush();
            snapshotRectangle.Fill = mySnapshotBrush;

            //MediaElement.CurrentStateChanged += (sender, e) =>
            //{
            //    Status.Text = MediaElement.CurrentState.ToString();
            //    Buffer.Visibility = MediaElement.CurrentState == MediaElementState.Buffering ? Visibility.Visible : Visibility.Collapsed;
            //};

            //this.MediaElement.BufferingProgressChanged += (sender, e) =>
            //{
            //    this.Buffer.Text = string.Format("{0:0.0} %", this.MediaElement.BufferingProgress * 100);
            //};
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Play();
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            //this.MediaElement.Pause();
            CaptureFrame();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Stop();
            
        }

        private void openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null) return;
            var sessions = (JsonArray)JsonValue.Load(e.Result);
            
        }

        private void CaptureFrame()
        {
            #region OLD AND DEPRECATED
            //WriteableBitmap wb = new WriteableBitmap(this.MediaElement, null);
            //Image image = new Image();
            //image.Height = 100;
            //image.Width = 100;
            //image.Source = wb;

            //mySnapshotBrush.ImageSource = wb;

            //byte[] imageBytes = null;
            //if (image != null)
            //{
            //    try
            //    {
            //        PngEncoder encoder = new PngEncoder();
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            var itImage = image.ToImage();
            //            encoder.Encode(itImage, ms);

            //            imageBytes = ms.ToArray();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //eat it!
            //    }
            //}
            //if (imageBytes == null) imageBytes = new byte[100000];

            //string strBytes = "";
            //if (imageBytes != null)
            //{
            //    string unencodedBytes = Convert.ToBase64String(imageBytes);
            //    strBytes = HttpUtility.UrlEncode(unencodedBytes);

            //}
            ////string dataString =string.Format("cameraInitiator={0}&id={1}&data={2}", this.CameraInitiator, this.ReferenceID, strBytes);

            #endregion

            string dataString = string.Format("cameraInitiator={0}&id={1}", this.CameraInitiator, this.ReferenceID);

            PostToUri(this.UriSaveSnapShot, dataString);

        }

        private void webClient_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                byte[] postData = Encoding.UTF8.GetBytes(e.UserState.ToString());
                e.Result.Write(postData, 0, postData.Length);
                e.Result.Close();
            }
        }

        #region Start/Stop/Snapshot Button Event Handlers
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Stop();
        }

        private void SnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            CaptureFrame();
        }

        #endregion

        #region PTZ Button Event Handlers
        private void LeftPanTilt_Click(object sender, RoutedEventArgs e)
        {
            PanTilt("Left", 1);
        }

        private void TopPanTilt_Click(object sender, RoutedEventArgs e)
        {
            PanTilt("Top", 1);
        }

        private void RightPanTilt_Click(object sender, RoutedEventArgs e)
        {
            PanTilt("Right", 1);
        }

        private void BottomPanTilt_Click(object sender, RoutedEventArgs e)
        {
            PanTilt("Bottom", 1);
        }

        private void SliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Zoom(e.NewValue);
        }

        #endregion

        private void Zoom(double zoomFactor)
        {
            string dataString = string.Format("zoomFactor={0}", zoomFactor);
            PostToUri(this.UriZoom, dataString);
        }

        private void PanTilt(string panTiltType, int panTiltFactor)
        {
            string dataString = string.Format("panTiltType={0}&panTiltFactor={1}", panTiltType, panTiltFactor);
            PostToUri(this.UriPanTilt, dataString);
        }

        private void PostToUri(string Uri, object data)
        {
            Uri serviceUri = new Uri(Uri, UriKind.Relative);
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            webClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(webClient_OpenWriteCompleted);
            webClient.OpenWriteAsync(serviceUri, "POST", data);
        }
    }
}
