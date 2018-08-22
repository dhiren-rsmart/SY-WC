using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;
using Telerik.Web.Mvc;
using System.IO;
using System.Diagnostics;

namespace smART.MVC.Present.Controllers {
  [Feature(EnumFeatures.Master_BankBook)]
  public class BankController : PartyChildGridController<BankLibrary, Bank> {
    public BankController() : base("Bank", new string[] { "Party" }) { }

    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public ActionResult _BankByPartyType(GridCommand command, string partytype) {
      int totalRows = 0;
      IEnumerable<Bank> resultList = new BankLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByPartyTypeWithPaging(partytype,
                                                                                                      out totalRows, command.Page,
                                                                                                      (command.PageSize == 0 ? 20 : command.PageSize),
                                                                                                      "", "Asc", null,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
      return View(new GridModel { Data = resultList, Total = totalRows });
    }

    //[HttpGet]
    //public ActionResult UpdateQBBankBalance() {
    //  string success = string.Empty;
    //  try {
    //    string appPath = ConfigurationHelper.GetsmARTQBIntegrationBatchFilePath();
    //    string fullPath = Path.Combine(appPath, "smART.QB.exe");
    //    if (!System.IO.File.Exists(fullPath))
    //      throw new System.IO.FileNotFoundException("'smART.QB.exe' not found at '" + fullPath + "' location.");
    //    //ProcessStartInfo startInfo = new ProcessStartInfo();
    //    //startInfo.FileName = fullPath;
    //    //startInfo.Arguments = "1";
    //    //Process process = Process.Start(startInfo);
    //    //if (process == null)
    //    //  success= "Bank balance update failed";
    //    success = Server.MapPath("");
    //    // AppDomain.CurrentDomain.BaseDirectory ;// fullPath;
    //  }
    //  catch (Exception ex) {
    //    success = ex.Message;
    //  }
    //  return Json(new { Sucess = success }, JsonRequestBehavior.AllowGet);
    //}


    [HttpGet]
    public ActionResult GetQBBankBalanceUpdateBatchFilePath() {
      string path = string.Empty;
      string appPath = ConfigurationHelper.GetsmARTQBIntegrationBatchFilePath();
      path = Path.Combine(appPath, "UpdateBankBalance.bat");
      if (!System.IO.File.Exists(path))
        path = "";
      return Json(new { Sucess = path }, JsonRequestBehavior.AllowGet);
    }
  }
}
