using MyTasks.Domain.Base;


namespace MyTasks.Domain
{
    public partial class ApplicationSettings : BaseObject
    {
        public ApplicationSettings()
        {
            
        }
    
        public int SettingId { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }

    }
}
