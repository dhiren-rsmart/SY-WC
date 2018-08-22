using System;
using System.Collections.Generic;

namespace smART.Common {

  /// <summary>
  /// This class contains session property of TenantId, Tenant Name, User Id and User Name.
  /// </summary>
  public class SmartSession {

    #region Constructor

    /// <summary>
    /// Initialize new instance of smARTSession class.
    /// </summary>
    public SmartSession() {
    }

    /// <summary>
    /// Initialize new instance of smARTSession class.
    /// </summary>
    /// <param name="userLoginId">Logged-in user id.</param>
    /// <param name="userName">Logged-in user name.</param>    
    public SmartSession(Guid userLoginId, string userName) {
      UserId = userLoginId;
      UserName = userName;
      LoginTime = DateTime.Now;
    }

    #endregion Constructor

    #region Public Properties

    // Collection of session properties.
    private Dictionary<string, object> _loggerExtendedProperties = null;

    /// <summary>
    /// It provides dictonary of session properties.
    /// </summary>
    public Dictionary<string, object> LoggerExtendedProperties {
      get {
        if (_loggerExtendedProperties == null)
          GenerateLoggerExtendedProperties();
        return _loggerExtendedProperties;
      }
    }


    /// <summary>
    /// The logged-in user id.
    /// </summary>
    public Guid UserId {
      get;
      set;
    }

    /// <summary>
    /// Logged-in user name.
    /// </summary>
    public string UserName {
      get;
      set;
    }

    /// <summary>
    /// User's login date and time.
    /// </summary>
    public DateTime LoginTime {
      get;
      private set;
    }

    #endregion

    #region Private Methods

    // This method provide extended property for a session object.
    private void GenerateLoggerExtendedProperties() {
      if (_loggerExtendedProperties != null)
        _loggerExtendedProperties.Clear();
      else
        _loggerExtendedProperties = new Dictionary<string, object>();
      _loggerExtendedProperties.Add("UserId", UserId);
      _loggerExtendedProperties.Add("UserName", UserName);
    }

    #endregion

  }
}
