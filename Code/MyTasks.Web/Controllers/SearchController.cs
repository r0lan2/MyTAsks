using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;
using MyTasks.Localization.Desktop;
using MyTasks.Web.Models;
using WebGrease.Css.Extensions;
namespace MyTasks.Web.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    public class SearchController : BaseController
    {
        private TicketUnitOfWork unitOfWork = new TicketUnitOfWork();

        [HttpGet]
        public ActionResult Index(SearchViewModel model)
        {
            // PopulateDropdowns();
            model.Projects= (from d in unitOfWork.ProjectRepository.All() orderby d.ProjectName select d).ToList();
            model.Categories = (from c in unitOfWork.CategoryRepository.All() orderby c.Name select c).ToList();
            model.UserList= (from u in unitOfWork.UserRepository.All() orderby u.UserName select u).ToList();
            model.Priorities= (from p in unitOfWork.PriorityRepository.All() orderby p.PriorityId select p).ToList();
            model.SearchResult = unitOfWork.RunSearch(model.SelectedProjectId,model.SelectedCategoryId,model.SelectedUserId,model.SelectedPriorityId).ToList();

            return View(model);
        }

    }
}