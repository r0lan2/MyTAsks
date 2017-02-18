using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            this.HasKey(t => t.ProjectId);

            // Properties
            // Table & Column Mappings
            this.ToTable("project");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ProjectManagerId).HasColumnName("ProjectManagerId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");

            //// Relationships

            this.HasRequired(t => t.customer)
               .WithMany(t => t.Projects)
               .HasForeignKey(d => d.CustomerId);
        }
    }
}
