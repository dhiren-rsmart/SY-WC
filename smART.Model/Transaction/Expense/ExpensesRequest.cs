using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {
  [Table("T_Expenses")]
  public class ExpensesRequest : BaseEntity {

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string EXPENSE_TYPE { get; set; }

    [StringLength(50, ErrorMessage = "Maximum length is 45")]
    public string EXPENSE_Sub_TYPE { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Paid_By { get; set; }

    public Double Amount_Paid { get; set; }

    public Party Paid_Party_To { get; set; }

    public Double Amount_Paid_Till_Date { get; set; }

    public Boolean Paid_YN { get; set; }

    //public bool Approved { get; set; }

    [StringLength(100, ErrorMessage = "Maximum length is 100")]
    public string Comments { get; set; }


    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    public string Reference_Table { get; set; } //? to ask

    public int Reference_ID { get; set; } // ? to ask 

    [StringLength(50, ErrorMessage = "Maximum length is 50")]
    public string Expense_Status { get; set; }

    public PaymentReceipt Payment { get; set; }

    public Invoice Invoice { get; set; }
   
    public DispatcherRequest Dispatcher_Request_Ref { get; set; }

    public Scale Scale_Ref { get; set; }
  }
}
