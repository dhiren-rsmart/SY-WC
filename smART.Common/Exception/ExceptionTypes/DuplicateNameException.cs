using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited to base exception. It is use for duplicate object found.
  /// </summary>
  [Serializable]
  public class DuplicateException : BaseException {

    #region Local Members

    private static readonly string _message = "Similar transaction already exists. Please click on search button and fetch existing record.";

    #endregion Local Members

    #region Constructor   

    /// <inheritdoc />
    public DuplicateException()
      : base(_message, null) {
    }

    /// <inheritdoc />
    public DuplicateException(string message)
      : base(message, null) {
    }

    /// <inheritdoc />
    public DuplicateException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <inheritdoc />
    public DuplicateException(string message, Exception innerException)
      : base(message, innerException) {
    }

    #endregion Constructor

  }
}
