﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace smART.ViewModel
{
    public abstract class InvoiceChildEntity : BaseEntity, IListType
    {
        [HiddenInput(DisplayValue = false)]
        public Invoice Invoice { get; set; }


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
