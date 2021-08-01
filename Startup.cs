using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebProject_NetFramework.Startup))]
namespace WebProject_NetFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
