using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// The exception that is thrown by the exception handling block to replace the exception from data layer.
  /// </summary>
  [Serializable]
  public class PassThroughException : BaseException {

    #region Local Members

    private static readonly string _message = "Pass this exception to an outer level.";

    #endregion Local Members

    #region Constructors

    /// <inheritdoc />
    public PassThroughException()
      : base(_message) {
    }

    /// <inheritdoc />
    public PassThroughException(string message)
      : base(message) {
    }

    /// <inheritdoc />
    public PassThroughException(string message, System.Exception inner)
      : base(message, inner) {
    }    

    #endregion Constructors
  }
}
