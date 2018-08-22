using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.Reflection;

namespace smART.ViewModel
{
    public abstract class FormatedBaseEntity: BaseEntity
    {
         [DisplayName("ID")]
        public string FormatedID
        {
            get { return (Created_Date.HasValue) ? "GSM/" + Convert.ToDateTime(Created_Date).Month + "/" + Convert.ToDateTime(Created_Date).Year + "/" + ID : ID.ToString(); }
        }

        [DisplayName("Created Date")]
        public override DateTime? Created_Date { get; set; }

    }
}
