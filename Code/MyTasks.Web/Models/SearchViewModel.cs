using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;

namespace MyTasks.Web.Models
{
    public class SearchViewModel
    {
        public int SelectedProjectId { get; set; }
        public int SelectedCategoryId { get; set; }
        public int SelectedPriorityId { get; set; }
        public string SelectedUserId { get; set; }
        public List<Project> Projects { get; set; }
        public List<Category> Categories { get; set; }
        public List<Priority> Priorities { get; set; }
        public List<Users> UserList { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        public Int32 TotalRecords { get; set; }
        public List<TicketListItem> SearchResult { get; set; }

        public SearchViewModel()
        {
            Page = 1;
            PageSize = 5;
            //Sort = "ProductId";
            //SortDir = "DESC";
        }

    }
}