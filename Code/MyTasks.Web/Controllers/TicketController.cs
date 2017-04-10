

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
    public class TicketController : BaseController
    {
        private TicketUnitOfWork unitOfWork = new TicketUnitOfWork();
        // GET: Ticket
        public ActionResult Index()
        {
            var currentUserId = CurrentUserId();
            var tickets = unitOfWork.GetOpenTicketsByUser(currentUserId);
            return View(tickets);
        }

        public ActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update()
        {
            var model = GetTicketModel();
            Data.Validators.ValidationStatus status = null;
            var fileViewModel = new FileViewModel();
            try
            {
                status = unitOfWork.Validate(model);
                if (status.IsValid)
                {
                    model.LastUpdateBy = CurrentUserId();
                    var ticket = unitOfWork.NewTicketEdition(model);
                    var files = fileViewModel.SavingTicketFilesIfExists(ticket.TicketDetailId);
                    unitOfWork.AddFileData(ticket, files);
                }
                return new JsonResult { Data = new { status = status } };
            }
            catch (Exception)
            {

                return new JsonResult { Data = new { status = new Data.Validators.ValidationStatus { IsValid = false, ErrorMessage = Localization.Desktop.Desktop.UnableToSaveChanges } } };
            }
        }

        private TicketData GetTicketModel()
        {
            var model = new TicketData();
            model.Title = Request.Form.Get("Title");
            model.ProjectId = int.Parse(Request.Form.Get("ProjectId"));
            model.AreaId = int.Parse(Request.Form.Get("AreaId"));
            model.CategoryId = int.Parse(Request.Form.Get("CategoryId")) ;
            model.UserId = Request.Form.Get("UserId");
            model.PriorityId = int.Parse(Request.Form.Get("PriorityId"));
            model.OwnerUserId = Request.Form.Get("OwnerUserId");
            model.TicketNumber = int.Parse(Request.Form.Get("TicketNumber"));
            model.CreatedBy = Request.Form.Get("CreatedBy");
            model.Content = Request.Form.Get("Content");
            model.IsBillable = bool.Parse(Request.Form.Get("IsBillable"));
            model.StatusId = int.Parse(Request.Form.Get("StatusId"));
            return model;
        }

        [ValidateInput(false)] 
        [HttpPost]
        public ActionResult Create(TicketData model)
        {
            var fileViewModel = new FileViewModel();
            if (model.TicketNumber <= 0)
            {
                model.CreatedBy = CurrentUserId();
                var ticket= unitOfWork.NewTicket(model);
                var files = fileViewModel.SavingTicketFilesIfExists(ticket.TicketDetailId);
                unitOfWork.AddFileData(ticket, files);
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult TicketHistory(int ticketNumber)
        {
            var ticketHistoryData=  unitOfWork.GetTicketHistory(ticketNumber);
            var lastEditedTicket = ticketHistoryData.LastEditedTicket;
            PopulateDropdowns(lastEditedTicket.CustomerId, lastEditedTicket.CategoryId, lastEditedTicket.PriorityId, lastEditedTicket.ProjectId, lastEditedTicket.UserId, lastEditedTicket.AreaId);
            return  View(ticketHistoryData);
            
        }

        public ActionResult TicketView(int ticketNumber)
        {
            var ticketHistoryData = unitOfWork.GetTicketHistory(ticketNumber);
            var lastEditedTicket = ticketHistoryData.LastEditedTicket;
            PopulateDropdowns(lastEditedTicket.CustomerId, lastEditedTicket.CategoryId, lastEditedTicket.PriorityId, lastEditedTicket.ProjectId, lastEditedTicket.UserId, lastEditedTicket.AreaId);
            return View(ticketHistoryData);
        }


        public List<Project> GetProjects()
        {
            return unitOfWork.GetProjects();
        }

        public List<Category> GetCategories()
        {
            return unitOfWork.GetCategories();
        }

        public List<Priority> GetPriorities()
        {
            return unitOfWork.GetPriorities();
        }
        
        public void PopulateDropdowns(int customerId, int categoryId, int priorityId, int projectId, string userId,int areaId)
        {
            PopulateCustomers(customerId);
            PopulateCategories(categoryId);
            PopulatePriorities(priorityId);
            PopulateProjects(projectId);
            PopulateUsers(userId);
            PopulateAreas(areaId);
        }

        public void PopulateDropdowns()
        {
            PopulateCustomers();
            PopulateCategories();
            PopulatePriorities();
            PopulateProjects();
            PopulateUsers();
            PopulateAreas();

        }
        
        public void PopulateAreas(object selectedAreaId = null)
        {
            var areas = from u in unitOfWork.AreaRepository.All() orderby u.Name select u;
            ViewBag.AreaId = new SelectList(areas, "AreaId", "Name", selectedAreaId);
        }
        
        public void PopulateUsers(object selectedUserId = null)
        {
            var users = from u in unitOfWork.UserRepository.All()  orderby u.UserName select u;
            ViewBag.UserList = new SelectList(users, "Id", "UserName", selectedUserId);
        }

        public void PopulateCustomers(object selectCustomerId = null)
        {
            var customers = from c in unitOfWork.CustomerRepository.All() orderby c.Name select c;
            ViewBag.CustomerList = new SelectList(customers, "CustomerId", "Name", selectCustomerId);
        }

        public void PopulateCategories(object selectCategoryId=null)
        {
            var categories = from c in unitOfWork.CategoryRepository.All() orderby c.Name select c;
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "Name", selectCategoryId);
        }

        public void PopulatePriorities(object selectPriorityId= null)
        {
            var priorities = from p in unitOfWork.PriorityRepository.All() orderby p.PriorityId select p;
            priorities.ForEach(p=> p.Name= Desktop.ResourceManager.GetString(p.Code));
            ViewBag.PriorityList = new SelectList(priorities, "PriorityId", "Name", selectPriorityId);
        }

        public void PopulateProjects(object selectProjectId=null)
        {
            var projects = from d in unitOfWork.ProjectRepository.All() orderby d.ProjectName select d;
            ViewBag.projectList = new SelectList(projects, "ProjectId", "ProjectName", selectProjectId);
        }


    }
}