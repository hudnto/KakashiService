using KakashiService.Core.Services;
using KakashiService.Web.ViewModel;
using System;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using KakashiService.Core.Entities;

namespace KakashiService.Web.Controllers
{
    public class RegistrationController : Controller
    {
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
            var serviceObject = new ServiceObject();
            ConfigurationVM.Convert(serviceObject, config);

            var bytes = Session["stream"] as byte[];
            if (bytes != null)
            {                
                var stream = new MemoryStream(bytes);
                serviceObject.FileStream = stream;
            }
            
            var main = new MainService();
            try
            {                
                main.Execute(serviceObject);
                message += String.Format("\nEndpoint: http://{0}:{1}/{2}.svc?wsdl", Request.UrlReferrer.Host, serviceObject.Port, serviceObject.Name);
                return Json(new { success = true, modal = new { message, title = "Operation Completed!" } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                message = "Error cloning! ("+e.Message+")";
                return Json(new { success = false, modal = new { message, title = "Operation Fail!" } }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportFile()
        {
            var file = HttpContext.Request.Files[0];
            var tempStream = new MemoryStream();
            file.InputStream.CopyTo(tempStream);
            Session["stream"] = tempStream.GetBuffer();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}