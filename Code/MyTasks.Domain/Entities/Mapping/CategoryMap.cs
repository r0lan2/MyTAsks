using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Domain;

namespace MyTasks.Domain.Entities.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryId);

            // Properties
            // Table & Column Mappings
            this.ToTable("category");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Name).HasColumnName("Name");

            //// Relationships
            //this.HasRequired(t => t.Dictionary)
            //    .WithMany(t => t.DeviceTypes)
            //    .HasForeignKey(d => d.DictionaryId);

        }



    }
}
