using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace smART.Integration.LeadsOnline
{

    public class SmARTLeadsScheduler : ISmARTLeadsScheduler
    {

        private IScheduler _scheduler;

        #region IScheduler Members

        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public void Start()
        {
            try
            {
                ISchedulerFactory scf = new StdSchedulerFactory();
                _scheduler = scf.GetScheduler();
                if (!_scheduler.IsStarted)
                    TextFileLogger.Log(string.Format("{0}Service Started at {1}.", System .Environment.NewLine , DateTime.Now.ToString()));
                _scheduler.Start();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public void Stop()
        {
            try
            {
                TextFileLogger.Log(string.Format("{0}Service Started at {1}.", System.Environment.NewLine, DateTime.Now.ToString()));
                _scheduler.Shutdown();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion IScheduler Members
    }
}
