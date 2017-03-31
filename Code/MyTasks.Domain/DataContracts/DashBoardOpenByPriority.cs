using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class DashBoardOpenByPriority
    {
        public string Priority { get; set; }
        public int NumberOfTickets { get; set; }
        public double Percentage { get; set; }

    }
}
