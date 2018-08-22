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
    public class SalesOrderNotesLibrary: NotesLibrary<VModel.SalesOrderNotes, Model.SalesOrderNotes, VModel.SalesOrder, Model.SalesOrder>
    {
        public SalesOrderNotesLibrary() : base() { }
        public SalesOrderNotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
            Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();
            Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
            Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
        }

    }
}
