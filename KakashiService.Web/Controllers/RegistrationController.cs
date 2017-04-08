using KakashiService.Core.Entities;
using KakashiService.Core.Services;
using System.Web.Mvc;

namespace KakashiService.Web.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var main = new MainService();

            var serviceObject = new ServiceObject();

            const int port = 20205;
            serviceObject.Name = "ComplexService" + port;
            serviceObject.Port = port;
            serviceObject.Path = @"C:\Kakashi";
            serviceObject.Namespace = "Kakashi";

            //serviceObject.Url = "http://localhost:40799/ServicoData.svc?wsdl";
            serviceObject.Url = "http://localhost:58764/Service2.svc?wsdl";
            //serviceObject.Url = "http://www.dneonline.com/calculator.asmx?wsdl";

            main.Execute(serviceObject);

            return View();
        }

        public JsonResult RegisterEndPoint(string endpoint)
        {


            return Json(new object { });
        }
    }
}