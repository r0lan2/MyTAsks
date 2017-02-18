using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyTasks.Domain.Base;

namespace MyTasks.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string CurrentUserId()
        {
            return User.Identity.GetUserId();
        }

        private string _baseUrl;
        public string BaseUrl
        {
            get
            {
                if (String.IsNullOrEmpty(_baseUrl))
                {
                    var request = System.Web.HttpContext.Current.Request;
                    var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                    _baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
                }
                return _baseUrl;
            }
            set { _baseUrl = value; }
        }



    }
}