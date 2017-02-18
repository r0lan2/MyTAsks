using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;

namespace MyTasks.Web.Controllers
{
    public class CustomerController : Controller
    {
        private LookupUnitOfWork unitOfWork = new LookupUnitOfWork();

        // GET: Customer
        public ActionResult Index()
        {
            var customers = unitOfWork.GetCustomers();
            return View(customers);
        }
        
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,MainContactName,MainContactEmail")]Customer newCustomer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var status = unitOfWork.Validate(newCustomer);
                    if (status.IsValid)
                    {
                        unitOfWork.AddCustomer(newCustomer);
                        unitOfWork.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", status.ErrorMessage);
                    }

                }
                else
                {
                    return View("Create", newCustomer);
                }
            }
            catch (Exception ex /* dex */)
            {
                ModelState.AddModelError("", Localization.Desktop.Desktop.UnableToSaveChanges);
            }
          
            return View(newCustomer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = unitOfWork.GetCustomer(id.Value);
        
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var customerToUpdate = unitOfWork.CustomerRepository.FindById(id.Value);
            if (TryUpdateModel(customerToUpdate, "",
               new string[] { "Name", "MainContactName","MainContactEmail" }))
            {
                try
                {
                    var status = unitOfWork.Validate(customerToUpdate);
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
            return View(customerToUpdate);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = unitOfWork.GetCustomer(id.Value);
            if (customer.IsUsed)
            {
                var customers = unitOfWork.GetCustomers();
                return View("index", customers);
            }
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = unitOfWork.GetCustomer(id);
            if (unitOfWork.CanDeleteCustomer(customer))
            {
                unitOfWork.CustomerRepository.Delete(customer);
                unitOfWork.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", Localization.Desktop.Desktop.CustomerIsAlreadyUsed);
            }

            return RedirectToAction("Index");
        }
    }
}