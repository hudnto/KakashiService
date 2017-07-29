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
            return View();
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
                return Json(new { success = true, modal = new {  title = "Operation Completed!" } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {             
                return Json(new { success = false, modal = new { title = "Operation Fail!" } }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}