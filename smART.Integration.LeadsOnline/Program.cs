using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;

namespace smART.Integration.LeadsOnline
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			{ 
				new LeadsOnlineWindowsService()
			};
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
#if (DEBUG)
                Debugger.Launch(); //<-- Simple form to debug a web services 
                SmARTLeadsScheduler sch = new SmARTLeadsScheduler();
                sch.Start();
                //=========== Testing Code========================                
                LeadsOnlineServiceManger leadsServiceManger = new LeadsOnlineServiceManger();
                string msg;
                leadsServiceManger.CheckLogin(out msg);
                //leadsServiceManger.PostTickets();
                Console.ReadLine();
                //=========== Testing Code========================
#endif


            }
        }
    }
}
