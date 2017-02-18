using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using MyTasks.Infrastructure.Security;


namespace MyTasks.Infrastructure.Mail
{
    public class Mailer
    {
        private const int MIinExpires = 30;

        private readonly IEmailClient _emailClient;

        public Mailer(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        

        public string GetDigest(string userName)
        {
            //create encription key as setting .. for now we are using default key.
            return Encryption.Encrypt(String.Format("{0}&{1}",
                userName,
                DateTime.Now.AddMinutes(MIinExpires).Ticks), string.Empty);
        }


    }
}
