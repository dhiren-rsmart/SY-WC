//// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
//// Main Author: Sanjeev Khanna
//// Last Major Update: 11/04/2011

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using smART.Library;
//using smART.ViewModel;
//using Telerik.Web.Mvc;
//using System.IO;
//using smART.Common;

//namespace smART.MVC.Present.Controllers.Transaction {

//  [Feature(EnumFeatures.Transaction_ScaleAttachment)]
//  public class QScaleAttachmentsController : ScaleAttachmentsController {

//    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
//      int totalRows = 0;
//      IEnumerable<ScaleAttachments> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

//      resultList = ((IParentChildLibrary<ScaleAttachments>) Library).GetAllByPagingByParentID(out totalRows, int.Parse("4934"), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
//      resultList = base.UpdateAttachmentImagePath(resultList);
//      ViewBag.MessageId = Guid.NewGuid().ToString();
//      return View(new GridModel {Data = resultList, Total = totalRows});
//    }

//  }
//}
