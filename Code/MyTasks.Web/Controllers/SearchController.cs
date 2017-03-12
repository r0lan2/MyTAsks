using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
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
        public ActionResult Index()
        {
            // PopulateDropdowns();
            SearchViewModel cv = new SearchViewModel();
            cv.Projects= (from d in unitOfWork.ProjectRepository.All() orderby d.ProjectName select d).ToList();
            return View(cv);
        }


        [HttpPost]
        public ActionResult Index(FormCollection fc, SearchViewModel objcv)
        {
            string selectedProjectId = fc["SelectedProjectId"];

            ViewData["SelectedProjectId"] = selectedProjectId;
            
            int projectId = Convert.ToInt32(selectedProjectId);

            SearchViewModel cv = new SearchViewModel();

            cv.Projects = (from d in unitOfWork.ProjectRepository.All() orderby d.ProjectName select d).ToList();

            cv.SelectedProjectId = objcv.SelectedProjectId;

            cv.SearchResult = unitOfWork.RunSearch(projectId);

            return View(cv);
        }

        //public void PopulateDropdowns()
        //{
        //    PopulateCategories();
        //    PopulatePriorities();
        //    PopulateProjects();
        //    PopulateUsers();
        //}

        //public void PopulateDropdowns(int customerId, int categoryId, int priorityId, int projectId, string userId, int areaId)
        //{
        //    PopulateCustomers(customerId);
        //    PopulateCategories(categoryId);
        //    PopulatePriorities(priorityId);
        //    PopulateProjects(projectId);
        //    PopulateUsers(userId);
        //}

        public void PopulateProjects(object selectProjectId = null)
        {
            var projects = from d in unitOfWork.ProjectRepository.All() orderby d.ProjectName select d;
            ViewBag.projectList = new SelectList(projects, "ProjectId", "ProjectName", selectProjectId);
        }


        //public void PopulateUsers(object selectedUserId = null)
        //{
        //    var users = from u in unitOfWork.UserRepository.All() orderby u.UserName select u;
        //    ViewBag.UserList = new SelectList(users, "Id", "UserName", selectedUserId);
        //}

        //public void PopulateCustomers(object selectCustomerId = null)
        //{
        //    var customers = from c in unitOfWork.CustomerRepository.All() orderby c.Name select c;
        //    ViewBag.CustomerList = new SelectList(customers, "CustomerId", "Name", selectCustomerId);
        //}

        //public void PopulateCategories(object selectCategoryId = null)
        //{
        //    var categories = from c in unitOfWork.CategoryRepository.All() orderby c.Name select c;
        //    ViewBag.CategoryList = new SelectList(categories, "CategoryId", "Name", selectCategoryId);
        //}

        //public void PopulatePriorities(object selectPriorityId = null)
        //{
        //    var priorities = from p in unitOfWork.PriorityRepository.All() orderby p.PriorityId select p;
        //    priorities.ForEach(p => p.Name = Desktop.ResourceManager.GetString(p.Code));
        //    ViewBag.PriorityList = new SelectList(priorities, "PriorityId", "Name", selectPriorityId);
        //}

       

    }
}