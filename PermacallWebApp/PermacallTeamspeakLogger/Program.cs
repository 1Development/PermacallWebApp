using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PermacallTeamspeakLogger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {


                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new LoggerService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception e)
            {
                LogRepo.WriteToFile(e.Message);
                throw;
            }
        }
    }
}
