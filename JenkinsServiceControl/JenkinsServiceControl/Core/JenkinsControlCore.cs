using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using NLog;

namespace JenkinsServiceControl.Core
{
    public class JenkinsControlCore
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        //Services 에서 돌고 있는 리스트 출력
        //testCode
        public bool GetServices(string serviceName)
        {
            ServiceController[] services =  ServiceController.GetServices();
            var checkServiceName = services.FirstOrDefault(s => s.ServiceName == serviceName);

            if (checkServiceName != null && checkServiceName.Status.Equals(ServiceControllerStatus.Running))
            {
                return true;
            }

            return false;
        }


        //Services 내 이름으로 검색하여 서비스 시작
        public void StartService(string serviceName, int timeoutMillsec)
        {
            ServiceController service = new ServiceController(serviceName);
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMillsec);

            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);

                if (CheckServiceStarted(serviceName))
                {
                    logger.Info(" Service Started Successfully.");
                }
            }
            catch (Exception e)
            {
                logger.Error(" StartService Error: " + e);
                throw;
            }
            finally
            {
                service.Dispose();
            }

        }

        //Services 내 이름으로 검색하여 서비스 멈춤
        public void StopService(string serviceName, int timeoutMillsec)
        {
            ServiceController service = new ServiceController(serviceName);
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMillsec);

            try
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                if (!CheckServiceStopped(serviceName))
                {
                    logger.Info(" Service Stopped Successfully.");
                }
            }
            catch (Exception e)
            {
                logger.Error(" StopService Error: " + e);
                throw;
            }
            finally
            {
                service.Dispose();
            }

        }

        public void SafeStopService(string serviceName, string serviceURL, string jenkinsUser, string jenkinsPwd)
        {
            ExecuteCommandInCMD ecc = new ExecuteCommandInCMD();

            try
            {
                ecc.runCommand(serviceURL,  jenkinsUser,  jenkinsPwd);

                if (!CheckServiceStopped(serviceName))
                {
                    Console.WriteLine(" Service Stopped Successfully.");
                    logger.Info(" Service Stopped Successfully.");
                }
            }
            catch(Exception e)
            {
                logger.Error(" StopService Error: " + e);
                throw;
            }

        }

        //Safe Stop 은 돌고 있는 job 이 끝나면 서비스가 내려가기 때문에
        //지속적으로 멈추는 시점을 확인 해야 한다.
        bool CheckServiceStopped(string serviceName)
        {
            if (GetServices(serviceName))
            {
                logger.Info(" Service is Stopping.......................................");
                Console.WriteLine(" Service is Stopping.......................................");

                Thread.Sleep(2000);
                CheckServiceStopped(serviceName);
            }

            return false;
        }

        bool CheckServiceStarted(string serviceName)
        {
            if (!GetServices(serviceName))
            {
                logger.Info(" Service is Starting.......................................");
                Console.WriteLine(" Service  is Starting.......................................");
                Thread.Sleep(2000);
                CheckServiceStarted(serviceName);
            }

            return true;
        }
    }
}
