using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using smART.Common;
namespace smART.MVC.Present.Helpers
{
    public class RoleHelper
    {
        public IEnumerable<Role> GetRoles()
        {
            RoleLibrary lib = new RoleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetAll();
        }

        public IEnumerable<RoleFeature> GetRoleFeatures(int roleID)
        {
            RoleLibrary lib = new RoleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetRoleFeatures(roleID);
        }

        public bool SetRoleFeatures(int roleID, IEnumerable<RoleFeature> roleFeatures)
        {
            RoleLibrary lib = new RoleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());

            List<RoleFeature> updatedFeatures = new List<RoleFeature>();
            List<RoleFeature> deletedFeatures = new List<RoleFeature>();

            foreach (RoleFeature roleFeature in roleFeatures)
            {
                if (!roleFeature.ViewAccessInd && !roleFeature.EditAccessInd && !roleFeature.DeleteAccessInd && !roleFeature.NewAccessInd)
                    deletedFeatures.Add(roleFeature);
                else
                    updatedFeatures.Add(roleFeature);
            }
            bool delRetVal = lib.DeleteRoleFeatures(roleID, deletedFeatures);
            bool updateRetVal = lib.SetRoleFeatures(roleID, updatedFeatures);

            return delRetVal && updateRetVal;
        }
    }
}