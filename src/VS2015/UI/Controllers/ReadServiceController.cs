using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KakashiService.Core.Entities;
using KakashiService.Core.Services;
using KakashiService.Web.ViewModel;

namespace KakashiService.Web.Controllers
{
    public class ReadServiceController : Controller
    {
        private ReadService _readService;
        public ReadServiceController()
        {
            _readService = new ReadService();
        }

        public ActionResult Index()
        {
            return View(new ConfigurationVM(true));
        }

        [HttpPost]
        public JsonResult Read(String Url)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }
            var serviceObject = new ServiceObject(){Url = Url};
            try
            {
                _readService.Execute(serviceObject);
                var response = PrepareResponse(serviceObject);
                return Json(new { success = true, response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {             
                return Json(new { success = false, modal = new { title = "Operation Fail!" } }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportFile()
        {
            var file = HttpContext.Request.Files[0];
            var stream = file.InputStream;

            var serviceObject = new ServiceObject();
            stream.CopyTo(serviceObject.FileStream);
            _readService.Execute(serviceObject);
            var response = PrepareResponse(serviceObject);
            return Json(new { success = true, response }, JsonRequestBehavior.AllowGet);
        }

        private object PrepareResponse(ServiceObject serviceObject)
        {
            var functionList = new List<String>();
            foreach (var function in serviceObject.Functions)
            {
                var parameters = String.Empty;
                foreach (var parameter in function.Parameters.OrderBy(a => a.Order))
                {
                    var comma = parameter.Order == function.Parameters.Max(a => a.Order) ? String.Empty : ", ";
                    parameters += parameter.TypeName + comma;
                }
                functionList.Add(String.Format("{0} {1}({2});", function.ReturnType, function.Name, parameters));
            }
            var name = serviceObject.OriginServiceName;
            var totalObject = serviceObject.ObjectTypes.Count;
            var objectList = new List<String>();
            foreach (var objectType in serviceObject.ObjectTypes)
            {
                var parameters = String.Empty;
                foreach (var attribute in objectType.Attributes)
                {
                    var comma = objectType.Attributes.Any(a => attribute == a) ? ", " : "";
                    parameters += attribute+comma;
                }
                objectList.Add(String.Format("{0} - ({1});", objectType.TypeName, parameters));
            }
            return new {name, totalFunctions = serviceObject.Functions.Count, functions = functionList, totalObject, objects = objectList};
        }
    }
}