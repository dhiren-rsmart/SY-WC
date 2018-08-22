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
    public class PurchaseOrderNotesLibrary: NotesLibrary<VModel.PurchaseOrderNotes, Model.PurchaseOrderNotes, VModel.PurchaseOrder, Model.PurchaseOrder>
    {
        public PurchaseOrderNotesLibrary() : base() { }
        public PurchaseOrderNotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
            Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();
            Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
            Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
        }

    }
}
