using KakashiService.Core.Entities;
using System.Configuration;

namespace KakashiService.Core.Services
{
    public class MainService
    {
        public void Execute()
        {
            var serviceObject = new ServiceObject();

            // TODO find a better way to place this
            serviceObject.Name = "DividirParaConquistar6";
            serviceObject.Port = 20018;
            serviceObject.Path = @"C:\Users\lcramos1\Desktop\Kakashi";
            serviceObject.Namespace = "Kakashi";

            serviceObject.Url = "http://localhost:40799/ServicoData.svc?wsdl";
            //serviceObject.Url = "http://localhost:58764/Service2.svc?wsdl";
            //serviceObject.Url = "http://www.dneonline.com/calculator.asmx?wsdl";

            serviceObject.IISPath = ConfigurationManager.AppSettings["iisPath"];
            serviceObject.MsBuildPath = ConfigurationManager.AppSettings["msbuildPath"];
            serviceObject.SvcUtilPath = ConfigurationManager.AppSettings["svcutilPath"];

            var readService = new ReadService();
            readService.Execute(serviceObject);

            var createService = new CreateService();
            createService.Execute(serviceObject);

            var buildService = new BuildService();
            buildService.Execute(serviceObject);
        }
    }
}
