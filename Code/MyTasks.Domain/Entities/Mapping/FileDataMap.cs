using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class FileDataMap : EntityTypeConfiguration<FileData>
    {
        public FileDataMap()
        {
            // Primary Key
            this.HasKey(t => t.FileId);

            // Properties
            // Table & Column Mappings
            this.ToTable("filedata");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.TicketDetailId).HasColumnName("TicketDetailId");

            //TODO: Check after changes in usermanagement to upload files.
            this.HasRequired(t => t.Ticket)
                      .WithMany(t => t.Files)
                      .HasForeignKey(d => d.TicketDetailId);

        }
    }
}
