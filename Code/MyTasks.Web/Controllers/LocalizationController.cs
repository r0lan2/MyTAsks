using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTasks.Web.Controllers
{
    public class LocalizationController : Controller
    {
        // GET: Localization
        public ActionResult Index()
        {
            Response.ContentType = "text/javascript";
            return View();
        }
    }
}