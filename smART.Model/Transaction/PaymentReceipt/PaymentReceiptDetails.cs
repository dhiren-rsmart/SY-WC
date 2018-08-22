// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Payment_Receipt_Details")] 
    public class PaymentReceiptDetails : BaseEntity
    {
        public Settlement Settlement { get; set; }
        public Invoice Invoice { get; set; }
        public PaymentReceipt PaymentReceipt { get; set; }
        public decimal Balance_Amount { get; set; }
        public bool Paid_In_Full { get; set; }
        public decimal Apply_Amount { get; set; }
        public ExpensesRequest ExpenseRequest { get; set; }

    }
}
