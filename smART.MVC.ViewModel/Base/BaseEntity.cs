using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.Reflection;

namespace smART.ViewModel {

  public abstract class BaseEntity : ViewModelBase {
    [Key]
    public int ID { get; set; }

    [HiddenInput(DisplayValue = false)]
    public int? Unique_ID { get; set; }

    [DefaultValue(true)]
    [HiddenInput(DisplayValue = false)]
    public bool Active_Ind { get; set; }

    [StringLength(45, ErrorMessage = "Maximum Length is 45")]
    [HiddenInput(DisplayValue = false)]
    public string Created_By { get; set; }

    [StringLength(45, ErrorMessage = "Maximum Length is 45")]
    [HiddenInput(DisplayValue = false)]
    public virtual string Updated_By { get; set; }

    [HiddenInput(DisplayValue = false)]
    [DisplayName("Created Date")]
    public virtual DateTime? Created_Date { get; set; }

    [HiddenInput(DisplayValue = false)]
    public virtual DateTime? Last_Updated_Date { get; set; }

    [HiddenInput(DisplayValue = false)]
    public int? Site_Org_ID { get; set; }

    [HiddenInput(DisplayValue = false)]
    public bool IsReadOnly { get; set; }

    //[RefreshProperties(RefreshProperties.All)]
    //public static void AllowRequiredFields(Type type, bool value) {
    //  PropertyInfo[] propInfos = type.GetProperties();

    //  foreach (PropertyInfo property in propInfos) {
    //    //MappedAttribute mappedAttribute = Attribute.GetCustomAttribute(property, typeof(MappedAttribute)) as MappedAttribute;

    //    //PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[property.Name];
    //    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(type)[property.Name];

    //    RequiredAttribute attrib = descriptor.Attributes[(typeof(RequiredAttribute))] as RequiredAttribute;

    //    if (attrib != null && value == false) {
    //      FieldInfo isRequired = attrib.GetType().GetField("<AllowEmptyStrings>k__BackingField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);

    //      isRequired.SetValue(attrib, value);

    //    }
    //  }
    //}
  }
}
