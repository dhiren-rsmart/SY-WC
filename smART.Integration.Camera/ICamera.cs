using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Integration.Camera
{
    public interface ICamera
    {
        // Define Camera operations
        SnapShot GetSnapShot();
        SnapShot GetSnapShotWithResolutionAndCompression(string resolution, string compression);
        bool Zoom(double zoomFactor);
        bool PanTilt(string panTiltType, double panTiltFactor);
    }
}
