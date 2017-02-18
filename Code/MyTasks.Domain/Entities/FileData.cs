using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities
{
    public class FileData
    {
        public FileData()
        {
        
        }
        public int FileId { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Path { get; set; }

        public int TicketDetailId { get; set; }

        public string FileGuid { get; set; }

        public Ticket Ticket { get; set; }

        public Guid FileGuidAsGuid {
            get { return new Guid(FileGuid); }
        }


    }
}
