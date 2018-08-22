using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited from base exception class. 
  /// It is used to pass exception when data is not currect.
  /// </summary>
  [Serializable]
  public class InvalidDataException : BaseException {

    #region Local Members

    private static readonly string _message = "Invalid data.";

    #endregion Local Members

    #region Constructors

    /// <inheritdoc />
    public InvalidDataException()
      : base(_message, null) {
    }

    /// <inheritdoc />
    public InvalidDataException(string message)
      : base(message, null) {
    }

    /// <inheritdoc />
    public InvalidDataException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <inheritdoc />
    public InvalidDataException(string message, Exception innerException)
      : base(message, innerException) {
    }

    #endregion Constructors

  }
}
