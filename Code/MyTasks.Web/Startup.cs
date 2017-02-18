using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyTasks.Web.Startup))]
namespace MyTasks.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
