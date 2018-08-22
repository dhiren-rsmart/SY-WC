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
using System.Windows.Threading;

namespace smART.MVC.Silverlight
{
    public partial class StreamingViewerImageControl : UserControl
    {
        private VideoCaptureDevice myWebCam;
        //private CaptureSource myCaptureSource;
        //private VideoBrush myVideoBrush;
        private ImageBrush mySnapshotBrush;
        //private WriteableBitmap myImage;
        private int ReferenceID;
        private string CameraInitiator;
        private string ImageURL;
        private DispatcherTimer imageTimer;

        string UriGetCameraDetails = "../Camera/_GetCameraDetails";
        string UriSaveCameraImage = "../Camera/_SaveCameraImage";
        string UriZoom = "../Camera/_CameraZoom";
        string UriPanTilt = "../Camera/_CameraPanTilt";
        string UriSaveSnapShot = "../Camera/_SaveSnapShot";

        int currentImage = 0;

        public StreamingViewerImageControl(string cameraInitiator, int id, string imageURL)
        {
            this.ReferenceID = id;
            this.CameraInitiator = cameraInitiator;
            this.ImageURL = imageURL;

            imageTimer = new DispatcherTimer();
            imageTimer.Interval = new TimeSpan(0,0,0,1,0);
            imageTimer.Start();
            imageTimer.Tick += new EventHandler(imageTimer_Tick);

            InitializeComponent();

            this.MediaElement.ImageOpened += new EventHandler<RoutedEventArgs>(MediaElement_ImageOpened);
            this.MediaElement2.ImageOpened += new EventHandler<RoutedEventArgs>(MediaElement2_ImageOpened);
        }

        void MediaElement2_ImageOpened(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Opacity = 0;
            this.MediaElement2.Opacity = 100;
        }

        void MediaElement_ImageOpened(object sender, RoutedEventArgs e)
        {
            this.MediaElement.Opacity = 100;
            this.MediaElement2.Opacity = 0;
        }

        private void imageTimer_Tick(object sender, EventArgs e)
        {
            BitmapImage bmi = new BitmapImage(new Uri(string.Format("{0}?unused={1}", this.ImageURL, DateTime.Now.Millisecond)));
            if (currentImage == 0)
            {
                this.MediaElement.Source = bmi;
                currentImage = 1;
            }
            else
            {
                this.MediaElement2.Source = bmi;
                currentImage = 0;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (imageTimer != null)
                imageTimer.Start();
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            CaptureFrame();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (imageTimer != null)
                imageTimer.Start();
        }

        private void openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null) return;
            var sessions = (JsonArray)JsonValue.Load(e.Result);
            
        }

        private void CaptureFrame()
        {
            string dataString = string.Format("cameraInitiator={0}&id={1}", this.CameraInitiator, this.ReferenceID);
            PostToUri(this.UriSaveSnapShot, dataString);

            BitmapImage bmi = new BitmapImage(new Uri(string.Format("{0}?unused={1}", this.ImageURL, DateTime.Now.Millisecond)));
            this.CaptureImage.Source = bmi;
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
            imageTimer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            imageTimer.Stop();
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
