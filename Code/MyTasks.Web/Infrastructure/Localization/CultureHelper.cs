using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTasks.Web.Security;

namespace MyTasks.Web.Infrastructure.Localization
{
    public static class CultureHelper
    {
        private const string CultureCookieName = ".AquaVetCultureCookie";

        public const string FallbackLanguageCode = "en-us";
        public static readonly Dictionary<String, string> Cultures = new Dictionary<string, string>
                                                                                  {
                                                                                      {"en-us", "en-GB"},
                                                                                      {"es-cl", "es-CL"}
                                                                                  };

        private static readonly Dictionary<String, String> NeutralToSpecificCulturesMapping = new Dictionary<string, string>
                                                                                  {
                                                                                      {"es", "es-CL"}
                                                                                  };

        public static List<AvailableCulture> AvailableCultures()
        {
             return
                CultureHelper.Cultures.Keys.Select(
                    t =>
                        new AvailableCulture
                        {
                            Culture = t,
                            NativeName=System.Globalization.CultureInfo.GetCultureInfo(Convert.ToString(t)).NativeName
                        }).ToList();
        }



        public static string GetValidCulture(string name)
        {
            name = name.ToLower();
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            if (Cultures.ContainsKey(name))
                return Cultures[name];

            if (NeutralToSpecificCulturesMapping.ContainsKey(name))
                return NeutralToSpecificCulturesMapping[name];

            // else             
            return GetDefaultCulture(); // return Default culture as no match found
        }

        public static string GetCountrySpecificCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            if (Cultures.ContainsKey(name))
                return name;

            if (NeutralToSpecificCulturesMapping.ContainsKey(name))
                return NeutralToSpecificCulturesMapping[name];

            // else             
            return GetDefaultCulture(); // return Default culture as no match found
        }


        /// <summary>
        /// Default will be en-GB
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return Cultures[Cultures.Keys.First()]; // return Default culture
        }

        /// <summary>
        /// First get culture from cookie, then user profile and finally try browser.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCurrentCulture(this HttpContextBase context)
        {
            // invoke the localization logic
            var cultureName = GetDefaultCulture();


            if ( context.User != null && context.User.Identity.IsAuthenticated && 
                !String.IsNullOrEmpty(context.User.Identity.AsClaimsIdentity().Language()))
            {
                return context.User.Identity.AsClaimsIdentity().Language();
            }

            // Attempt to read the culture cookie from Request
            var cultureCookie = context.Request.Cookies[CultureCookieName];

            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
                return cultureName;
            }
            if (context.Request.UserLanguages != null)
                cultureName = context.Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

            var validCulture = GetCountrySpecificCulture(cultureName);

            SetLanguageCookie(context, validCulture);

            return validCulture; // This is safe

        }

        public static void SetLanguageCookie(HttpContextBase context, string culture)
        {
            // Save culture in a cookie
            var cookie = context.Request.Cookies[CultureCookieName];

            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie(CultureCookieName, culture)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            context.Response.Cookies.Add(cookie);

        }





    }


    public class AvailableCulture
    {
        public string Culture { set; get; }
        public string NativeName { set; get; }
    }

}
