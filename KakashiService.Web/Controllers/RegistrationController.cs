using KakashiService.Core.Services;
using KakashiService.Web.ViewModel;
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

        public JsonResult Register(ConfigurationVM config)
        {
            var message = "Service " + config.ServiceName + " Cloned!";

            if (!ModelState.IsValid)
            {

            }

            var serviceObject = ConfigurationVM.Convert(config);
            var main = new MainService();
            main.Execute(serviceObject);

            return Json(new { success = true, message });
        }
    }
}