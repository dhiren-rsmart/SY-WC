using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using smART.Library;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Service {

  public class BankController : ApiController {

    // GET api/values
    [HttpGet]
    public IEnumerable<Bank> Get() {
      try {
          BankLibrary lib = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        return lib.GetAll();
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    // GET api/values/5
    [HttpGet]
    public Bank Get(int id) {
      try {
          BankLibrary lib = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        return lib.GetByID(id.ToString());
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "Get", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    // GET api/values/5
    [HttpGet]
    public IEnumerable<Bank> GetOrganizationBanks() {
      try {
          BankLibrary lib = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        return lib.GetOrganizationBank();
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "GetOrganizationBanks", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");
        return null;
      }
    }

    // POST api/values
    public void Post([FromBody]string value) {
    }

    // PUT api/values/5
    [HttpPut]
    public void PUTBankBalance(int id, decimal value) {
      try {
          BankLibrary lib = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        Bank bank = lib.GetByID(id.ToString());
        bank.Closing_Balance = value;
        bank.Last_Updated_Date = DateTime.Now;
        lib.UpdateBalance(b => b.ID == bank.ID, bank);
      }
      catch (Exception ex) {
        string details = string.Format("Method: {1} {0} Message: {2} {0} Stack Trace: {3}", System.Environment.NewLine, "UpdateBankBalance", ex.Message, ex.StackTrace.ToString());
        smART.Common.MessageLogger.Instance.LogMessage(ex, details, Common.Priority.High, 0, System.Diagnostics.TraceEventType.Error, "Service Error", "Service");        
      }
    }

    // DELETE api/values/5
    public void Delete(int id) {
    }
  }
}