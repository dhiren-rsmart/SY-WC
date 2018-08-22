using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using smART.Library;
using smART.ViewModel;
using smART.Common;


namespace smART.MVC.Service.Controllers {

  public class BaseController<TLibrary, TEntity> : ApiController
    where TLibrary : ILibrary<TEntity>, new()
    where TEntity : ViewModelBase, new() {

    protected TLibrary Library {
      get;
      private set;
    }

    public string ConString {
      get {
          return ConfigurationHelper.GetsmARTDBContextConnectionString();
      }
    }

    protected string[] _includePredicates;

    public BaseController() {
      Library = new TLibrary();
      Library.Initialize(ConString);
    }


    // GET api/values
    [HttpGet]
    public virtual IEnumerable<TEntity> Get() {
      try {
        return Library.GetAll(_includePredicates);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    // GET api/values/5
    [HttpGet]
    public virtual TEntity Get(int id) {
      try {
        return Library.GetByID(id.ToString(), _includePredicates);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    // POST api/values
    public virtual void Post([FromBody]TEntity value) {
      try {
        Library.Add(value);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateQBRef", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
    }

    // PUT api/values/5
    [HttpPut]
    public void Put(int id, [FromBody]TEntity value) {
      try {
        Library.Modify(value, _includePredicates);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateQBRef", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
    }

    // DELETE api/values/5
    public void Delete(int id) {
      try {
        Library.Delete(id.ToString(), _includePredicates);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateQBRef", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
    }

  }
}
