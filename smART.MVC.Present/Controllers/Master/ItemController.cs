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
using smART.MVC.Present.Helpers;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Master_Item)]
    public class ItemController : BaseFormController<ItemLibrary, Item>
    {
        #region /* Constructors */

        public ItemController() : base("~/Views/Master/Item/_List.cshtml",null,new string[] { "ItemNotes","ItemAttachment"}) { }

        #endregion /* Constructors */

        #region /* Override methods */

        //[HttpPost]
        //public override ActionResult Save(Item entity)   //Added By dharemndra to fix issue #130 of excel sheet.
        //{
        //    ModelState.Clear();
        //    if (string.IsNullOrWhiteSpace(entity.Item_Category))
        //    {
        //        ModelState.AddModelError("Item_Category","Item Category is Required.");
        //    }
        //    if (string.IsNullOrWhiteSpace(entity.Item_Group))
        //    {
        //        ModelState.AddModelError("Item_Group", "Item Group is Required.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if (entity.ID == 0)
        //        {
        //            entity = Library.Add(entity);

        //            // Also save all relevant child records in database
        //            if (ChildEntityList != null)
        //            {
        //                SaveChildEntities(ChildEntityList, entity);
        //                ClearChildEntities(ChildEntityList);
        //            }
        //        }
        //        else
        //        {
        //            Library.Modify(entity);
        //        }
        //        ModelState.Clear();
        //    }
        //    return Display(entity);
        //}

        protected override void ValidateEntity(Item entity) {
          ModelState.Clear();
          if (string.IsNullOrWhiteSpace(entity.Item_Category)) {
            ModelState.AddModelError("Item_Category", "Item Category is Required.");
          }
          if (string.IsNullOrWhiteSpace(entity.Item_Group)) {
            ModelState.AddModelError("Item_Group", "Item Group is Required.");
          }
        }
        protected override void SaveChildEntities(string[] childEntityList, Item entity)
        {
            foreach (string ChildEntity in childEntityList)
            {
                switch (ChildEntity)
                {
                    #region /* Case Statements - All child grids */                   
                    
                    case "ItemNotes":
                        if (Session[ChildEntity] != null)
                        {
                            ItemNotesLibrary itemNotesLibrary = new ItemNotesLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                            IEnumerable<ItemNotes> resultList = (IList<ItemNotes>)Session[ChildEntity];
                            foreach (ItemNotes itemNote in resultList)
                            {
                                itemNote.Parent = new Item { ID = entity.ID };
                                //itemNote.Notes = System.Web.HttpUtility.HtmlDecode(itemNote.Notes);
                                itemNotesLibrary.Add(itemNote);
                            }
                        }
                        break;

                    case "ItemAttachment":
                        if (Session[ChildEntity] != null)
                        {
                            ItemAttachmentLibrary itemLibrary = new ItemAttachmentLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                            IEnumerable<ItemAttachment> resultList = (IList<ItemAttachment>)Session[ChildEntity];
                            string destinationPath;
                            string sourcePath;
                            FilelHelper fileHelper = new FilelHelper();
                            foreach (ItemAttachment item in resultList)
                            {
                                destinationPath = fileHelper.GetSourceDirByFileRefId(item.Document_RefId.ToString());// Path.Combine(Configuration.GetsmARTDocPath(), item.Document_RefId.ToString());
                                sourcePath = fileHelper.GetTempSourceDirByFileRefId(item.Document_RefId.ToString()); // Path.Combine(Configuration.GetsmARTTempDocPath(), item.Document_RefId.ToString());
                                item.Document_Path = fileHelper.GetFilePath(sourcePath);
                                fileHelper.MoveFile(item.Document_Name, sourcePath, destinationPath);  

                                item.Parent = new Item { ID = entity.ID };
                                itemLibrary.Add(item);                              
                            }
                        }
                        break;

                    #endregion
                }
            }
        }
        
        #endregion /* Override methods */

        #region Deleted
        //public override ActionResult _Index()
        //{
        //    int totalRows = 0;
        //    IEnumerable<Item> resultList = ((ILibrary<Item>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc");
        //    return View("~/Views/Master/Item/_List.cshtml", resultList);
        //}
        #endregion Deleted
    }
}