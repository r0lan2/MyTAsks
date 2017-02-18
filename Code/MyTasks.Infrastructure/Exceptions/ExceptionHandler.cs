using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace MyTasks.Infrastructure.Exceptions
{
    public class ExceptionHandler
    {
        public const string BugCouldNotSubmitMessage = "Couldn't submit exception";
        public const string SuccesSubmitBugMessage = "bug was sent! thank you for you feedback!";

        public string FromEmail { get; set; }
        public string ErrorDetails { get; set; }
        public string IssuesMailTo { get; set; }


        public string SubmitBug()
        {
            try
            {
                SendMessage();
                return SuccesSubmitBugMessage;
            }
            catch (Exception)
            {
                return BugCouldNotSubmitMessage;
            }
        }


        public void SendMessage()
        {
            var subject = "this is a error";
            var body = ErrorDetails;
            var email = new MailMessage(
              FromEmail,
              IssuesMailTo,
              subject,
              body
              );
            email.BodyEncoding = Encoding.UTF8;
            email.IsBodyHtml = true;
            email.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain));
            email.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
            var mailClient = new SmtpClient();
             mailClient.Send(email);

        }


    }
}