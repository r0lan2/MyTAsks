using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;

namespace MyTasks.Web.Controllers
{
    public class ProjectController : BaseController
    {
        private ProjectUnitOfWork unitOfWork = new ProjectUnitOfWork();
        private UserUnitOfWork userUnitOfWork = new UserUnitOfWork();


        // GET: Project
        public ActionResult Index(int? selectCustomerId)
        {
            var projects = unitOfWork.GetProjectsByCustomer(selectCustomerId);
            return View(projects);
        }
        

        public ActionResult Create()
        {
            PopulateCustomersDropDownList();
            PopulateProjectAdministratorsDropDownList();
            return View();
        }

        private void PopulateCustomersDropDownList(object selectCustomerId = null)
        {
            var customerQuery = unitOfWork.CustomerRepository.Select().OrderBy(c => c.Name);
            ViewBag.CustomerList = new SelectList(customerQuery, "CustomerId", "Name", selectCustomerId);
        }
        
        private void PopulateProjectAdministratorsDropDownList(object selectProjectManagerId = null)
        {
            var projectManagers = userUnitOfWork.GetProjectManagers().OrderBy(c => c.Id);
            ViewBag.ProjectManagerList = new SelectList(projectManagers, "Id", "UserName", selectProjectManagerId);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(Project proj)
        {
            Data.Validators.ValidationStatus status = null;
            try
            {
                    status  = unitOfWork.Validate(proj);
                    if (status.IsValid)
                    {
                        unitOfWork.AddProject(proj);
                        unitOfWork.SaveChanges();
                    }
                    return new JsonResult { Data = new { status = status } };
            }
            catch (Exception ex /* dex */)
            {
                return new JsonResult { Data = new { status = new Data.Validators.ValidationStatus { IsValid = false,ErrorMessage = Localization.Desktop.Desktop.UnableToSaveChanges }  } };
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project proj = unitOfWork.GetProjectAndAreas(id.Value);
            if (proj == null)
            {
                return HttpNotFound();
            }
            PopulateCustomersDropDownList(proj.CustomerId);
            PopulateProjectAdministratorsDropDownList(proj.ProjectManagerId);
            return View(proj);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectToUpdate = unitOfWork.ProjectRepository.FindById(id.Value);
            if (TryUpdateModel(projectToUpdate, "",
               new string[] { "ProjectName", "Description", "CustomerId", "ProjectManagerId" }))
            {
                try
                {
                    var status = unitOfWork.Validate(projectToUpdate);
                    if (status.IsValid)
                    {
                        unitOfWork.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", status.ErrorMessage);
                    }
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", Localization.Desktop.Desktop.UnableToSaveChanges);
                }
            }
            PopulateCustomersDropDownList(projectToUpdate.CustomerId);
            return View(projectToUpdate);
        }


        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project course = unitOfWork.GetProject(id.Value);

            if (course.IsUsed)
            {
                var projects = unitOfWork.GetProjectsByCustomer(null);
                return View("Index",  projects);
            }

            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           unitOfWork.DeleteProject(id);
           return RedirectToAction("Index");
        }
        

    }
}