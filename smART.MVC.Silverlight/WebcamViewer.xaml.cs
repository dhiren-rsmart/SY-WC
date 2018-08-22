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

using System.IO;
using ImageTools;
using ImageTools.IO.Png;
using System.Text;
using System.Windows.Media.Imaging;
using System.Json;

namespace smART.MVC.Silverlight
{
    public partial class WebcamViewer : UserControl
    {
        private VideoCaptureDevice myWebCam;
        private CaptureSource myCaptureSource;
        private VideoBrush myVideoBrush;
        private ImageBrush mySnapshotBrush;
        private WriteableBitmap myImage;
        private int ReferenceID;
        private string CameraInitiator;

        string UriGetCameraDetails = "../Camera/_GetCameraDetails";
        string UriSaveCameraImage = "../Camera/_SaveCameraImage";
        string UriZoom = "../Camera/_CameraZoom";
        string UriPanTilt = "../Camera/_CameraPanTilt";

        public WebcamViewer(string cameraInitiator, int referenceId)
        {
            this.ReferenceID = referenceId;
            this.CameraInitiator = cameraInitiator;
            InitializeComponent();
            InitializeTextData();
            InitializeWebCam();
        }

        private void InitializeTextData()
        {
            Uri serviceUri = new Uri(this.UriGetCameraDetails, UriKind.Relative);
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += openReadCompleted;
            webClient.OpenReadAsync(serviceUri);
        }

        private void InitializeWebCam()
        {
            myCaptureSource = new CaptureSource();

            //1° step: get the default capture device
            System.Collections.ObjectModel.ReadOnlyCollection<VideoCaptureDevice> videos = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
            myWebCam = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

            // 2° step: indicate the video capture device to be used by the CaptureSource 
            myCaptureSource.VideoCaptureDevice = myWebCam;

            // 3° step: initialize a VideoBrush with the CaptureSource just initialized 
            myVideoBrush = new VideoBrush();
            myVideoBrush.SetSource(myCaptureSource);

            // 4° step: fill the Rectangle with the stream provided by the VideoBrush
            webcamRectangle.Fill = myVideoBrush;
            mySnapshotBrush = new ImageBrush();
            snapshotRectangle.Fill = mySnapshotBrush;

            myCaptureSource.CaptureImageCompleted += new EventHandler<CaptureImageCompletedEventArgs>(myCaptureSource_CaptureImageCompleted);
        }

        private void openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null) return;
            var sessions = (JsonArray)JsonValue.Load(e.Result);
            //foreach (string sessionName in sessions)
            //{
            //    //this.textBlock1.Text += string.Format("{0}{1}", Environment.NewLine, sessionName);
            //}
        }

        private void myCaptureSource_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            mySnapshotBrush.ImageSource = e.Result;
            myImage = e.Result;

            #region Get Image Bytes
            byte[] imageBytes = null;
            if (myImage != null)
            {
                //byte[] buffer = myImage.ToByteArray();            

                //SaveFileDialog d = new SaveFileDialog();
                //d.ShowDialog();
                //PngEncoder.SaveToPNG(myImage,d);

                //imageBytes = ToByteArray(myImage);

              
                PngEncoder encoder = new PngEncoder();
                using (MemoryStream ms = new MemoryStream())
                {
                    var itImage = myImage.ToImage();                    
                    encoder.Encode(itImage, ms);

                    imageBytes = ms.ToArray();
                }

               
            }
            if (imageBytes == null) imageBytes = new byte[100000];
            
            string strBytes = "";
            if (imageBytes != null)
            {
                strBytes = Convert.ToBase64String(imageBytes);
            }
            string dataString = string.Format("cameraInitiator={0}&id={1}&data={2}&unused={3}", this.CameraInitiator, this.ReferenceID, strBytes, DateTime.Now.Millisecond);
            
            #endregion

            PostToUri(this.UriSaveCameraImage, dataString);

            //Uri serviceUri = new Uri(this.UriSaveCameraImage, UriKind.Relative);
            //WebClient webClient = new WebClient();
            //webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            //webClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(webClient_OpenWriteCompleted);
            //webClient.OpenWriteAsync(serviceUri, "POST", dataString);
        }

        public  byte[] ToByteArray( WriteableBitmap bmp)
        {
            int[] p = bmp.Pixels;
            int len = p.Length * 4;
            byte[] result = new byte[len]; // ARGB
            Buffer.BlockCopy(p, 0, result, 0, len);
            return result;
        }


        private void webClient_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                #region DEPRECATED
                //byte[] imageBytes = null;

                //if (myImage != null)
                //{
                //    PngEncoder encoder = new PngEncoder();
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        var itImage = myImage.ToImage();
                //        encoder.Encode(itImage, ms);

                //        imageBytes = ms.ToArray();
                //    }
                //}
                //if (imageBytes == null) imageBytes = new byte[100000];
                //if (imageBytes != null)
                //{
                //    string strBytes = Convert.ToBase64String(imageBytes);
                //    string dataString = string.Format("cameraInitiator={0}&id={1}&data={2}", this.CameraInitiator, this.ReferenceID, strBytes);
                //    byte[] postData = Encoding.UTF8.GetBytes(dataString);

                //    e.Result.Write(postData, 0, postData.Length);
                //}
                
                #endregion

                byte[] postData = Encoding.UTF8.GetBytes(e.UserState.ToString());
                e.Result.Write(postData, 0, postData.Length);
                e.Result.Close();
            }
        }

        #region Start/Stop/Snapshot Button Event Handlers
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((CaptureDeviceConfiguration.AllowedDeviceAccess ||
                CaptureDeviceConfiguration.RequestDeviceAccess()) && myCaptureSource.VideoCaptureDevice != null)
                myCaptureSource.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if ((CaptureDeviceConfiguration.AllowedDeviceAccess ||
                CaptureDeviceConfiguration.RequestDeviceAccess()) && myCaptureSource.VideoCaptureDevice != null)
                myCaptureSource.Stop();
        }

        private void SnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            if (myCaptureSource.VideoCaptureDevice != null &&
                myCaptureSource.State == CaptureState.Started)
                myCaptureSource.CaptureImageAsync();

            //#region TESTING CODE - REMOVE ON DEPLOY TO PRODUCTION
            //// The following code is ONLY FOR TESTING PURPOSES.
            //// This code should be removed once deploying to production
            //// TESTING CODE ---
            //if (myCaptureSource.VideoCaptureDevice == null)
            //{
            //    Uri serviceUri = new Uri("../Camera/_SaveCameraImage", UriKind.Relative);

            //    WebClient webClient = new WebClient();
            //    webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            //    webClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(webClient_OpenWriteCompleted);
            //    webClient.OpenWriteAsync(serviceUri, "POST");
            //}
            //// END TESTING CODE ---

            //#endregion
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
