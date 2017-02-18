using System;

namespace BigLamp.DatabaseInstaller.Exceptions
{
    public class DatabaseInstallerException : Exception
    {
        public DatabaseInstallerException()
        {
        
        }

        public DatabaseInstallerException(string message) : base(message)
        {
        }

        public DatabaseInstallerException(string message, Exception innerException) : base(message, innerException)
        {
        }

    
        protected DatabaseInstallerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context)
        {
        }

    }
}
