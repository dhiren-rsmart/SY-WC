using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common {

  [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class UniqueAttribute : System.Attribute {
    private string[] _atts;
    public string[] KeyFields {
      get {
        return _atts;
      }
    }
    public UniqueAttribute(string keyFields) {
      this._atts = keyFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }
  } 

}
