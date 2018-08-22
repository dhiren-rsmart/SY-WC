using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;
using smART.Library;
using smART.Common;

namespace smART.MVC.Present.Helpers
{
    public class EmployeeHelper
    {
        public IEnumerable<RoleFeature> GetRoleFeaturesForEmployee(int UserID)
        {
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetRoleFeaturesForEmployee(UserID);
        }

        public IEnumerable<Feature> GetFeaturesForEmployee(int UserID)
        {
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetFeaturesForEmployee(UserID);
        }

        public Employee GetEmployeeByUsername(string username)
        {
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetEmployeeByUsername(username);
        }

        public IEnumerable<Role> GetRolesForEmployee(int UserID)
        {
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.GetRolesForEmployee(UserID);
        }

        public bool SaveEmployeeRoles(int employeeID, int[] roleIDs)
        {
            EmployeeLibrary lib = new EmployeeLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            return lib.SaveEmployeeRoles(employeeID, roleIDs);
        }
    }
}