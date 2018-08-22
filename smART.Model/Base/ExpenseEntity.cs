using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    
    public abstract class ExpenseEntity<TEntity>:ExpensesRequest 
        where TEntity: BaseEntity
    {
        public TEntity Parent { get; set; }
    }
}
