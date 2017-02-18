using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;

namespace MyTasks.Web.Controllers
{
    public class DashboardController : BaseController
    {
        private TicketUnitOfWork unitOfWork = new TicketUnitOfWork();
        // GET: DashBoard
        public ActionResult Index()
        {
            var projectsSummary = unitOfWork.GetProjectsSummary();
            return View(projectsSummary);
        }
    }
}