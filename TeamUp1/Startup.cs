using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamUp1.Startup))]
namespace TeamUp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
