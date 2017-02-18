using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyTasks.Web.Models;
using MyTasks.Web.Security;

namespace MyTasks.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //User.Identity.AsClaimsIdentity().UserId()
            //UserAdminInitializer.InitializeUserAdmin();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}