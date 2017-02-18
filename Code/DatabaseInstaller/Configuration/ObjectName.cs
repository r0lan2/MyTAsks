using System;
using System.Configuration;

namespace BigLamp.DatabaseInstaller.Configuration
{
    public class ObjectName : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string FieldName
        {
            get { return Convert.ToString(this["name"]); }
        }
    }

}
