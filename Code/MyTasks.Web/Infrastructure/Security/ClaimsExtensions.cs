using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using MyTasks.Web.Models;
using MyTasks.Web.Infrastructure.Localization;

namespace MyTasks.Web.Security
{
    public static class ClaimsExtensions
    {
     
        public static class MyTasksClaimTypes
        {
            public const string UserId = "UserId";
            public const string UserFullName =  "UserFullName";
            public const string Address = "Address";
            public const string Dni = "Dni";
            public const string CostPerDay = "CostPerDay";
            public const string PictureFileName = "PictureFileName";
        }
        public static ClaimsIdentity AsClaimsIdentity(this IIdentity identity)
        {
            return identity as ClaimsIdentity;
        }

        public static void AddClaims(this ClaimsIdentity identity, ApplicationUser user, bool isPersistent, string currentCulture)//, List<string> rolesForUser, )//, List<int> siteIdsUserHasAccesTo)
        {
            identity.AddClaim(new Claim(MyTasksClaimTypes.UserId, user.Id));
            identity.AddClaim(new Claim(MyTasksClaimTypes.UserFullName, string.IsNullOrEmpty(user.FullName)? string.Empty:user.FullName ));
            identity.AddClaim(new Claim(MyTasksClaimTypes.Address, string.IsNullOrEmpty(user.Address) ? string.Empty : user.Address));
            identity.AddClaim(new Claim(MyTasksClaimTypes.Dni, string.IsNullOrEmpty(user.Dni) ? string.Empty : user.Dni));
            identity.AddClaim(new Claim(ClaimTypes.Email, string.IsNullOrEmpty(user.Email) ? string.Empty : user.Email));
            identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isPersistent.ToString()));
            identity.AddClaim(new Claim(MyTasksClaimTypes.PictureFileName, string.IsNullOrEmpty(user.PictureName) ? string.Empty : user.PictureName));

            var cultureName = user.Language;

            if (cultureName == null && currentCulture != null)
                cultureName = currentCulture;

            var validCulture = CultureHelper.GetCountrySpecificCulture(cultureName);

            identity.AddClaim(new Claim(ClaimTypes.Locality, validCulture));
                 
        }
        
        public static string FullName(this ClaimsIdentity identity)
        {
            var claim = identity.FindFirst(MyTasksClaimTypes.UserFullName);

            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                return claim.Value;
            }
            return identity.Name;
        }
        public static string UserId(this ClaimsIdentity identity)
        {
            var claim = identity.FindFirst(MyTasksClaimTypes.UserId) ?? identity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                return claim.Value;
            }
            return null;
        }
        public static string Email(this ClaimsIdentity identity)
        {
            var claim = identity.FindFirst(ClaimTypes.Email);
            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                return claim.Value;
            }
            return string.Empty;
        }

        public static bool IsPersistent(this ClaimsIdentity identity)
        {
            var claim = identity.FindFirst(ClaimTypes.IsPersistent);
            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                return bool.Parse(claim.Value);
            }
            return false;
        }


        public static string Language(this ClaimsIdentity identity)
        {
            var claim = identity.FindFirst(ClaimTypes.Locality);
            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                return claim.Value;
            }
            return string.Empty;
        }


        public static string PictureFileNameAbsolutePath(this ClaimsIdentity identity)
        {
            var path = "~/" + MvcApplication.ApplicationSettings.ProfilesPicturePath + identity.Email();
            var defaultPicture = "~/Content/images/nobody_m.original.jpg";

            var pictureFileNameClaim = identity.FindFirst(MyTasksClaimTypes.PictureFileName);

            if (pictureFileNameClaim != null && !string.IsNullOrEmpty(pictureFileNameClaim.Value))
            {
                return path + "/" +   pictureFileNameClaim.Value;
            }
            return defaultPicture;
        }
        
    }
}
