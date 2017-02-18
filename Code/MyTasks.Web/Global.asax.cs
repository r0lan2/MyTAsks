using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using MyTasks.Web.Infrastructure.Configuration;

namespace MyTasks.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private SetupUnitOfWork unitOfWork = new SetupUnitOfWork();
        public static ApplicationSettingContainer ApplicationSettings { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FluentValidationModelValidatorProvider.Configure();
            SetApplicationSettings();
        }


        private void SetApplicationSettings()
        {
            try
            {
                ApplicationSettings = ApplicationSettingContainer.Create(unitOfWork.ApplicationSettingsRepository.All().ToList());
            }
            catch (Exception e)
            {
                //do nothing because database is not created yet
            }

        }
    }
}
