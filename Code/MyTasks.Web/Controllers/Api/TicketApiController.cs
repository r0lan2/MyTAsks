using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;

namespace MyTasks.Web.Controllers.Api
{
    public class TicketApiController : ApiController
    {

        private TicketUnitOfWork unitOfWork = new TicketUnitOfWork();

        [System.Web.Http.Route("api/TicketApi/TicketSearch")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TicketSearchResult> TicketSearch(string term)
        {
            var result= unitOfWork.TicketSearch(term);
            return result;
        }
        
        [System.Web.Http.Route("api/TicketApi/GetProjectsSummary")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProjectSummary> GetProjectsSummary()
        {
            var projectsSummary = unitOfWork.GetProjectsSummary();
            return projectsSummary;
        }

        [System.Web.Http.Route("api/TicketApi/GetUsersSummary")]
        [System.Web.Http.HttpGet]
        public IEnumerable<UserSummary> GetUsersSummary()
        {
            var usersSummary = unitOfWork.GetUserSummary();
            return usersSummary;
        }
        
        public IEnumerable<AreaDTO> GetAreas(int projectId)
        {
            var areas = unitOfWork.GetAreasByProject(projectId);
            return areas;
        }

        [System.Web.Http.Route("api/TicketApi/GetStatusList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<String> GetStatusList()
        {
            var statusList = unitOfWork.StatusRepository.All().Select(s=>s.Name).ToList();
            return statusList;
        }


        [System.Web.Http.Route("api/TicketApi/GetPriorityList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<string> GetPriorityList()
        {
            var priorityList = unitOfWork.PriorityRepository.All().Select(s => s.Name).ToList();
            return priorityList;
        }

        [System.Web.Http.Route("api/TicketApi/GetCategoryList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<string> GetCategoryList()
        {
            var categoryList = unitOfWork.CategoryRepository.All().Select(s => s.Name).ToList();
            return categoryList;
        }


        [System.Web.Http.Route("api/TicketApi/GetProjects")]
        [System.Web.Http.HttpGet]
        public IEnumerable<string> GetProjects()
        {
            var projectList = unitOfWork.ProjectRepository.All().Select(s => s.ProjectName).ToList();
            return projectList;
        }

        [System.Web.Http.Route("api/TicketApi/GetProjectList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProjectDTO> GetProjectList()
        {
            var allProjects = unitOfWork.ProjectRepository.All();
            return allProjects.Select(project => new ProjectDTO()
            {
                ProjectId = project.ProjectId, ProjectName = project.ProjectName
            }).ToList();

        }


        [System.Web.Http.Route("api/TicketApi/GetDashBoardSummary")]
        [System.Web.Http.HttpGet]
        public DashBoardSummaryDTO GetDashBoardSummary()
        {
            return unitOfWork.GetDashBoardSummary();
        }

        [System.Web.Http.Route("api/TicketApi/GetDashBoardOpenTicketByPriority")]
        [System.Web.Http.HttpGet]
        public List<DashBoardOpenByPriority> GetDashBoardOpenTicketByPriority(string projects)
        {
            return unitOfWork.GetDashBoardOpenTicketByPriority(projects);
        }

        [System.Web.Http.Route("api/TicketApi/GetDashBoardStatusSummary")]
        [System.Web.Http.HttpGet]
        public List<DashBoardStatusSummary> GetDashBoardStatusSummary()
        {
            return unitOfWork.GetDashBoardStatusSummary();
        }
        

    }
}
