﻿#pragma checksum "D:\Rsmart\Rsmart-SY\Server\smART.MVC.Silverlight\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "79214FFB00949EDA36B4A4134177A7CE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace smART.MVC.Silverlight {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button StartButton;
        
        internal System.Windows.Controls.Button StopButton;
        
        internal System.Windows.Controls.Button SnapshotButton;
        
        internal System.Windows.Shapes.Rectangle webcamRectangle;
        
        internal System.Windows.Shapes.Rectangle snapshotRectangle;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/smART.MVC.Silverlight;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.StartButton = ((System.Windows.Controls.Button)(this.FindName("StartButton")));
            this.StopButton = ((System.Windows.Controls.Button)(this.FindName("StopButton")));
            this.SnapshotButton = ((System.Windows.Controls.Button)(this.FindName("SnapshotButton")));
            this.webcamRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("webcamRectangle")));
            this.snapshotRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("snapshotRectangle")));
        }
    }
}

