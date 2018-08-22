using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using smART.Common;

namespace smART.Business{

  /// <summary>
  /// The exception that is thrown by exception handling block.It log the orginal message and replace the original exception,
  /// occurred in Data layer, with a user friendly message.
  /// </summary>
  [Serializable]
  public class BusinessRulesException : BaseException {

    private static readonly string _message = "An error occurred during applying business rules operation.";

    #region Constructors

    /// <inheritdoc />
    public BusinessRulesException()
      : base(_message) {
    }

    /// <inheritdoc />
    public BusinessRulesException(string message)
      : base(message) {
    }

    /// <inheritdoc />
    public BusinessRulesException(string message, System.Exception inner)
      : base(message, inner) {   
    }

    #endregion

  }
}
