using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;

using smART.Common;
namespace smART.MVC.Present.Security
{
    public static class AccessControl
    {
        public static bool IsControllerActionAccessible(
            IPrincipal user,
            ControllerBase controllerBase,
            ActionDescriptor actionDescriptor
            )
        {
            // Check controller first
            bool controllerAccess = IsControllerAccessile(user, controllerBase);
            if (!controllerAccess)
                return false;

            return true;

        }

        public static bool IsControllerAccessile(
            IPrincipal user,
            ControllerBase controller)
        {
            bool isAccessible = false;
            FeatureAttribute[] attribs = (FeatureAttribute[])controller.GetType().GetCustomAttributes(typeof(FeatureAttribute), true);
            if (attribs != null && attribs.Length > 0)
            {
                foreach (EnumFeatures enumFeature in attribs[0].Features)
                {
                    if (enumFeature == EnumFeatures.Exempt)
                    {
                        isAccessible = true;
                        break;
                    }
                    isAccessible |= ((SmartPrincipal)user).IsInFeature(enumFeature);
                }
            }
            return isAccessible;

            //return true;
        }

        public static bool IsActionAccessible(
            IPrincipal user,
            ControllerBase controller,
            ActionDescriptor action)
        {
            return true;
        }

        public static bool IsActionAccessible(
        IPrincipal user,
        EnumFeatures feature,
        EnumActions action)
        {
            //return true;
            return ((SmartPrincipal)user).IsInFeatureAction(feature, action);
        }
    }
}