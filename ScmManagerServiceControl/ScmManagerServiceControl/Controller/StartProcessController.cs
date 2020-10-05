using System;
using System.IO;
using Newtonsoft.Json.Linq;
using ScmManagerServiceControl.Core;
using ScmManagerServiceControl.Model;

namespace ScmManagerServiceControl.Controller
{
    public class StartProcessController
    {
        private static ServiceData sd;

        public void StartProcess()
        {
            Start();
        }

        //start
        void Start()
        {
            //reading info from the json file
            string myFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "serviceData.json");
            JObject data = JObject.Parse(File.ReadAllText(myFilePath));

            sd = new ServiceData
            {
                progressName = data["progressName"].ToString(),
                serviceName = data["serviceName"].ToString()

            };

            Utility util = new Utility();

            //1. kill process
            int processCheckResult = util.CheckProcess(sd.progressName);
            bool killProcessResult = true;

            if (processCheckResult > 0)
            {
                util.KillProcess(processCheckResult);
                killProcessResult = util.CheckProcossIsAlive(sd.progressName);
            }


            //2. Start service in Services
            // check if the services status is running if not run Service
            if (!killProcessResult)
            {
                ServiceControlCore scc = new ServiceControlCore();
                if (!scc.GetServices(sd.serviceName))
                {
                    scc.StartService(sd.serviceName, 2000);
                }
            }
        }
    }
}
