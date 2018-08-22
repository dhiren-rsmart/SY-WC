using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    public abstract class NotesEntity<TEntity>:BaseNotes 
        where TEntity: BaseEntity
    {
        public TEntity Parent { get; set; }

    }
}
