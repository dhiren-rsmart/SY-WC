using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace smART.MVC.Library.BusinessRules
{
    public class BusinessRuleEngine
    {
        public static  bool InvokeMethod(string classType, string methodName, object businessEntity, object modelEntity, object dbContext, bool cancel)
        {
            // Using Reflection to dynamicly create an instance of the object
            Type theType = Type.GetType(classType);
            ConstructorInfo objConstructor = theType.GetConstructor(new Type[] { });
            object theObject = objConstructor.Invoke(new object[] { });

            MethodInfo ProcCallInfo = theType.GetMethod(methodName);
            object[] args = new object[] { businessEntity, modelEntity, dbContext, cancel };
            object retVal = ProcCallInfo.Invoke(theObject, args);

            return (bool)args[3];
        }

        public static void InvokeMethod(string classType, string methodName, object businessEntity, object modelEntity, object dbContext)
        {
            // Using Reflection to dynamicly create an instance of the object
            Type theType = Type.GetType(classType);
            ConstructorInfo objConstructor = theType.GetConstructor(new Type[] { });
            object theObject = objConstructor.Invoke(new object[] { });

            MethodInfo ProcCallInfo = theType.GetMethod(methodName);
            object[] args = new object[] { businessEntity, modelEntity, dbContext };
            object retVal = ProcCallInfo.Invoke(theObject, args);
        }
    }
}
