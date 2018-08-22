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
 
    public class BookingNotesLibrary: NotesLibrary<VModel.BookingNotes, Model.BookingNotes, VModel.Booking, Model.Booking>
    {
        public BookingNotesLibrary() : base() { }
        public BookingNotesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Booking, Model.Booking>();
            Mapper.CreateMap<Model.Booking, VModel.Booking>();
            Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
            Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
        }

    }
    
}
