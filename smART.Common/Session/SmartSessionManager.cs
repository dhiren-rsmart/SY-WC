using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace smART.Common {

  public class SmartSessionManagxer {

    #region local Variables

    // A singleton instance of smARTSessionManager.
    private static SmartSessionManagxer _instance = null;
    // A dictionary of all session against uniquely identify session id.
    private Dictionary<string, SmartSession> _sessions = new Dictionary<string, SmartSession>();

    #endregion

    #region Constructor

    /// <summary>
    /// Provide a singleton instance of smARTSessionManager class.
    /// </summary>   
    public static SmartSessionManagxer Instance {
      get {
        if (_instance == null)
          _instance = new SmartSessionManagxer();
        return _instance;
      }
    }

    #endregion

    #region Private Methods

    // Add smARTSession instance to dictionary collection by given session id.
    private void AddSession(string sessionId, SmartSession session) {
      // If session id already exist in dictionary, remove it, and add current session information.
      if (_sessions.Keys.Contains(sessionId))
        _sessions.Remove(sessionId);
      _sessions.Add(sessionId, session);
    }

    // Remove the smARTSession instance from dictionary collection.
    private void RemoveSession(string sessionId) {
      _sessions.Remove(sessionId);
    }

    // Return the current active session count from session dictionary.
    private int GetSessionCount() {
      return _sessions.Count();
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value of the type string.
    /// <para>
    /// This delegate handler subscribe by the client application to return client session id.
    /// </para>
    /// </summary>
    public Func<string> GetSessionId;

    /// <summary>
    /// This method is called when user login to initialize new session object 
    /// and add an entry into dictonary against unique session id.
    /// </summary>
    /// <param name="userLoginId">Logged-in user id.</param>
    /// <param name="userName">Logged-in user name.</param>
    /// <param name="sessionId">Unique identifier for the session.</param>
    public void Login(Guid userLoginId, string userName, string sessionId) {
      SmartSession session = new SmartSession(userLoginId, userName);
      AddSession(sessionId, session);
    }

    /// <summary>
    /// This method is called when user logout
    /// to remove the user session object from dictonary.
    /// </summary>
    /// <param name="sessionId">Session id to match smARTSession.</param>
    public void Logout(string sessionId) {
      RemoveSession(sessionId);
    }

    /// <summary>
    /// Get session object by session id.
    /// </summary> 
    /// <param name="sessionId">Session id to match smARTSession instance.</param>
    /// <returns>An instance of smARTSession object to match session id.</returns>
    public SmartSession GetSession(string sessionId) {
      SmartSession session = (SmartSession)_sessions[sessionId];
      return session;
    }

    /// <summary>
    /// Return the current session object.
    /// </summary>
    /// <returns>An instance current smARTSession object.</returns>
    public SmartSession GetCurrentSession() {
      // Call client side handler method to get session id of current request.
      string sessionId = GetSessionId();
      return GetSession(sessionId);
    }

    /// <summary>
    /// Return the logged-in userid.
    /// </summary>
    public Guid GetLoginUserId {
      get {
        return _sessions[GetSessionId()].UserId;
      }
    }

    
    /// <summary>
    /// It returns all session object into collection.
    /// </summary>
    /// <returns>Collection of session object.</returns>
    public ICollection GetAllSessions() {
      return _sessions.Values;
    }

    #endregion

  }
}
