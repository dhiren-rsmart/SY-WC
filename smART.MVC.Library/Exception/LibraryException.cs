using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using smART.Common;

namespace smART.Library {

  /// <summary>
  /// The exception that is thrown by exception handling block.It log the orginal message and replace the original exception,
  /// occurred in Data layer, with a user friendly message.
  /// </summary>
  [Serializable]
  public class LibraryException : BaseException {

    private static readonly string _message = "An error occurred during service operation.";

    #region Constructors

    /// <inheritdoc />
    public LibraryException()
      : base(_message) {
    }

    /// <inheritdoc />
    public LibraryException(string message)
      : base(message) {
    }

    /// <inheritdoc />
    public LibraryException(string message, System.Exception inner)
      : base(message, inner) {   
    }

    #endregion

  }
}
