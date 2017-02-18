using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class WorkingOnMap : EntityTypeConfiguration<WorkingOn>
    {
        public WorkingOnMap()
        {
            this.HasKey(t => t.WorkingOnId);

            // Properties
            // Table & Column Mappings
            this.ToTable("workingon");
            this.Property(t => t.TicketId).HasColumnName("TicketDetailId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");

            this.HasRequired(t => t.ticket)
                         .WithMany(t => t.workingon)
                         .HasForeignKey(d => d.TicketId);

        }

    }
}
