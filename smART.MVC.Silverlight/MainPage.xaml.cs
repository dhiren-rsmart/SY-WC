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
using System.Json;

using System.IO;
using ImageTools;
using ImageTools.IO.Png;
using System.Text;
using System.Windows.Media.Imaging;

namespace smART.MVC.Silverlight
{
    public partial class MainPage : UserControl
    {
        private VideoCaptureDevice myWebCam;
        private CaptureSource myCaptureSource;
        private VideoBrush myVideoBrush;
        private ImageBrush mySnapshotBrush;
        private WriteableBitmap myImage;
        private int Id;
        private string CameraInitiator;

        public MainPage(string cameraInitiator, int id)
        {
            this.Id = id;
            this.CameraInitiator = cameraInitiator;
            InitializeComponent();
            InitializeTextData();
            InitializeWebCam();
        }

        private void InitializeTextData()
        {
            Uri serviceUri = new Uri("../Camera/_GetCameraDetails", UriKind.Relative);
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += openReadCompleted;
            webClient.OpenReadAsync(serviceUri);
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

        private void myCaptureSource_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            mySnapshotBrush.ImageSource = e.Result;
            myImage = e.Result;

            Uri serviceUri = new Uri("../Camera/_SaveCameraImage", UriKind.Relative);

            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            webClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(webClient_OpenWriteCompleted);
            webClient.OpenWriteAsync(serviceUri, "POST");
        }

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


            if (myCaptureSource.VideoCaptureDevice == null)
            {
                Uri serviceUri = new Uri("../Camera/_SaveCameraImage", UriKind.Relative);

                WebClient webClient = new WebClient();
                webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                webClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(webClient_OpenWriteCompleted);
                webClient.OpenWriteAsync(serviceUri, "POST");
            }

        }

        void webClient_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                byte[] imageBytes = null;

                if (myImage != null)
                {
                    //PngEncoder encoder = new smART .MVC.Silverlight.PngEncoder();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var itImage = myImage.ToImage();
                        //ms = PngEncoder.Encode(itImage, 20, 30);
                        //encoder.Encode(itImage, ms);

                        imageBytes = ms.ToArray();
                    }
                }
                if (imageBytes == null) imageBytes = new byte[100000];
                if (imageBytes != null)
                {
                    string strBytes = Convert.ToBase64String(imageBytes);
                    string dataString = string.Format("cameraInitiator={0}&id={1}&data={2}", this.CameraInitiator, this.Id, strBytes);
                    byte[] postData = Encoding.UTF8.GetBytes(dataString);

                    e.Result.Write(postData, 0, postData.Length);
                }
                e.Result.Close();
            }
        }

    }
}
