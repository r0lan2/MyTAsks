using System.Configuration;
using BigLamp.DatabaseInstaller.Configuration;

namespace BigLamp.DatabaseInstaller.Configuration
{
    public class DatabaseObjectsList : ConfigurationSection
    {
        [ConfigurationProperty("StoredProcedureNames")]
        public ObjectNamesCollection StoredProcedures
        {
            get { return (ObjectNamesCollection)(this["StoredProcedureNames"]); }
        }

        [ConfigurationProperty("FunctionNames")]
        public ObjectNamesCollection Functions
        {
            get { return (ObjectNamesCollection)(this["FunctionNames"]); }
        }

        [ConfigurationProperty("ViewNames")]
        public ObjectNamesCollection Views
        {
            get { return (ObjectNamesCollection)(this["ViewNames"]); }
        }


        [ConfigurationProperty("Exclude")]
        public ObjectNamesCollection Exclude
        {
            get { return (ObjectNamesCollection)(this["Exclude"]); }
        }
    }
}