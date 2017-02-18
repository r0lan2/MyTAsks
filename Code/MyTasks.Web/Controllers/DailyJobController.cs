using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;


namespace MyTasks.Web.Controllers
{
    public class DailyJobController : Controller
    {
        private TicketUnitOfWork unitOfWork = new TicketUnitOfWork();


        // GET: DailyJob
        public ActionResult Index()
        {

            return View();
        }

        public List<Users> getActiveUsers()
        {
            var users = unitOfWork.UserRepository.All();
            return users.ToList();
        }

        //public ActionResult Create()
        //{

        //}

    }
}