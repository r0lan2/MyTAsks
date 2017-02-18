using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MyTasks.Web.Security.Web
{
    public class AllowAnonymousOnLocalHost : AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return IsLocalHost(actionContext);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsLocalHost(actionContext))
            {
                return;
            }
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }

        private static bool IsLocalHost(HttpActionContext actionContext)
        {
            return actionContext.Request.RequestUri.Host.ToLower().Contains("localhost");
        }
    }
   
}