using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
    public enum EnumTransactionType
    {
        Payment,
        Receipt,
        Scale,
        Booking
    }

   public enum EnumPaymentType {
     Tickets,
     Expenses
   }

   public enum EnumExpenseStatus {
     Approved,
     Reject
   }
   
}
