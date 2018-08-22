using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;


namespace smART.Library
{
    public class NoteLibrary : PartyChildLibrary<VModel.Note, Model.Note>
    {
        public NoteLibrary() : base() { }
        public NoteLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }
    }
}
