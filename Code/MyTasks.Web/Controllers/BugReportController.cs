using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Elmah;
using MyTasks.Data.Repositories;
using MyTasks.Infrastructure.Exceptions;
using MyTasks.Web.Models;
using MyTasks.Web.Infrastructure;

namespace MyTasks.Web.Controllers
{
    public class BugReportController : Controller
    {

        
        // GET: BugReport
        public ActionResult ShowBugReport()
        {
            return View();
        }


        public ActionResult SucessReportedBug(bool isSuccess)
        {
            ViewBag.isSuccess = isSuccess;
            return View();
        }


        public ActionResult SubmitBug(BugReportInfo bugInfo)
        {
            var issuesMailTo = MvcApplication.ApplicationSettings.IssuesRecipientEmail;
            string result;
            var submitBugIsOk = true;
            try
            {
                var entry = GetLatestErrorLogEntry();
                if (entry != null)
                {
                    var extra = GetFormattedExtraInformation(entry, bugInfo.UserMessage);
                    var email = string.IsNullOrEmpty(bugInfo.UserMail) ? "no email specified" : bugInfo.UserMail;

                    var exceptionHandler = new ExceptionHandler()
                    {
                        ErrorDetails = extra,
                        FromEmail = email,
                        IssuesMailTo = issuesMailTo
                    };
                    
                    result = exceptionHandler.SubmitBug();
                }
                else
                {
                    ErrorReportingFailed(new Exception("Could not find log entry for bug submission."));
                    result = ExceptionHandler.BugCouldNotSubmitMessage;
                }
            }
            catch (Exception e)
            {
                ErrorReportingFailed(e);
                result = ExceptionHandler.BugCouldNotSubmitMessage;
            }

            if (result == ExceptionHandler.BugCouldNotSubmitMessage)
                submitBugIsOk = false;

            return RedirectToAction("SucessReportedBug", "BugReport", new { isSuccess = submitBugIsOk });


        }

        private void ErrorReportingFailed(Exception e)
        {
            ErrorSignal signal = ErrorSignal.FromCurrentContext();
            signal.Raise(e);
        }

        private ErrorLogEntry GetLatestErrorLogEntry()
        {
            var errors = new List<ErrorLogEntry>();
            ErrorLog.GetDefault(System.Web.HttpContext.Current).GetErrors(0, 1, errors);
            return errors.Count > 0 ? errors.FirstOrDefault() : null;
        }

        //private string GetFormattedErrorDescription(Error currentError)
        //{
        //    //throw new Exception("Could not get error description.");
        //    string pageName = string.Empty;
        //    if (Request.UrlReferrer != null)
        //        pageName = System.IO.Path.GetFileName(Request.UrlReferrer.Query);
        //    return pageName + ": " + currentError.Type + ": " + currentError.Message;
        //}

        private string GetFormattedExtraInformation(ErrorLogEntry entry, string userMessage)
        {
            var errorLogRelativeUrl = Url.Content(String.Format("~/elmah/detail?id={0}", entry.Id));
            var extra = Server.HtmlEncode(GetExtraInformation(userMessage));
            var protocol = Request.IsSecureConnection ? "https://" : "http://";
            extra += Environment.NewLine;
            extra += String.Format("Url to errorlog: {0}{1}{2} .", protocol, Request.Url.Host, errorLogRelativeUrl);
            extra += Environment.NewLine;
            extra += "In web app: Admin BigLamp -> Error Log";

            return extra;

        }

        //TODO:Add extra info about selected object 
        public static string GetExtraInformation(string userMessage)
        {
            string strExtraString = Environment.NewLine;
            strExtraString += ("Error at " + DateTime.Now) + Environment.NewLine;
            strExtraString += "App: " + MyTasks.Web.Infrastructure.Configuration.ApplicationInfo.BuildVersion();
            strExtraString += " , Db:" + MyTasks.Web.Infrastructure.Configuration.ApplicationInfo.DbVersion() + Environment.NewLine + Environment.NewLine;

            strExtraString += "Error description: " + Environment.NewLine;
            strExtraString += userMessage + Environment.NewLine;

            return strExtraString;
        }




    }
}