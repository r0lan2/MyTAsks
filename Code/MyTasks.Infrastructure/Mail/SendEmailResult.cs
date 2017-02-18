
namespace MyTasks.Infrastructure.Mail
{
    using System;
    using System.Net.Mail;

    public class SendEmailResult
    {
        #region Properties

        public Exception Exception
        {
            get;
            set;
        }

        public bool Failed
        {
            get { return Exception != null; }
        }

        public MailMessage Message
        {
            get;
            internal set;
        }

        #endregion Properties
    }
}