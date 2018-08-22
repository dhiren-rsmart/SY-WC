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

  public class QBLogController : ApiController {

    // GET api/values
    [HttpGet]
    public IEnumerable<QBLog> GetAll() {
        QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetUnPostedParentQBLogs();
    }


    // GET api/values
    [HttpGet]
    public IEnumerable<QBLog> GetUnPostedParentQBLogs() {
      try {
          QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        return lib.GetUnPostedParentQBLogs();
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "GetUnPostedParentQBLogs", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }


    // GET api/values/5
    [HttpGet]
    public QBLog Get(int? id) {
        QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetByID(id.Value.ToString());
    }

    // GET api/values/5
    [HttpGet]
    public IEnumerable<QBLog> GetByParentId(int id) {
      try {
        QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        return lib.GetByParentID(id);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "GetByParentId", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }


    // POST api/values
    public void Post([FromBody]QBLog value) {
    }

    // PUT api/values/5
    [HttpPut]
    public void Put(int id, [FromBody]ServiceQBLog value) {
      try {
          QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());    
        QBLog qbLog = lib.GetByID(id.ToString());
        qbLog.Posting_Date = value.Date;
        qbLog.QB_Ref_No = value.QBRefNo;
        qbLog.Status =value.Status ;
        qbLog.Status_Remarks = value.StatusRemark ;
        lib.Modify(qbLog);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateQBRef", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
    }

    // PUT api/values/5
    [HttpPut]
    public void UpdateQBRef(int id, string qbRef, string status, string remarks) {
      try {
          QuickBookLibrary lib = new QuickBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        QBLog qbLog = lib.GetByID(id.ToString());
        qbLog.Posting_Date = DateTime.Now;
        qbLog.QB_Ref_No = qbRef;
        qbLog.Status = status;
        qbLog.Status_Remarks = remarks;
        lib.Modify(qbLog);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateQBRef", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
      }
    }

    // DELETE api/values/5
    public void Delete(int id) {
    }

  }
}
