using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a custom exception class inherited from base exception class. 
  /// It is used to pass exception when operation is invalid.
  /// </summary>
  [Serializable]
  public class InvalidOperationException : BaseException {

    #region Local Members

    private static readonly string _message = "Invalid operation.";

    #endregion Local Members

    #region Constructors


    /// <summary>
    /// 
    /// </summary>
    public InvalidOperationException()
      : base(_message, null) {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public InvalidOperationException(string message)
      : base(message, null) {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="innerException"></param>
    public InvalidOperationException(Exception innerException)
      : base(_message, innerException) {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <param name="opName"></param>
    public InvalidOperationException(string message, Exception innerException, string opName = "")
      : base(message, innerException) {
      SetData(opName);
    }

    #endregion Constructors

    #region Public

    /// <summary>
    /// This method set operation name in excetpion.
    /// </summary>
    /// <param name="opName">Operation name.</param>
    public void SetData(string opName) {
      if (!string.IsNullOrEmpty(opName))
        SetData("OperationName", opName);
    }

    /// <summary>
    /// This Method get operation name from exception.
    /// </summary>
    public void GetData() {
      GetData("OperationName");
    }

    #endregion Public

  }
}
