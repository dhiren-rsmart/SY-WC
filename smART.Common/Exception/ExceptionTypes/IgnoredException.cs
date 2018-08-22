using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited from base exception class.
  /// It is used to ignore.
  /// </summary>
  [Serializable]
  public class IgnoredException : BaseException {

    #region Local Members

    private static readonly string _message = "Ignore this error.";

    #endregion Local Members

    #region Constructors

    /// <inheritdoc />
    public IgnoredException()
      : base(_message, null) {
    }

    /// <inheritdoc />
    public IgnoredException(string message)
      : base(message, null) {
    }

    /// <inheritdoc />
    public IgnoredException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <inheritdoc />
    public IgnoredException(string message, Exception innerException)
      : base(message, innerException) {
    }

    #endregion Constructors

  }
}
