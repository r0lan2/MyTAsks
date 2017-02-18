using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Infrastructure.Configuration
{
    public class ConfigurationHelper
    {

        public static string GetStringValueFromConfig(string settingName, bool throwExceptionIfEmpty)
        {
            string str;
            str = ConfigurationManager.AppSettings[settingName];
            if (throwExceptionIfEmpty && string.IsNullOrEmpty(str))
                throw new ConfigurationErrorsException(settingName + " is not set in the config file");
            else
                return str;
        }

        public static string GetStringValueFromConfig(string settingName)
        {
            return ConfigurationHelper.GetStringValueFromConfig(settingName, true);
        }


        public static ConfigurationSection GetConfigurationSection(string sectionName)
        {
            object objectValue = RuntimeHelpers.GetObjectValue(ConfigurationManager.GetSection(sectionName));
            if (objectValue == null)
                throw new ConfigurationErrorsException(string.Format("Failed to get section {0} from config", (object)sectionName));
                ConfigurationSection configurationSection = objectValue as ConfigurationSection;
                if (configurationSection == null)
                    throw new ConfigurationErrorsException(string.Format("Invalid type in section {0}", (object)sectionName), (Exception)new Exceptions.InvalidCastException(typeof(ConfigurationSection), RuntimeHelpers.GetObjectValue(objectValue)));
            else
                return configurationSection;
        }

    }
}
