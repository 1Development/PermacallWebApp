using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PermacallTeamspeakLogger
{
    public partial class LoggerService : ServiceBase
    {
        public LoggerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            logTimer.Enabled = false;
        }

        private void logTimer_Tick(object sender, EventArgs e)
        {
            LogRepo.Log();
        }
    }
}
