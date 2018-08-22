// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/04/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using System.IO;
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers.Transaction
{
    [Feature(EnumFeatures.Transaction_ScaleAttachment)]
    public class ScaleAttachmentsController : AttachmentGridController<ScaleAttachmentsLibrary, ScaleAttachments, Scale>
    {
        public ScaleAttachmentsController()
            : base("ScaleAttachments", new string[] { "Parent" })
        {
        }

        public ActionResult _ShowScaleItemAttachments(int scaleDetailId, int scaleId)
        {
            ScaleAttachmentsLibrary lib = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            int totalRows = 0;
            ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
            IEnumerable<ScaleAttachments> resultList = lib.GetAttachmentsWithPagingByRefIdAndRefType(EnumAttachmentRefType.Item, scaleDetailId,
                                                                                     scaleId,
                                                                                     out  totalRows,
                                                                                     1,
                                                                                     ViewBag.PageSize,
                                                                                     "",
                                                                                     "Asc",
                                                                                     IncludePredicates);
            resultList = UpdateAttachmentsImagePath(resultList);

            return View("~/Views/Transaction/QScale/_ScaleItemAttachments.cshtml", resultList);
        }


        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _ShowScaleItemAttachments(GridCommand command, int scaleDetailId, int scaleId)
        {
            int totalRows = 0;
            ScaleAttachmentsLibrary lib = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<ScaleAttachments> resultList = lib.GetAttachmentsWithPagingByRefIdAndRefType(
                                                                                                      EnumAttachmentRefType.Item, scaleDetailId,
                                                                                                      scaleId,
                                                                                                      out totalRows,
                                                                                                      command.Page,
                                                                                                      command.PageSize,
                                                                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                      command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                        IncludePredicates,
                                                                                                      (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                                      );
            resultList = UpdateAttachmentsImagePath(resultList);
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override void ChildGrid_OnAdding(ScaleAttachments entity)
        {
            entity.Ref_Type = (int)EnumAttachmentRefType.General;
        }
         
    }
}
