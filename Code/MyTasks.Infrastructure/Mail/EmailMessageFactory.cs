// <copyright file=EmailMessageFactory.cs company=Justin Spradlin>
//      Copyright (c) 2011 Justin Spradlin.  ALL RIGHTS RESERVED
// </copyright>
// <product></product>
// <author>jspradlin</author>
// <created>Friday, April 29, 2011</created>
// <lastedit>Friday, April 29, 2011</lastedit>

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using File = System.IO.File;
using MyTasks.Localization;
using MyTasks.Localization.Desktop;

namespace MyTasks.Infrastructure.Mail
{
    using System.Configuration;
    using System.Net.Mail;

    /// <summary>
    /// This class contains methods to create a mail message to be sent to a user.
    /// </summary>
    public static class EmailMessageFactory
    {
        private static string _basePath;
      
        public static string BasePath
        {
            get
            {
                if (string.IsNullOrEmpty(_basePath))
                    _basePath = HostingEnvironment.MapPath("~/");
                return _basePath;
            }
            set { _basePath = value; }
        }

        
        private static string GetTemplate(string userCulture, string templatePath, string templateName)
        {
            var templateEmail = $"{templateName}.{userCulture}.cshtml";
            return File.ReadAllText(BasePath + templatePath + @"\" + templateEmail);
        }

        private static string GetTemplatePath()
        {
            var templatePath = ConfigurationManager.AppSettings["ForgotPasswordEmailTemplatePath"];
                       
            return templatePath;
        }

        public static string GetMailCss()
        {
            var templatePath = ConfigurationManager.AppSettings["AssessmentEmailTemplatePath"];
            var css = File.ReadAllText(HostingEnvironment.MapPath("~/") + templatePath + "/css/style.css");

            return "<style>" + css + "</style>";
        }

        public static MailMessage GetConfirmationMailFromTemplate(string fromAddress, string toAddress, string email,
            string userFullName,
            string userCulture, string applicationBasePath, string addressToReply, string urlConfirmationNewUser, string password)
        {
            var templatePath = GetTemplatePath();
            var template = GetTemplate(userCulture, templatePath, MailTemplates.ConfirmationNewUser);


            var defaultBodyContent = Desktop.ConfirmationNewUserEmailBody;// string.Format(Desktop.ConfirmationNewUserEmailBody, urlConfirmationNewUser);

            
            var subject = Desktop.ConfirmationNewUserSubject;
            var adviceToReply = string.Format(Desktop.AdviceToReply, addressToReply);

            var dinamicTemplateKey = MailTemplates.ConfirmationNewUser + userCulture + Guid.NewGuid();
            var model = new
            {
                Email = email,
                FullName = userFullName,
                BodyContent = defaultBodyContent,
                AdviceToReply = adviceToReply,
                Password = password,
                ActivationUrl = urlConfirmationNewUser,
            };

            return GenerateEmail(template, applicationBasePath, subject, defaultBodyContent, toAddress, dinamicTemplateKey , fromAddress, model);

        }




        public static MailMessage GetForgotEmailFromTemplate(string fromAddress, string toAddress, string userName, string userFullName, 
            string userCulture, string applicationBasePath,  string addressToReply, string urlActivationPassword)
        {
            var templatePath = GetTemplatePath();
            var template = GetTemplate(userCulture, templatePath, MailTemplates.ForgotPassword);
           
            var adviceToReply = string.Format(Desktop.AdviceToReply, addressToReply);
            var bodyContent = Desktop.ForgotPasswordDefaultBodyMail;
            var subject = Desktop.ForgotPasswordSubject;

            var model = new
            {
                UserName = userName,
                FullName = userFullName,
                BodyContent = bodyContent,
                AdviceToReply = adviceToReply,
                resetPasswordUrl= urlActivationPassword
            };
            

            var dinamicTemplateKey = MailTemplates.ForgotPassword + userCulture + Guid.NewGuid();
            return GenerateEmail(template, applicationBasePath, subject, bodyContent, toAddress, dinamicTemplateKey, fromAddress, model);
        }

        private static MailMessage GenerateEmail(string template, string applicationBasePath, string subject, string bodyContent, string toAddress, string templateKey, string addressFrom, object model)
        {
            
            var bodyToShow = bodyContent;
            if (!string.IsNullOrEmpty(template))
            {
                template = template.Replace("~/", applicationBasePath);
                bodyToShow = EmailTemplateResolver.GetEmailBody(template, templateKey, model);
            }

            var mail = new MailMessage(addressFrom, toAddress);
            mail.Subject = subject;
            mail.Body = bodyToShow;


            return mail;
        }


        //public static MailMessage FormatMailBodyForHtmlSending(MailMessage mailMessage, string applicationName)
        //{
        //    var templatePath = ConfigurationManager.AppSettings["AssessmentEmailTemplatePath"];
        //    //replace seperator image
        //    var bodyString = mailMessage.Body;
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(bodyString);

        //    var imageNode = doc.DocumentNode.SelectSingleNode("//div[@id='divImgSeperator']");
        //    if (imageNode != null) imageNode.InnerHtml = "<img alt='Seperator Image' class='pull-left' src='cid:imgSeperator'/>";
        //    string result = null;
        //    using (var writer = new StringWriter())
        //    {
        //        doc.Save(writer);
        //        result = writer.ToString();
        //    }
        //    var css = File.ReadAllText(BasePath + "/Content/bootstrap.min.css");
        //    var body = "<html>"
        //                            + $"<head><style>{css}</style></head>"
        //                            + GetMailCss()
        //                            + "<body>"
        //                            + result
        //                            + "</body>"
        //                            + "</html>";

        //    var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
        //    try
        //    {
        //        var imagelink = new LinkedResource($@"{BasePath}{templatePath}\img\{applicationName}\CustomerEmailSeparator.png", "image/png")
        //        {
        //            ContentId = "imgSeperator",
        //            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        //        };
        //        htmlView.LinkedResources.Add(imagelink);
        //    }
        //    catch (DirectoryNotFoundException)
        //    {
        //      //No image added        
        //    }

        //    mailMessage.AlternateViews.Add(htmlView);

        //    return mailMessage;
        //}

    }
}