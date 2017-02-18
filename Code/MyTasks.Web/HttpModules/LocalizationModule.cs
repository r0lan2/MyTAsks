using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web;
using MyTasks.Web.Infrastructure.Localization;

namespace MyTasks.Web.HttpModules
{
    public class LocalizationModule : IHttpModule
    {
        /// <summary>
        /// Sets the current thread culture.
        /// </summary>
        /// <param name="cultureName">Name of the culture as it is described in the documentation of CultureInfo.</param>
        protected static void SetCulture(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += context_PostAcquireRequestState;
        }

        private void context_PostAcquireRequestState(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;
            SetCulture(new HttpContextWrapper(HttpContext.Current).GetCurrentCulture());
        }

        public void Dispose()
        {
           
        }
      
    }


}