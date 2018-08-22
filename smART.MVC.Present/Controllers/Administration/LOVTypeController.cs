using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Master_LOVType)]
    public class LOVTypeController : BaseFormController<LOVTypeLibrary, LOVType>
    {
        #region Constructor

      public LOVTypeController() : base("~/Views/Administration/LOVType/_List.cshtml", new string[] {"ParentType" }, new string[] { "LOV"}, new string[] { "ParentType" }) {
      }

        #endregion Constructor

        #region Override Methods

        protected override void SaveChildEntities(string[] childEntityList, LOVType entity)
        {
            foreach (string ChildEntity in childEntityList)
            {
                switch (ChildEntity)
                {
                    #region /* Case Statements - All child grids */

                    case "LOV":
                        if (Session[ChildEntity] != null)
                        {
                            LOVLibrary lovLibrary = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                            IEnumerable<LOV> resultList = (IList<LOV>)Session[ChildEntity];
                            foreach (LOV lov in resultList)
                            {
                                lov.LOVType = new LOVType { ID = entity.ID };
                                if (lov.Parent != null) {
                                  lov.Parent = new LOV {
                                    ID = lov.Parent.ID
                                  };
                                }
                                lovLibrary.Add(lov);
                            }
                        }
                        break;

                    #endregion
                }
            }
        }

        protected override void ValidateEntity(smART.ViewModel.LOVType entity) {
          ModelState.Clear();
          if (string.IsNullOrEmpty(entity.LOVType_Name))
            ModelState.AddModelError("LOVType_Name", "LOV Type Name field is required.");
        }

        #endregion Override Methods

        #region Deleted

        //protected override void SaveChildEntities(string[] childEntityList, Item entity)
        //{
        //    foreach (string ChildEntity in childEntityList)
        //    {
        //        switch (ChildEntity)
        //        {
        //            #region /* Case Statements - All child grids */

        //            case "LOV":
        //                if (Session[ChildEntity] != null)
        //                {
        //                    LOVLibrary lovLibrary = new LOVLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        //                    IEnumerable<LOV> resultList = (IList<LOV>)Session[ChildEntity];
        //                    foreach (LOV lov in resultList)
        //                    {
        //                        lov.LOVType = new LOVType { ID = entity.ID };
        //                        lovLibrary.Add(lov);
        //                    }
        //                }
        //                break;

        //            #endregion
        //        }
        //    }
        //}

        //public override ActionResult _Index()
        //{
        //    int totalRows = 0;
        //    IEnumerable<LOVType> resultList = ((ILibrary<LOVType>)Library).GetAllByPaging(out totalRows, 1, 20, "", "Asc"); ;
        //    return View("~/Views/Administration/LOVType/_List.cshtml", resultList);
        //}

        #endregion Deleted

    }
}