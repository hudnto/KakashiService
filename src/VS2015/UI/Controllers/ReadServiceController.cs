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
        // GET: ReadService
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
            var readService = new ReadService();
            var serviceObject = new ServiceObject(){Url = Url};
            try
            {
                readService.Execute(serviceObject);
                var response = PrepareResponse(serviceObject);
                return Json(new { success = true, response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {             
                return Json(new { success = false, modal = new { title = "Operation Fail!" } }, JsonRequestBehavior.AllowGet);
            }
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

            return new {name, totalFunctions = serviceObject.Functions.Count, functions = functionList, totalObject};
        }
    }
}