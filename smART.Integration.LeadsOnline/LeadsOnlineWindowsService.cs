using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel.Web;

namespace smART.Integration.LeadsOnline
{
    public partial class LeadsOnlineWindowsService : ServiceBase
    {
        SmARTLeadsScheduler _leadsScheduler;
        public LeadsOnlineWindowsService()
        {
            InitializeComponent();
            ServiceName = "LeadsOnlineWindowsService";
            // Set window service properties.
            this.CanShutdown = true;
            this.CanStop = true;
            _leadsScheduler = new SmARTLeadsScheduler();
        }

        protected override void OnStart(string[] args)
        {
            _leadsScheduler.Start();
        }

        protected override void OnStop()
        {
            _leadsScheduler.Stop();
        }

        protected override void OnShutdown()
        {
            _leadsScheduler.Stop();
            this.Stop();
            base.OnShutdown();
        }
    }
}
