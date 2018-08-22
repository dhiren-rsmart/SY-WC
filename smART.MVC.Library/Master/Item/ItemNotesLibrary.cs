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
    public class ItemNotesLibrary: NotesLibrary<VModel.ItemNotes, Model.ItemNotes, VModel.Item, Model.Item>
    {
        public ItemNotesLibrary() : base() { }
        public ItemNotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Item, Model.Item>();
            Mapper.CreateMap<Model.Item, VModel.Item>();
            Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
            Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
        }

    }
    
}
