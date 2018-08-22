using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;


namespace smART.MVC.Present.ModelBinder
{
    public class BaseEntityModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            PropertyInfo pInfo = bindingContext.Model.GetType().GetProperty("ID");
            if (pInfo != null)
            {
                int id = (int)pInfo.GetValue(bindingContext.Model, null);

                switch (propertyDescriptor.Name)
                {
                    case "Created_By":
                        if (id == 0) propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.User.Identity.Name);
                        break;
                    case "Created_Date":
                        if (id == 0) propertyDescriptor.SetValue(bindingContext.Model, DateTime.Now);
                        break;
                    case "Updated_By":
                        propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.User.Identity.Name);
                        break;
                    case "Last_Updated_Date":
                        propertyDescriptor.SetValue(bindingContext.Model, DateTime.Now);
                        break;
                    case "Active_Ind":
                        if (id == 0) propertyDescriptor.SetValue(bindingContext.Model, true);
                        break;
                    case "Site_Org_ID":
                         propertyDescriptor.SetValue(bindingContext.Model,  Convert.ToInt32(controllerContext.HttpContext.Session["Site_Org_ID"] ));
                        break;
                    case "Unique_ID":
                        if (id == 0) propertyDescriptor.SetValue(bindingContext.Model, Convert.ToInt32(controllerContext.HttpContext.Session["Unique_ID"]));
                        break;
                }
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        //#region IModelBinder Members
        //public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    if (controllerContext == null)
        //        throw new ArgumentNullException("controllerContext");
        //    if (bindingContext == null)
        //        throw new ArgumentNullException("bindingContext");

        //    var serializer = new DataContractJsonSerializer(bindingContext.ModelType);
        //    return serializer.ReadObject(controllerContext.HttpContext.Request.InputStream);
        //}
        //#endregion 

    }
}