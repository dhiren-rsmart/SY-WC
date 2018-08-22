using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel
{
    public abstract class ExpenseEntity<TEntity> : ExpensesRequest 
        where TEntity: BaseEntity
    {
          [HiddenInput(DisplayValue = false)]
        public TEntity Parent { get; set; }
                
     }
}
