using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.ViewModel {

  public class CameraInput {
    public int InitiatorId { get; set; }
    public EnumCameraInitiator CameraInitiator { get; set; }
    public string XAPPath { get; set; }
    public string SourceAddress { get; set; }
    public string ParameterString { get; set; }
  }

  public enum EnumCameraInitiator {
    Scale
  }
}
