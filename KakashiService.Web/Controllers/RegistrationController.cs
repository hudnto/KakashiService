using KakashiService.Core.Services;
using KakashiService.Web.ViewModel;
using System;
using System.Web.Mvc;

namespace KakashiService.Web.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new ConfigurationVM());
        }

        [HttpPost]
        public JsonResult Register(ConfigurationVM config)
        {
            var message = "Service " + config.ServiceName + " Cloned!";

            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            var serviceObject = ConfigurationVM.Convert(config);
            var main = new MainService();
            //main.Execute(serviceObject);

            message += String.Format("\nEndpoint: http://{0}:{1}/{2}.svc?wsdl", Request.UrlReferrer.Host, serviceObject.Port, serviceObject.Name);

            return Json(new { success = true, modal = new { message, title = "Operation Completed!" } }, JsonRequestBehavior.AllowGet);
        }
    }
}