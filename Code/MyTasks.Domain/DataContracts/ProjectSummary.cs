using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class ProjectSummary
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
        public int ResolvedTickets { get; set; }
    }
}
