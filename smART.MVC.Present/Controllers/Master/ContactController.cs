using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using smART.ViewModel;
using Telerik.Web.Mvc;
using smART.Common;
using smART.MVC.Present.Helpers;

namespace smART.MVC.Present.Controllers
{
    [Feature(EnumFeatures.Master_Contact)]
    public class ContactController : PartyChildGridController<ContactLibrary, Contact>
    {
        public ContactController() : base("Contact", new string[] { "Party" }) { }

        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult _ContactByPartyType(GridCommand command, string partytype, bool isNew = false)
        //{
        //    int totalRows = 0;

        //    ILibrary<Contact> lib = new ContactLibrary(Configuration.GetsmARTDBContextConnectionString());

        //    FilterDescriptor filterDesc = new FilterDescriptor("Party_Type", FilterOperator.IsEqualTo, partytype);
        //    command.FilterDescriptors.Add(filterDesc);

        //    IEnumerable<Contact> resultList = lib.GetAllByPaging(out totalRows, command.Page, (command.PageSize == 0 ? 20 : command.PageSize), "", "Asc", null, (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));

        //    return View(new GridModel { Data = resultList, Total = totalRows });
        //}
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _ContactByPartyType(GridCommand command, string partytype, bool isNew = false)
        {
            int totalRows = 0;

            ContactLibrary lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Contact> resultList =lib.GetByPartyTypeWithPaging(partytype,
                                                                          out totalRows, command.Page,
                                                                          (command.PageSize == 0 ? 20 : command.PageSize),
                                                                           "", "Asc", null,
                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                          );      

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _DriverRoleContactByPartyId(GridCommand command, int id)
        {
            int totalRows = 0;

            ContactLibrary lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Contact> resultList = lib.GetDriverRoleContactByPartyIdWithPaging(id,
                                                                          out totalRows, command.Page,
                                                                          (command.PageSize == 0 ? 20 : command.PageSize),
                                                                           "", "Asc", null,
                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                          );

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _DriverRoleContactByPartyType(GridCommand command, bool isNew = false)
        {
            int totalRows = 0;

            ContactLibrary lib = new ContactLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Contact> resultList = lib.GetDriversWithPaging(out totalRows, command.Page,
                                                                          (command.PageSize == 0 ? 20 : command.PageSize),
                                                                           "", "Asc", null,
                                                                          (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                          );

            return View(new GridModel { Data = resultList, Total = totalRows });
        }
    }
}
