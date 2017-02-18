using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MyTasks.Localization.Desktop;

namespace MyTasks.Web.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Rut")]
        public string Dni { get; set; }

        [Display(Name = "Cost per day")]
        public float CostPerDay { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "RoleIdIsRequired")]
        public string RoleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "LanguageIsRequired")]
        public string Language { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}