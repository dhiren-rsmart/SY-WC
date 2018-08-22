using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.ViewModel
{
    public class MenuSelect
    {
        public bool Main_MasterSelected { get; set; }
        public bool Main_TransactionSelected { get; set; }
        public bool Main_AdministrationSelected { get; set; }

        public bool Master_PartySelected { get; set; }
        public bool Master_EmployeeSelected { get; set; }
        public bool Master_ItemSelected { get; set; }
        public bool Master_PriceListSelected { get; set; }

        public bool Transaction_DispatcherSelected { get; set; }
        public bool Transaction_WeighingSelected { get; set; }
        public bool Transaction_ExpensesSelected { get; set; }
        public bool Transaction_SalesOrderSelected { get; set; }

        public bool Administration_LOVTypeSelected { get; set; }
    }
}
