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
    public class DispatcherRequestExpenseLibrary : ExpenseLibrary<VModel.ExpensesRequest, Model.ExpensesRequest, VModel.DispatcherRequest, Model.DispatcherRequest>
    {
        public DispatcherRequestExpenseLibrary() : base() { }
        public DispatcherRequestExpenseLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);

            Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
            Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

            Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
            Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();
        }

        public override string GetRefrenceTable() { return new Model.DispatcherRequest().GetType().Name; }

    }
}
