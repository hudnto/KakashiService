using KakashiService.Core.Services;
using KakashiService.Web.ViewModel;
using System;
using System.Threading;
using System.Web.Mvc;
using KakashiService.Core.Entities;

namespace KakashiService.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private ServiceObject _serviceObject;

        public RegistrationController()
        {
            _serviceObject = new ServiceObject();
        }
        public ActionResult Index()
        {
            return View(new ConfigurationVM(true));
        }

        [HttpPost]
        public JsonResult Register(ConfigurationVM config)
        {
            var message = "Service " + config.ServiceName + " Cloned!";

            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }
            _serviceObject = ConfigurationVM.Convert(config);
            var main = new MainService();
            try
            {                
                main.Execute(_serviceObject);
                message += String.Format("\nEndpoint: http://{0}:{1}/{2}.svc?wsdl", Request.UrlReferrer.Host, _serviceObject.Port, _serviceObject.Name);
                return Json(new { success = true, modal = new { message, title = "Operation Completed!" } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                message = "Error cloning! Exception message: "+e.Message;
                return Json(new { success = false, modal = new { message, title = "Operation Fail!" } }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportFile()
        {
            var file = HttpContext.Request.Files[0];
            var stream = file.InputStream;
            _serviceObject.FileStream = stream;
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}