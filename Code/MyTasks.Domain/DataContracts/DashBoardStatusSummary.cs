using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class DashBoardStatusSummary
    {
        public string Status { get; set; }
        public int NumberOfTickets { get; set; }
        public double Percentage { get; set; }
    }
}
