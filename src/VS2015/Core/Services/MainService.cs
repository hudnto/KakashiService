using KakashiService.Core.Entities;
using System.Configuration;

namespace KakashiService.Core.Services
{
    public class MainService
    {
        public void Execute(ServiceObject serviceObject)
        {
            serviceObject.IISPath = ConfigurationManager.AppSettings["iisPath"];
            serviceObject.MsBuildPath = ConfigurationManager.AppSettings["msbuildPath"];
            serviceObject.SvcUtilPath = ConfigurationManager.AppSettings["svcutilPath"];
            serviceObject.SvcUtilPath = ConfigurationManager.AppSettings["logPath"];

            var readService = new ReadService();
            
            readService.Execute(serviceObject);

            var createService = new CreateService();
            createService.Execute(serviceObject);

            var buildService = new BuildService();
            buildService.Execute(serviceObject);
        }
    }
}
