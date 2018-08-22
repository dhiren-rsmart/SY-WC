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
    public class ScaleExpenseLibrary : ExpenseLibrary<VModel.ExpensesRequest, Model.ExpensesRequest, VModel.Scale, Model.Scale>
    {
        public ScaleExpenseLibrary() : base() { }
        public ScaleExpenseLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);

            Mapper.CreateMap<VModel.Scale, Model.Scale>();
            Mapper.CreateMap<Model.Scale, VModel.Scale>();

            Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
            Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();
        }

        public override string GetRefrenceTable() { return new Model.Scale().GetType().Name; }

    }
}
