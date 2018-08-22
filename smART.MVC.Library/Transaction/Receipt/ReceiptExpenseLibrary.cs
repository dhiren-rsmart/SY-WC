using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;

namespace smART.Library
{
    public class ReceiptExpenseLibrary : ExpenseLibrary<VModel.ExpensesRequest, Model.ExpensesRequest, VModel.PaymentReceipt, Model.PaymentReceipt>
    {
        public ReceiptExpenseLibrary() : base() { }
        public ReceiptExpenseLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);

            Mapper.CreateMap<VModel.PaymentReceipt, Model.PaymentReceipt>();
            Mapper.CreateMap<Model.PaymentReceipt, VModel.PaymentReceipt>();

            Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
            Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();
        }

        public override string  GetRefrenceTable(){return smART.Common.EnumTransactionType.Receipt.ToString();}

    }
}
