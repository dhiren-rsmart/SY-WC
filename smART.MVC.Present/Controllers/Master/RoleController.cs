using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;

namespace smART.MVC.Present.Controllers.Master
{
    [Feature(EnumFeatures.Master_Role)]
    public class RoleController : BaseFormController<RoleLibrary, Role>
    {
        #region Constructor

        public RoleController() : base("~/Views/Master/Role/_List.cshtml", null, new string[] { "RoleFeature" }) { }

        #endregion Constructor

        #region Override Methods

        protected override void SaveChildEntities(string[] childEntityList, Role entity)
        {
            foreach (string ChildEntity in childEntityList)
            {
                switch (ChildEntity)
                {
                    #region /* Case Statements - All child grids */

                    case "RoleFeature":
                        if (Session[ChildEntity] != null)
                        {
                            RoleFeatureLibrary roleFeatureLibrary = new RoleFeatureLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                            IEnumerable<RoleFeature> resultList = (IList<RoleFeature>)Session[ChildEntity];
                            foreach (RoleFeature data in resultList)
                            {
                                data.Role = new Role { ID = entity.ID };
                                roleFeatureLibrary.Add(data);
                            }
                        }
                        break;

                    #endregion
                }
            }
        }

        protected override void DeleteChildEntities(string[] childEntityList, string parentID) {
          foreach (string ChildEntity in childEntityList) {
            switch (ChildEntity) {
              #region /* Case Role - All child grids */
              case "RoleFeature":
                if (Convert.ToInt32(parentID) > 0) {
                  RoleFeatureLibrary roleFeatureLibrary = new RoleFeatureLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                  IEnumerable<RoleFeature> resultList = roleFeatureLibrary.GetAllByParentID(Convert.ToInt32(parentID));
                  foreach (RoleFeature data in resultList) {
                    roleFeatureLibrary.Delete(data.ID.ToString());
                  }
                }
                break;
              #endregion
            }
          }
        }

        #endregion Override Methods

    }
}
