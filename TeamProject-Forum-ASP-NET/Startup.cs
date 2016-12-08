using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamProject_Forum_ASP_NET.Startup))]
namespace TeamProject_Forum_ASP_NET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
