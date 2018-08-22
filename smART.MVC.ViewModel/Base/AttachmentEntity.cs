// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace smART.ViewModel
{
    public abstract class AttachmentEntity<TEntity> : BaseAttachment, IListType
        where TEntity : BaseEntity
    {
        [HiddenInput(DisplayValue = false)]
        public TEntity Parent { get; set; }

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
