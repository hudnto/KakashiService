using KakashiService.Core.Entities;
using System.Configuration;
using System.Web.Mvc;

namespace KakashiService.Web.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var serviceObject = new ServiceObject();
            serviceObject.IISPath = ConfigurationManager.AppSettings["iisPath"];
            serviceObject.MsBuildPath = ConfigurationManager.AppSettings["msbuildPath"];
            serviceObject.SvcUtilPath = ConfigurationManager.AppSettings["svcutilPath"];
            return View();
        }

        public JsonResult RegisterEndPoint(string endpoint)
        {


            return Json(new object { });
        }
    }
}