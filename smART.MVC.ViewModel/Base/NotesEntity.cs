using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;


namespace smART.ViewModel
{
    public abstract class NotesEntity<TEntity> : BaseNotes, IListType
        where TEntity: BaseEntity
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
