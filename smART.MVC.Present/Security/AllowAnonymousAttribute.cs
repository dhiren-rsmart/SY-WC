using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smART.MVC.Present.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }
}