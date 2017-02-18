using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTasks.Data.Repositories;
using MyTasks.Domain;

namespace MyTasks.Web.Infrastructure.Configuration
{
    public sealed class ApplicationSettingContainer
    {
        public List<ApplicationSettings> Settings { get; set; }

        private ApplicationSettingContainer(List<ApplicationSettings> settings)
        {
            Settings = settings;
        }

        public string GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey key)
        {
            var keyAsString = key.ToString();
            try
            {
                var settings = Settings.FirstOrDefault((ask) => ask.ConfigurationKey == keyAsString);
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

        private void UpdateApplicationSettingByKey(string key, string value)
        {
            var settings = Settings.FirstOrDefault((ask) => ask.ConfigurationKey == key);
            if (settings != null)
            {
                settings.ConfigurationValue = value;
            }
        }
        public void UpdateCachedApplicationSettings(List<object> changedSettings)
        {
            foreach (var setting in changedSettings)
            {
                var applicationSetting = (ApplicationSettings)setting;
                MvcApplication.ApplicationSettings.UpdateApplicationSettingByKey(applicationSetting.ConfigurationKey,
                  applicationSetting.ConfigurationValue);

            }
        }
        public static ApplicationSettingContainer Create(List<ApplicationSettings> settings)
        {
            var applicationSettings = new ApplicationSettingContainer(settings);
            return applicationSettings;
        }

        public string IssuesRecipientEmail
        {
            get
            {
                return GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey.IssuesRecipientEmail);
            }
        }

        public string SenderEmail
        {
            get
            {
                return GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey.SenderEmail);
            }
        }

        public string FileStoreBasePath
        {
            get
            {
                return GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey.FileStoreBasePath);
            }
        }

        public string TicketPath
        {
            get
            {
                var root = FileStoreBasePath;
                var ticket= GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey.FileStoreTicketFiles);
                return root + ticket;
            }
        }

        public string ProfilesPicturePath
        {
            get
            {
                var root = FileStoreBasePath;
                var profilePicturesPath = GetApplicationSettingByKey(ApplicationSettingsRepository.ApplicationSettingKey.FileStoreProfilesFiles);
                return root + profilePicturesPath;
            }
        }




    }
}