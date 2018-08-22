using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present.Controllers.Master
{
    [Feature(EnumFeatures.Master_NotesAppointment)]
    public class NoteController: PartyChildGridController<NoteLibrary, Note>
    {
        public NoteController() : base("Note", new string[] { "Party"}) { }

    }
}
