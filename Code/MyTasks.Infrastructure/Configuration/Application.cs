using System.Configuration;

namespace MyTasks.Infrastructure.Configuration
{
    public static  class Application
    {
        //public const string AuthenticationCookieName = ".AquaVet.ApplicationCookie";

        public static string ConnectionString
        {
            get {
                const string appSettingsKey = "DefaultConnection";
                return ConfigurationManager.ConnectionStrings[appSettingsKey].ConnectionString;
            }
        }

        public static string RootConnectionString
        {
            get
            {
                const string appSettingsKey = "RootConnection";
                return ConfigurationManager.ConnectionStrings[appSettingsKey].ConnectionString;
            }
        }



        public static string GetAppSetting(string appSettingsKey)
        {
            return ConfigurationManager.AppSettings[appSettingsKey];
        }

        

    }
}
