using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieWebApp.Startup))]
namespace MovieWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
