using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library
{
    public class LeadLogLibrary : GenericLibrary<VModel.LeadLog, Model.LeadLog>
    {
        public LeadLogLibrary() : base() { }
        public LeadLogLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }
               
        public IEnumerable<VModel.LeadLog> GetLeadLogs()
        {
            return base.GetByExpression(i => i.Active_Ind == true);
        }
    }
}
