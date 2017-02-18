using System.Configuration;


namespace BigLamp.DatabaseInstaller.Configuration
{
    public class DatabaseObjectsListConfiguration : ConfigurationSectionGroup
    {
        public static DatabaseObjectsList GetConfig()
        {
            return (DatabaseObjectsList)(ConfigurationManager.GetSection("biglamp/databaseobjectslist"));
        }
    }

}

