using System;
using System.Linq;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Domain;

namespace MyTasks.Data.Repositories
{
    public class ApplicationSettingsRepository : GenericRepository<IWorkinghoursDataContext, ApplicationSettings>
    {

        public ApplicationSettingsRepository(IWorkinghoursDataContext context)
            : base(context)
        {

        }

        public ApplicationSettingsRepository()
            : base(new WorkinghoursDataContext())
        {

        }
    

        public string GetApplicationSettingByKey(ApplicationSettingKey key)
        {
            var keyAsString = key.ToString();
            try
            {
                var settings = Context.ApplicationSettings.FirstOrDefault((ask) => ask.ConfigurationKey == keyAsString);
                if (settings != null)
                {
                    if (!string.IsNullOrEmpty(settings.ConfigurationValue))
                    {
                        return settings.ConfigurationValue;
                    }
                    return settings.DefaultValue;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public void UpdateSetting(string key, string value)
        {

            UpdateSettingValue( key,value);
        }

        private void UpdateSettingValue(string stringKey,string value)
        {
            var setting = Context.ApplicationSettings.FirstOrDefault((ask) => ask.ConfigurationKey == stringKey);
            if (setting != null)
            {
                setting.ConfigurationValue = value;
                Update(setting);
            }
            setting = Context.ApplicationSettings.FirstOrDefault((ask) => ask.ConfigurationKey == stringKey);
        }


        public void UpdateSetting(ApplicationSettingKey key, string value)
        {
            var stringKey = key.ToString();
            UpdateSettingValue(stringKey,value);

        }
        public enum ApplicationSettingKey
        {
            SenderEmail,
            IssuesRecipientEmail,
            FileStoreBasePath,
            FileStoreTicketFiles,
            FileStoreProfilesFiles
        }

    }
}
