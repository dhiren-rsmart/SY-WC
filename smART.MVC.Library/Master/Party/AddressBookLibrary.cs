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
    public class AddressBookLibrary : PartyChildLibrary<VModel.AddressBook, Model.AddressBook>
    {
        public AddressBookLibrary() : base() { }
        public AddressBookLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public IEnumerable<VModel.AddressBook> GetAddressesByPartyIdWithPaging(int partyId, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.AddressBook> modEnumeration = _repository.FindByPaging<Model.AddressBook>(out totalRows, o => o.Party.ID == partyId,
                                                                                         page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.AddressBook> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public VModel.AddressBook GetPrimaryAddressesByPartyId(int partyId)
        {
            return GetSingleByCriteria(p => p.Party.ID == partyId && p.Primary_Flag == true && p.Active_Ind == true);
        }

    }
}
