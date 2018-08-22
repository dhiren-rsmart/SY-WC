using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using Newtonsoft.Json;
using smART.Common;
using smART.ViewModel;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Security
{
    public class SmartPrincipal : IPrincipal
    {
        private IIdentity _identity;
        [JsonIgnore]
        public IIdentity Identity
        {
            get { return _identity ?? (_identity = new GenericIdentity(Name)); }
        }

        public string Name { get; set; }
        public int ID { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        //public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<RoleFeature> RoleFeatures { get; set; }

        public bool IsInRole(string role)
        {
            if (RoleFeatures == null)
            {
                EmployeeHelper employeeHelper = new EmployeeHelper();
                RoleFeatures = employeeHelper.GetRoleFeaturesForEmployee(ID);
            }

            if (RoleFeatures == null) return false;

            return (RoleFeatures.SingleOrDefault(o => o.Role.Role_Name.Contains(role)) != null);
        }

        public bool IsInFeature(EnumFeatures feature)
        {
            if (RoleFeatures == null)
            {
                EmployeeHelper employeeHelper = new EmployeeHelper();
                RoleFeatures = employeeHelper.GetRoleFeaturesForEmployee(ID);
            }


            if (RoleFeatures == null) return false;

            return (RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName == feature.ToString()) != null);
        }

        public bool IsInFeatureAction(EnumFeatures feature, EnumActions action)
        {
            if (RoleFeatures == null)
            {
                EmployeeHelper employeeHelper = new EmployeeHelper();
                RoleFeatures = employeeHelper.GetRoleFeaturesForEmployee(ID);
            }

            
            if (RoleFeatures == null) return false;

            RoleFeature rf;
            switch (action)
            {
                case EnumActions.Add:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && o.NewAccessInd == true);
                    return (rf != null);
                case EnumActions.Edit:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && o.EditAccessInd == true);
                    return (rf != null);
                case EnumActions.Delete:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && o.DeleteAccessInd == true);
                    return (rf != null);
                case EnumActions.Save:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && (o.NewAccessInd == true || o.EditAccessInd == true));
                    return (rf != null);
                case EnumActions.Search:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && o.ViewAccessInd == true);
                    return (rf != null);
                case EnumActions.Print:
                    rf = RoleFeatures.FirstOrDefault(o => o.Feature.FeatureName.Contains(feature.ToString()) && o.NewAccessInd == true);
                    return (rf != null);
                default:
                    return IsInFeature(feature);
            }
            
        }

        /// <summary>
        /// Get's a JSON serialized string of a SimplePrincipal object
        /// </summary>
        public static string GetCookieUserData(SmartPrincipal principal)
        {
            return JsonConvert.SerializeObject(principal);
        }

        /// <summary>
        /// Creates a SimplePrincipal object using a JSON string from the asp.net auth cookie
        /// </summary>
        public static SmartPrincipal CreatePrincipalFromCookieData(string userData)
        {
            return JsonConvert.DeserializeObject<SmartPrincipal>(userData);
        }
    }
}
