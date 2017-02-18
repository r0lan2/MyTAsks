using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyTasks.Infrastructure.Security;
using MyTasks.Localization.Desktop;

namespace MyTasks.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "EmailAddressNotValid")]
        public string Email { get; set; }


        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "UserNameIsRequired")]
      
        public string UserName { get; set; }
        
        public string Password { get; set; }

      
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "FullNameIsRequired")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Rut")]
        public string Dni { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "RoleIdIsRequired")]
        public string RoleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "LanguageIsRequired")]
        public string Language { get; set; }

        public string GenerateTemporalPassword()
        {
            return PasswordGenerator.Generate(8,8);
        }


    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "EmailAddressNotValid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "EmailAddressNotValid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
