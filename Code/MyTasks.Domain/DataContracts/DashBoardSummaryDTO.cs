using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class DashBoardSummaryDTO
    {
        public int TotalUsers { get; set; }
        public int TotalProjects { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalTickets { get; set; }

    }
}
