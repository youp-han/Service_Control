using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace ScmManagerServiceControl.Core
{
    public class Utility
    {

        //check the status of the service using url
        public bool checkURL(string url)
        {
            //1. check if the site is alive
                //Informational responses(100–199)
                //Successful responses(200–299) OK
                //Redirects(300–399)
                //Client errors(400–499)
                //Server errors(500–599)
                //http://zetcode.com/csharp/httpclient/

                try
                {
                    HttpClient client = new HttpClient();
                    var checkResult = client.GetAsync(url);
                    Console.WriteLine("Status =" + checkResult.Result.StatusCode);

                    if (checkResult.Result.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch 
                {
                    return false;

                }


            return false;
        }

        //check if the process name exists within the task manager list
        public int CheckProcess(string processName)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ToString().Contains(processName))
                {
                    Console.WriteLine("Process Exists :" + processName);
                    return process.Id;
                }

            }
            Console.WriteLine("Process Does Not exists :" + processName);
            return -1;
        }

        //if so, then kill Process
        public void KillProcess(int processID)
        {
            var process = Process.GetProcessById(processID);
            process.Kill();
        }


        //check, until Process is killed completely
        public bool CheckProcossIsAlive(string processName)
        {
            if (CheckProcess(processName) > 0)
            {
                Thread.Sleep(2000);
                Console.WriteLine(processName+ " is Stopping.......................................");

                CheckProcossIsAlive(processName);
                
            }
            else
            {
                Console.WriteLine(processName + " is stopped.......................................");

            }

            return false;
        }

    }


}
