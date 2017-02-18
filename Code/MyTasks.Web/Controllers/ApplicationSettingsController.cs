using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using MyTasks.Web.Infrastructure.Configuration;

namespace MyTasks.Web.Controllers
{
    public class ApplicationSettingsController : BaseController
    {

        private SetupUnitOfWork unitOfWork = new SetupUnitOfWork();
        // GET: ApplicationSettings
        public ActionResult Index()
        {
            var settings = unitOfWork.GetSettings();
            return View(settings);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSettings appSeting = unitOfWork.ApplicationSettingsRepository.FindById(Id.Value);
            if (appSeting == null)
            {
                return HttpNotFound();
            }
            return  View(appSeting);

        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppSetting([Bind(Include = "SettingId,ConfigurationKey,ConfigurationValue,DefaultValue,Description")]ApplicationSettings setting)
        {
            if (setting == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appSettingToUpdate = unitOfWork.ApplicationSettingsRepository.FindById(setting.SettingId);
                try
                {
                    unitOfWork.ApplicationSettingsRepository.UpdateSetting(setting.ConfigurationKey,setting.ConfigurationValue);
                    unitOfWork.SaveChanges();
                    MvcApplication.ApplicationSettings = ApplicationSettingContainer.Create(unitOfWork.ApplicationSettingsRepository.All().ToList());
                    return RedirectToAction("Index");
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            
            return View(appSettingToUpdate);
        }



    }
}