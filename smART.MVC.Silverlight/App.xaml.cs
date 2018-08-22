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

namespace smART.MVC.Silverlight
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            int id = -1;
            string cameraInitiator = string.Empty;
            string cameraAddress = string.Empty;
            string cameraType = "3";

            foreach (string key in e.InitParams.Keys)
            {
                if (key.ToLower().Equals("id"))
                {
                    id = int.Parse(e.InitParams[key]);
                }
                else if (key.ToLower().Equals("camerainitiator"))
                {
                    cameraInitiator = e.InitParams[key];
                }
                else if (key.ToLower().Equals("camerasourceaddress"))
                {
                    cameraAddress = e.InitParams[key];
                }
                else if (key.ToLower().Equals("cameratype"))
                {
                    cameraType = e.InitParams[key];
                }
            }

            UIElement rootElement;
            switch (cameraType)
            {
                case "1":
                default:
                    rootElement = new StreamingViewerImageControl(cameraInitiator, id, cameraAddress);
                    break;
                case "2":
                    rootElement = new StreamingViewerControl(cameraInitiator, id, cameraAddress);
                    break;
                case "3":
                    rootElement = new WebcamViewer(cameraInitiator, id);
                    break;
            }
            this.RootVisual = rootElement;
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
