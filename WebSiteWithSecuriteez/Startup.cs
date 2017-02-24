using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSiteWithSecuriteez.Startup))]
namespace WebSiteWithSecuriteez
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
