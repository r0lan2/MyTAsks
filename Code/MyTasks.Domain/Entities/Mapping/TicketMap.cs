using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class TicketMap : EntityTypeConfiguration<Ticket>
    {
        public TicketMap()
        {
            this.HasKey(t => t.TicketDetailId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ticket");
            this.Property(t => t.TicketNumber).HasColumnName("TicketNumber");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.PriorityId).HasColumnName("PriorityId");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.TicketStatusId).HasColumnName("TicketStatusId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.IsHtml).HasColumnName("IsHtml");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.OwnerUserId).HasColumnName("OwnerUserId");
            this.Property(t => t.AssignedTo).HasColumnName("AssignedTo");
            this.Property(t => t.LastUpdateBy).HasColumnName("LastUpdateBy");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.IsLastDetail).HasColumnName("IsLastDetail");

            this.HasRequired(t => t.Project)
                          .WithMany(t => t.Tickets)
                          .HasForeignKey(d => d.ProjectId);

            this.HasRequired(t => t.Area)
                          .WithMany(t => t.Tickets)
                          .HasForeignKey(d => d.AreaId);

            this.HasRequired(t => t.Priority)
                         .WithMany(t => t.Tickets)
                         .HasForeignKey(d => d.PriorityId);

            this.HasRequired(t => t.Category)
                        .WithMany(t => t.Tickets)
                        .HasForeignKey(d => d.CategoryId);

            this.HasRequired(t => t.Ticketstatus)
                      .WithMany(t => t.Tickets)
                      .HasForeignKey(d => d.TicketStatusId);


        }
    }
}
