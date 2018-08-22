using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace smART.Common {

  /// <summary>
  /// It is a base exception class inherited from System.ApplicationException class.
  /// All other custom exception will be inherited from this class.
  /// </summary>
  [Serializable]
  public class BaseException : System.ApplicationException {

    #region Local Members

    private static readonly string _message = "An exception occurred.";

    #endregion Local Members

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the BaseException class.
    /// </summary>
    public BaseException()
      : base(_message) {
    }

    /// <summary>
    /// Initializes a new instance of the BaseException class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describe the error.</param>
    public BaseException(string message)
      : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the BaseException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>   
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference
    /// if no inner exception is specified.
    /// </param>
    public BaseException(System.Exception inner)
      : base(_message, inner) {
    }


    /// <summary>
    /// Initializes a new instance of the BaseException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describe the error.</param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference
    /// if no inner exception is specified.
    /// </param>
    public BaseException(string message, System.Exception inner)
      : base(message, inner) {
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add extra information in excetpion.
    /// </summary>
    /// <param name="key">Extra information key.</param>
    /// <param name="data">Extra information of.</param>    
    public virtual void SetData(string key, object data) {
      // This test is done to protect from the error of re-addition of a key.
      // We will overwite the data. This is an arbitrary decision.
      if (this.Data.Contains(key))
        this.Data[key] = data;
      else
        this.Data.Add(key, data);
    }

    /// <summary>
    /// Get extra information from exception.
    /// </summary>
    /// <param name="key">Extra information key.</param>
    /// <returns></returns>
    public object GetData(string key) {
      object data = null;
      if (this.Data.Contains(key))
        data = this.Data[key];

      return data;
    }

    #endregion Public Methods

  }
}
