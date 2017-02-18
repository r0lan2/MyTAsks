using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace MyTasks.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidCastException : System.InvalidCastException
    {
        private Type _expectedType;
        private Type _actualType;

        public override string Message
        {
            get
            {
                string str1 = "";
                string str2 = "";
                if (this._expectedType != null)
                    str1 = this._expectedType.FullName;
                if (this._actualType != null)
                    str2 = this._actualType.FullName;
                return base.Message +
                       string.Format(" Invalid cast. Expected {0}, got {1}.", (object) str1, (object) str2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public InvalidCastException()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public InvalidCastException(string message)
            : base(message)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public InvalidCastException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidCastException(Type expectedType, Type actualType, string message)
            : base(message)
        {
            this._expectedType = expectedType;
            this._actualType = actualType;
        }

        public InvalidCastException(Type expectedType, object actualType, string message)
            : base(message)
        {
            this._expectedType = expectedType;
            if (actualType == null)
                return;
            this._actualType = actualType.GetType();
        }

        public InvalidCastException(Type expectedType, object actualType)
        {
            this._expectedType = expectedType;
            if (actualType == null)
                return;
            this._actualType = actualType.GetType();
        }

        protected InvalidCastException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                return;
            this._expectedType = (Type) info.GetValue("expectedType", typeof (Type));
            this._actualType = (Type) info.GetValue("actualType", typeof (Type));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info == null)
                return;
            info.AddValue("expectedType", (object) this._expectedType);
            info.AddValue("actualType", (object) this._actualType);
        }
    }
}