using KakashiService.Core.Entities;
using KakashiService.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KakashiService.UnitTest.Services
{
    [TestClass]
    public class MainServiceTest
    {
        [TestMethod]
        public void SimpleRun()
        {
            var main = new MainService();

            var serviceObject = new ServiceObject();

            serviceObject.Name = "DividirParaConquistar6";
            serviceObject.Port = 20018;
            serviceObject.Path = @"C:\Users\lcramos1\Desktop\Kakashi";
            serviceObject.Namespace = "Kakashi";

            serviceObject.Url = "http://localhost:40799/ServicoData.svc?wsdl";
            //serviceObject.Url = "http://localhost:58764/Service2.svc?wsdl";
            //serviceObject.Url = "http://www.dneonline.com/calculator.asmx?wsdl";

            main.Execute(serviceObject);
        }
    }
}
