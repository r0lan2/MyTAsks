using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Domain.Entities;

namespace MyTasks.Domain.Entities.Mapping
{
    public class DbVersionMap : EntityTypeConfiguration<Dbversion>
    {
        public DbVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.Version);

            // Properties
            // Table & Column Mappings
            this.ToTable("dbversion");
            this.Property(t => t.ScriptFileName).HasColumnName("ScriptFileName");

        }

    }
}
