using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Diagnostics;

namespace smART.Integration.LeadsOnline
{

    public class PostTicketOnLeadServerJob : IJob
    {

        private static bool runningJob;

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (!runningJob)
                {
                    runningJob = true;
                    LeadsOnlineServiceManger leadsServiceManger = new LeadsOnlineServiceManger();
                    leadsServiceManger.PostTickets();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            finally
            {
                runningJob = false;
            }
        }

        #endregion
    }
}
