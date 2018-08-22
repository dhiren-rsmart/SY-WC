using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Diagnostics;

namespace smART.Common {
 
  /// <summary>
  /// This class provide methods to handle policy by EntLib exception handling block.
  /// The application create singleton object of this type at runtime.
  /// </summary>
  public class PolicyHandler {

    #region Local Members

    static ExceptionPolicyFactory _exceptionPolicyFactory;

    #endregion Local Members

    #region Singleton Instance

    private static PolicyHandler _instance = null;
    /// <summary>
    /// Provide singleton instance of HandlePolicy class.
    /// </summary>   
    public static PolicyHandler Instance {
      get {
        if (_instance == null)
          _instance = new PolicyHandler();
        return _instance;
      }
    }

    #endregion Singleton Instance

    #region Constructors

    /// <summary>
    /// Initialize a new instance of HandlePolicy class.
    /// </summary>
    private PolicyHandler() {
      Initialize();
    }

    #endregion Constructors

    #region Private

    /// <summary>
    /// Initialize the variables.
    /// </summary>
    private void Initialize() { 
      _exceptionPolicyFactory = new ExceptionPolicyFactory();
    }

    #endregion Private

    #region Public

    /// <summary>
    /// This method handle the policy by enterprise library exception handling block
    /// </summary>
    /// <param name="policyName">Policy Name.</param>
    /// <param name="ex">Exception</param>
    /// <returns></returns>
    public bool Handle(string policyName, ref Exception ex) {
      bool rethrow = false;
      ExceptionPolicyImpl exceptImpl = _exceptionPolicyFactory.Create(policyName);
      rethrow = exceptImpl.HandleException(ex);
      if (rethrow) {
        throw ex;
      }
      return rethrow;
    }
    #endregion Public
  }
}
