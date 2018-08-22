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
  public class InvoiceExpenseLibrary : ExpenseLibrary<VModel.ExpensesRequest, Model.ExpensesRequest, VModel.Invoice, Model.Invoice>
    {
        public InvoiceExpenseLibrary() : base() { }
        public InvoiceExpenseLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);

            Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
            Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();

            Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
            Mapper.CreateMap<Model.Invoice, VModel.Invoice>();
        }

        public override string GetRefrenceTable() { return new Model.Invoice().GetType().Name; }
    }
}
