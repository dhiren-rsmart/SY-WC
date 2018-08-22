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
    public class ContactLibrary : PartyChildLibrary<VModel.Contact, Model.Contact>
    {
        public ContactLibrary() : base() { }
        public ContactLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Party, Model.Party>();
            Mapper.CreateMap<Model.Party, VModel.Party>();
            Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
            Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();
        }

        public IEnumerable<VModel.Contact> GetByPartyType(string partyType)
        {
            IEnumerable<Model.Contact> modParties = from parties in _repository.GetQuery<Model.Contact>()
                                                    where parties.Party.Party_Type.ToLower().Equals(partyType.ToLower())
                                                    select parties;
            IEnumerable<VModel.Contact> busParties = Map(modParties);
            return busParties;
        }

        public IEnumerable<VModel.Contact> GetByPartyTypeWithPaging(string partyType, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.Contact> modEnumeration = _repository.FindByPaging<Model.Contact>(out totalRows, o => o.Party.Party_Type.Equals(partyType, StringComparison.OrdinalIgnoreCase),
                                                                                             page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Contact> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Contact> GetDriverRoleContactByPartyIdWithPaging(int partyId, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.Contact> modEnumeration = _repository.FindByPaging<Model.Contact>(out totalRows, o => o.Party.ID==partyId && o.Role.Equals("Driver",StringComparison.OrdinalIgnoreCase),
                                                                                             page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Contact> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Contact> GetDriversWithPaging( out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.Contact> modEnumeration = _repository.FindByPaging<Model.Contact>(out totalRows, o => (o.Party.Party_Type.Equals("Trucking Company", StringComparison.OrdinalIgnoreCase) || o.Party.Party_Type.Equals("Organization", StringComparison.OrdinalIgnoreCase))
                                                                                                                    && o.Role.Equals("Driver", StringComparison.OrdinalIgnoreCase),
                                                                                             page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Contact> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Contact> GetEmailContactsByPartyId(int partyId) {
          IEnumerable<Model.Contact> modContacts = from contacts in _repository.GetQuery<Model.Contact>()
                                                  where contacts.Party.ID == partyId && contacts.Receive_Emails == true 
                                                  select contacts;
          IEnumerable<VModel.Contact> busContacts = Map(modContacts);
          return busContacts;
        }

        public override System.Linq.Expressions.Expression<Func<Model.Contact, bool>> UniqueEntityExp(Model.Contact modelEntity, VModel.Contact businessEntity) {
          return m => m.First_Name.Equals(modelEntity.First_Name, StringComparison.InvariantCultureIgnoreCase)
                      && m.Last_Name.Equals(modelEntity.Last_Name, StringComparison.InvariantCultureIgnoreCase)
                      && m.Party .ID == modelEntity.Party .ID 
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }
    }
}
