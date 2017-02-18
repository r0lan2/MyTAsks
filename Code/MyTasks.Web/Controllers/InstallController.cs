using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BigLamp.DatabaseInstaller;
using BigLamp.DatabaseInstaller.Exceptions;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Localization.Desktop;
using MyTasks.Web.Infrastructure.Configuration;

namespace MyTasks.Web.Controllers
{
    public class InstallController : BaseController
    {
        private SetupUnitOfWork unitOfWork = new SetupUnitOfWork();

        // GET: Install
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ShowUpdateMessage = false;
            IEnumerable<MyTasks.Domain.Dbversion> dbVersionChanges = unitOfWork.DbVersionRepository.All();

            if (TempData["UpdateResult"] != null)
            {
                var result = TempData["UpdateResult"] as UpdateResult;
                ViewBag.ShowUpdateMessage = true;
                ViewBag.IsSucceed = result.IsSucceed;
                ViewBag.ResultMessage = result.ResultMessage;
            }
            return View(dbVersionChanges);
        }

        [AllowAnonymous]
        public ActionResult FirstInstall()
        {
            ViewBag.ShowUpdateMessage = false;
            if (TempData["InstallResult"] != null)
            {
                var result = TempData["InstallResult"] as UpdateResult;
                ViewBag.ShowUpdateMessage = true;
                ViewBag.IsSucceed = result.IsSucceed;
                ViewBag.ResultMessage = result.ResultMessage;
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult InstallDatabase()
        {
            try
            {
                var rootConnectionString = MyTasks.Infrastructure.Configuration.Application.RootConnectionString;
                var connectionString = MyTasks.Infrastructure.Configuration.Application.ConnectionString;

                var databaseIsNotInstalled = !DatabaseInstallerByObject.IsDatabasseInstalled(rootConnectionString,connectionString);

                if (databaseIsNotInstalled)
                {
                    DatabaseInstallerByObject.CreateAndBuildDatabase(rootConnectionString,connectionString);
                    
                    TempData["InstallResult"] = new UpdateResult()
                    {
                        IsSucceed = true,
                        ResultMessage = Desktop.DatabaseInstalled
                    };
                    MvcApplication.ApplicationSettings =
                        ApplicationSettingContainer.Create(unitOfWork.ApplicationSettingsRepository.All().ToList());
                    return RedirectToAction("FirstInstall");
                }
                else
                {
                    TempData["InstallResult"] = new UpdateResult()
                    {
                        IsSucceed = false,
                        ResultMessage = Desktop.DatabaseAlreadyInstalled
                    };
                    return RedirectToAction("FirstInstall");
                }
            }
            catch (DatabaseInstallerException dbException)
            {
                TempData["InstallResult"] = new UpdateResult()
                {
                    IsSucceed = false,
                    ResultMessage = Desktop.DatabaseInstallError + ":" + dbException.Message
                };
                return RedirectToAction("FirstInstall");

            }
        }



        [HttpPost]
        public ActionResult UpdateDatabase()
        {
            try
            {
                var connectionString = MyTasks.Infrastructure.Configuration.Application.ConnectionString;
                DatabaseInstallerByObject.BuildDatabase(DatabaseInstallerByObject.ReadKeyFromPosition.Prefix, connectionString);

                TempData["UpdateResult"] = new UpdateResult()
                {
                    IsSucceed = true,
                    ResultMessage = Desktop.DatabaseUpdated
                };
                MvcApplication.ApplicationSettings = ApplicationSettingContainer.Create(unitOfWork.ApplicationSettingsRepository.All().ToList());

                return RedirectToAction("Index");
                
            }
            catch (DatabaseInstallerException dbException)
            {
                TempData["UpdateResult"] = new UpdateResult()
                {
                    IsSucceed = false,
                    ResultMessage = Desktop.DbUpdateError + ":" + dbException.Message
                };
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(dbException));
                return RedirectToAction("Index");
              
            }
        }
        



    }

    public class UpdateResult
    {
        public bool IsSucceed { get; set; }
        public string ResultMessage { get; set; }

    }



}