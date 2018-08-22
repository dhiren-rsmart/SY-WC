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
    public class InvoiceNotesLibrary: NotesLibrary<VModel.InvoiceNotes, Model.InvoiceNotes, VModel.Invoice, Model.Invoice>
    {
        public InvoiceNotesLibrary() : base() { }
        public InvoiceNotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
            Mapper.CreateMap<Model.Invoice, VModel.Invoice>();
            Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
            Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
        }

    }
}
