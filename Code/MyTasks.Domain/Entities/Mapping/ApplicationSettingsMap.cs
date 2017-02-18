using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class ApplicationSettingsMap : EntityTypeConfiguration<ApplicationSettings>
    {
        public ApplicationSettingsMap()
        {
            // Primary Key
            this.HasKey(t => t.SettingId);

            // Properties
            // Table & Column Mappings
            this.ToTable("applicationsettings");
            this.Property(t => t.ConfigurationKey).HasColumnName("ConfigurationKey");
            this.Property(t => t.ConfigurationValue).HasColumnName("ConfigurationValue");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedTime");
        }
    }
}
