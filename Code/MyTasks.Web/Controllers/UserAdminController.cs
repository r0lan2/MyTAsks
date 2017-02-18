using MyTasks.Web.Models;
using MyTasks.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BigLamp.AspNet.Identity.MySQL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyTasks.Infrastructure.Mail;
using MyTasks.Web.Extensions;
using MyTasks.Web.Infrastructure.Localization;

namespace MyTasks.Web.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    public class UsersAdminController : BaseController
    {
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToList().AsyncToList());
        }

       
        //
        // GET: /Users/Create
        public ActionResult Create()
        {
            PopulateRoles();
            PopulateLanguages();
            return View();
        }

        public void PopulateRoles(object selectedRole = null)
        {
            var roles = RoleManager.Roles.ToList();
            ViewBag.RoleList = new SelectList(roles, "Name", "Name", selectedRole);
        }

        public void PopulateLanguages(object selectedLanguage = null)
        {
            var cultures = CultureHelper.AvailableCultures();
            ViewBag.LanguageList = new SelectList(cultures, "Culture", "NativeName", selectedLanguage);
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            var generatedPassword = userViewModel.GenerateTemporalPassword();
            userViewModel.Password = generatedPassword;
            userViewModel.ConfirmPassword = generatedPassword;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.UserName, Email = userViewModel.Email, FullName = userViewModel.FullName, Address = userViewModel.Address, Dni = userViewModel.Dni, Language = userViewModel.Language};
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(userViewModel.RoleId))
                    {
                        var result = await UserManager.AddToRoleAsync(user.Id, userViewModel.RoleId);//  AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList( RoleManager.Roles, "Name", "Name");
                            return View();
                        }
                    }

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailWithCredentialsAsync(user.Id, TypeOfMails.NewUserMail, callbackUrl, generatedPassword); 

                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    PopulateLanguages();
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles =  UserManager.GetRoles(user.Id);
            var roleId = userRoles.First();

            PopulateRoles(roleId);
            PopulateLanguages(user.Language);
            var editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                Dni = user.Dni,
                RoleId = roleId,
                Language = user.Language,
                UserName = user.UserName
            };
           
            return View(editUserViewModel);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,FullName,Address,Dni,CostPerDay,RoleId,Language")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Dni = editUser.Dni;
                user.FullName = editUser.FullName;
                user.Address = editUser.Address;
                user.Language = editUser.Language;

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                var selectedRoles = new string[] { editUser.RoleId };

                
                var resultUpdateUserRole = await UserManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray<string>());
                var resultUpdateUser= await UserManager.UpdateAsync(user);

                if (!resultUpdateUserRole.Succeeded || !resultUpdateUser.Succeeded)
                {
                    ModelState.AddModelError("", resultUpdateUserRole.Errors.First());
                    return View();
                }
               var resultRemoveRoles = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray<string>());

               if (!resultRemoveRoles.Succeeded)
                {
                    ModelState.AddModelError("", resultRemoveRoles.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            if (user.Id == CurrentUserId()) return View("Index", await UserManager.Users.ToList().AsyncToList());

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        public async Task<ActionResult> SendPassword(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var forgotPasswordViewModel = new ForgotPasswordViewModel() { Email = user.Email };
            return View(forgotPasswordViewModel);
        }

        public  ActionResult SendPasswordConfirmation()
        {
            return View();
        }





        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
              
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, TypeOfMails.ForgotPassword, callbackUrl);
                return RedirectToAction("SendPasswordConfirmation", "UsersAdmin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
