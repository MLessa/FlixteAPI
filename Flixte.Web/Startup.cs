using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flixte.Web.Startup))]
namespace Flixte.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
