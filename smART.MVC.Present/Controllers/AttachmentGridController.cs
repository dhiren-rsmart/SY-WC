// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/13/2011

using System;
using System.Collections.Generic;
// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/13/2011

using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.MVC.Present.Extensions;
using Omu.ValueInjecter;
using System.IO;
using smART.MVC.Present.Helpers;
using smART.Common;

namespace smART.MVC.Present.Controllers {
  public abstract class AttachmentGridController<TLibrary, TEntity, TParentEntity> : BaseGridController<TLibrary, TEntity>
    where TLibrary : ILibrary<TEntity>, IParentChildLibrary<TEntity>, new()
    where TEntity : AttachmentEntity<TParentEntity>, new()
    where TParentEntity : BaseEntity, new() {

    #region /* Local Variables */

    FilelHelper _fileHelper;

    #endregion /* Local Variables */

    #region /* Constructors */

    public AttachmentGridController(string sessionName, string[] includePredicates = null)
      : base(sessionName, includePredicates) {
      _fileHelper = new FilelHelper();
    }

    public AttachmentGridController(string dbContextConnectionString, string sessionName, string[] includePredicates = null)
      : base(dbContextConnectionString, sessionName, includePredicates) {
      _fileHelper = new FilelHelper();
    }

    #endregion

    #region /* Supporting Actions - Display Actions, Insert Action, Delete Action */

    protected override ActionResult Display(GridCommand command, string id, bool isNew) {
      int totalRows = 0;
      IEnumerable<TEntity> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

      if (isNew || id == "0") {
        resultList = TempEntityList;
        totalRows = TempEntityList.Count;
      }
      else {
        resultList = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
      }
      resultList = UpdateAttachmentsImagePath(resultList);

      ViewBag.MessageId = Guid.NewGuid().ToString();
      return View(new GridModel { Data = resultList, Total = totalRows });
    }


    public IEnumerable<TEntity> UpdateAttachmentsImagePath(IEnumerable<TEntity> results) {
      foreach (var item in results) {
       UpdateAttachmentImagePath(item);
      }
      return results;
    }

    private void UpdateAttachmentImagePath(TEntity item) {
        string[] types = smART.Common.ConfigurationHelper.GetsmARTAttachmentImageTypes().Split(','); 
      if (types!= null && types.Any(s => s.Contains( item.Document_Type.ToLower()))){
        if (item.ID > 0)
          //item.Image = string.Concat("../../../Content/smARTDocPath/Attachments/", item.Document_RefId.ToString(), "/", item.Document_Name);
         item.Image = string.Concat(ConfigurationHelper.GetsmARTDocUrl(), "/" , "Attachments" ,"/", item.Document_RefId.ToString(), "/", item.Document_Name);
        else
          item.Image = string.Concat(ConfigurationHelper.GetsmARTDocUrl(), "/", "Temp", "/", item.Document_RefId.ToString(), "/", item.Document_Name);
          //item.Image = string.Concat("../../../Content/smARTDocPath/Temp/Attachments/", item.Document_RefId.ToString(), "/", item.Document_Name);
      }
      else
        item.Image = "";
    }

    protected override ActionResult Display(GridCommand command, TEntity entity, bool isNew = false) {
      if (entity.Parent != null && entity.Parent.ID != 0)
        return Display(command, entity.Parent.ID.ToString(), isNew);
      else
        return base.Display(command, entity, isNew);

    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Insert(TEntity data, GridCommand command, bool isNew = false) {
      try {
        string docRefId = Convert.ToString(TempData["DocRefId"]);
        
        if (string.IsNullOrEmpty(docRefId)) {
          ModelState.AddModelError("docRefId", "Error occured on save document.");
        }

        if (ModelState.IsValid) {

          data.Document_RefId = Guid.Parse(docRefId);
          data = UpdateFileInfo(data, _fileHelper.GetTempSourceDirByFileRefId(docRefId));
          data.Last_Updated_Date = DateTime.Now;
          data.Updated_By = User.Identity.Name; // HttpContext.Current.User.Identity.Name;

          if (isNew) {
            data.Document_Path = _fileHelper.GetFilePath(_fileHelper.GetTempSourceDirByFileRefId(docRefId)); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefId, data.Document_Name);
            TempEntityList.Add(data);
          }
          else {
            data.Document_Path = _fileHelper.GetFilePath(_fileHelper.GetSourceDirByFileRefId(docRefId));// Path.Combine(Configuration.GetsmARTDocPath(), docRefId, data.Document_Name);
            data = Library.Add(data);

            string destinationPath = _fileHelper.GetSourceDirByFileRefId(docRefId);// Path.Combine(Configuration.GetsmARTDocPath(), docRefId);
            string sourcePath = _fileHelper.GetTempSourceDirByFileRefId(docRefId); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefId);

            _fileHelper.MoveFile(data.Document_Name, sourcePath, destinationPath);
          }
          UpdateAttachmentImagePath(data);
          TempData["DocRefId"] = null;
        }
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      return Display(command, data, isNew);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
      string filePath;
      string fileName;
      try {
        if (isNew) {

          //TODO: Delete entity with id
          TEntity entity = TempEntityList.SingleOrDefault(m => m.ID == int.Parse(id));
          filePath = _fileHelper.GetTempSourceDirByFileRefId(entity.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTTempDocPath(), entity.Document_RefId.ToString());
          fileName = entity.Document_Name;
          TempEntityList.Remove(entity);

        }
        else {
          TEntity entity = Library.GetByID(id);

          filePath = _fileHelper.GetSourceDirByFileRefId(entity.Document_RefId.ToString());
          fileName = entity.Document_Name;

          Library.Delete(id);

        }

        _fileHelper.RemoveFile(fileName, filePath);
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      if (string.IsNullOrEmpty(MasterID))
        return Display(command, isNew);
      else
        return Display(command, MasterID, isNew);
    }


    [HttpPost]
    [GridAction(EnableCustomBinding = true)]
    public override ActionResult _Update(TEntity data, GridCommand command, bool isNew = false) {
      UpdateAttachmentImagePath(data);
      return Display(command, data, isNew);
    }


    #endregion /* Supporting Actions - Display Actions, Insert Action, Delete Action */

    #region /* Supporting File Actions  */

    [HttpPost]
    public ActionResult Save(IEnumerable<HttpPostedFileBase> attachments, string docRefID) {
      try {
        if (docRefID == Guid.Empty.ToString())
          docRefID = Guid.NewGuid().ToString();

        foreach (var file in attachments) {
          // Some browsers send file names with full path. We only care about the file name.
          var fileName = Path.GetFileName(file.FileName);
          var destinationPath = _fileHelper.GetTempSourceDirByFileRefId(docRefID); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefID);

          _fileHelper.SaveFile(file, destinationPath);

          TempData["DocRefId"] = docRefID;
        }
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      // Return an empty string to signify success
      return Content("");
    }

    [HttpPost]
    public ActionResult Remove(string[] fileNames, string docRefID) {
      try {
        if (docRefID == Guid.Empty.ToString())
          docRefID = Convert.ToString(TempData["docRefId"]); //GetFileRefIdFromTempData();

        foreach (var fullName in fileNames) {
          var fileName = Path.GetFileName(fullName);
          var destinationPath = _fileHelper.GetTempSourceDirByFileRefId(docRefID); // Path.Combine(Configuration.GetsmARTTempDocPath(), docRefID);
          _fileHelper.RemoveFile(fullName, destinationPath);
          TempData["DocRefId"] = null;
        }
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
      }
      // Return an empty string to signify success
      return Content("");
    }


    //[HttpGet]
    //public ActionResult OpenDocument(string id)
    //{
    //    if (!string.IsNullOrEmpty(id) && Guid.Empty.ToString() != id)
    //    {
    //        string filePath = _fileHelper.GetFilePathByFileRefId(id);
    //        if (string.IsNullOrEmpty(filePath ))
    //           filePath = _fileHelper.GetTempFilePathByFileRefId(id);

    //        System.Diagnostics.Process.Start(filePath);
    //    }
    //    return Content("");
    //}


    [HttpGet]
    public FilePathResult OpenDocument(string id) {
      try {
        string filePath = string.Empty;
        if (!string.IsNullOrEmpty(id) && Guid.Empty.ToString() != id) {
          filePath = _fileHelper.GetFilePathByFileRefId(id);
          if (string.IsNullOrEmpty(filePath))
            filePath = _fileHelper.GetTempFilePathByFileRefId(id);
        }
        return File(filePath, "text/plain", _fileHelper.GetFileInfo(filePath).Name);
      }
      catch (Exception ex) {
        ModelState.AddModelError("Error", ex.Message);
        return null;
      }
    }


    #endregion /* Supporting File Actions  */

    #region /* Supporting File Methods */

    private string GetFileRefIdFromTempData() {
      string DocRefId = Guid.Empty.ToString();

      if (TempData["Attachments"] != null) {
        TEntity entity = (TEntity)TempData["Attachments"];
        if (entity != null) {
          DocRefId = entity.Document_RefId.ToString();
        }
      }
      return DocRefId;
    }

    private TEntity UpdateFileInfo(TEntity data, string sourceDirPath) {
      if (data != null) {
        string filePath = _fileHelper.GetFilePath(sourceDirPath);

        FileInfo fileInfo = _fileHelper.GetFileInfo(filePath);

        if (fileInfo != null) {
          data.Document_Name = fileInfo.Name;
          data.Document_Type = fileInfo.Extension.Replace(".", "");
          data.Mime_Type = "";
          data.Document_Size = fileInfo.Length;
        }
      }
      return data;
    }

    #endregion /* Supporting File Methods */
  }
}