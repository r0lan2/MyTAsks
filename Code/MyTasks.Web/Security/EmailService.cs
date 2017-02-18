using MyTasks.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BigLamp.AspNet.Identity.MySQL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyTasks.Data.Repositories;
using MyTasks.Infrastructure.Mail;
using MyTasks.Web.Extensions;
using MyTasks.Web.Infrastructure.Localization;
using MyTasks.Infrastructure.Security;


namespace MyTasks.Web.Security
{
    public class EmailService : IIdentityMessageService, IDisposable
    {

        public string NoReplyAddress
        {
            get
            {
                return MvcApplication.ApplicationSettings.SenderEmail;
            }
        }

        public string ToReplyAddress
        {
            get { return MvcApplication.ApplicationSettings.IssuesRecipientEmail; }
        }

        public string ApplicationBasePath
        {
            get { return VirtualPathUtility.ToAbsolute("~/"); }
        }

        public Task SendAsync(IdentityMessage message)
        {
            EmailMessage emailMessageWithPassword = message as EmailMessage;
            var manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var mailClient = new SmtpClient();
            var urlActivcation = message.Body;
            var toAddress = message.Destination;
            var user = manager.FindByEmail(toAddress);

            var password= emailMessageWithPassword !=null? emailMessageWithPassword.Password:"";

            MailMessage emailMessage = null;
           
            if (message.Subject == TypeOfMails.ForgotPassword)
                emailMessage = EmailMessageFactory.GetForgotEmailFromTemplate(NoReplyAddress, toAddress, user.Email, user.FullName, user.Language, ApplicationBasePath, ToReplyAddress, urlActivcation);
            
            else
                emailMessage = EmailMessageFactory.GetConfirmationMailFromTemplate(NoReplyAddress, toAddress, user.Email, user.FullName, user.Language, ApplicationBasePath, ToReplyAddress, urlActivcation, password);


            emailMessage.IsBodyHtml = true;
            return mailClient.SendMailAsync(emailMessage);

        }




       


        public static EmailService Create()
        {
            return new EmailService();
        }

        public void Dispose()
        {
          
        }
    }

    public class EmailMessage: IdentityMessage
    {
        public string Password { get; set; }
    }

}