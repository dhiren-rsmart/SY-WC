using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited from base exception class. 
  /// It is used to handle exception when value is null.
  /// </summary>
  [Serializable]
  public class NullValueException : BaseException {

    #region Local Members

    private static readonly string _message = "Null value is not allowed.";

    #endregion Local Members

    #region Constructors

    /// <inheritdoc />
    public NullValueException()
      : base(_message, null) {
    }

    /// <inheritdoc />
    public NullValueException(string message)
      : base(message, null) {
    }

    /// <inheritdoc />
    public NullValueException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <inheritdoc />
    public NullValueException(string message, Exception innerException)
      : base(message, innerException) {
    }

    #endregion Constructors

  }
}
