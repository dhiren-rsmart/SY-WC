using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited from base exception class. 
  /// It is used to handle syntax error exception.
  /// </summary>
  [Serializable]
  public class SyntaxException : BaseException {

    #region Local Members

    private static readonly string _message = "The syntax is incorrect.";

    #endregion Local Members

    #region Constructors
    
    /// <inheritdoc />
    public SyntaxException()
      : base(_message, null) {
    }

    /// <inheritdoc />
    public SyntaxException(string message)
      : base(message, null) {
    }

    /// <inheritdoc />
    public SyntaxException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <inheritdoc />
    public SyntaxException(string message, Exception innerException)
      : base(message, innerException) {
    }

    #endregion Constructors
  }
}
