using System;
using System.Configuration;
using JenkinsServiceControl.Core;
using NLog;

namespace JenkinsServiceControl
{
    class Program
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            string upperDirectoryName = ConfigurationSettings.AppSettings["directory_RootName"];
            string serviceName = ConfigurationSettings.AppSettings["service_Name"];
            string serviceURL = ConfigurationSettings.AppSettings["service_URL"];
            string jenkinsUser = ConfigurationSettings.AppSettings["user_Name"];
            string jenkinsPwd = ConfigurationSettings.AppSettings["password"];


            string[] deleteFolderName = { "lastStable", "lastStableBuild", "lastSuccessful", "lastSuccessfulBuild" };

            try
            {
                JenkinsControlCore jsc = new JenkinsControlCore();

                if (jsc.GetServices(serviceName))
                {
                    logger.Info(" < " + serviceName + " > "+ " Service Found, ReStart Sequence Started");
                    Console.WriteLine(" < " + serviceName + " > " + " Service Found, ReStart Sequence Started");

                    //1. Java CLI Safe-Shutdown
                    jsc.SafeStopService(serviceName,  serviceURL,  jenkinsUser,  jenkinsPwd);
                    //1. Services Stop
                    //jsc.StopService(serviceName, 20000);

                    //2. Folder search & delete
                    //폴더명 search:  lastStable, lastStableBuild, lastSuccessful, lastSuccessfulBuild
                    FolderDelete folderDelete = new FolderDelete();

                    foreach (var folderName in deleteFolderName)
                    {
                        folderDelete.deleteFolders(upperDirectoryName, folderName);
                    }


                    //4. Jenkins Service Start
                    jsc.StartService(serviceName, 20000);
                    Console.WriteLine(" < " + serviceName + " > " + " Service Restarted.... Restart Sequence Ended");
                    //Console.ReadLine();

                }
                else
                {
                    logger.Info(" < " + serviceName + " > " + " Either Service NOT Found, or Not Running");

                }



            }
            catch (Exception e)
            {
                logger.Error(" Error: " + e);
            }

            

        }
    }
}
