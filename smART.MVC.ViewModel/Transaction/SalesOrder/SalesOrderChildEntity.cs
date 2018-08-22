using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace smART.ViewModel
{
    public abstract class SalesOrderChildEntity : BaseEntity, IListType
    {
        [HiddenInput(DisplayValue = false)]
        public SalesOrder SalesOrder { get; set; }


        #region IListType Members

        [HiddenInput(DisplayValue = false)]
        public virtual string ListText
        {
            get { return ID.ToString(); }
        }

        [HiddenInput(DisplayValue = false)]
        public virtual string ListValue
        {
            get { return ID.ToString(); }
        }

        #endregion
    }
}
