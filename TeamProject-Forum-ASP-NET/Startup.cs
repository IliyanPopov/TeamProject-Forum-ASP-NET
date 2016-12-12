
using System.Data.Entity;
using Microsoft.Owin;
using Owin;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Migrations;

[assembly: OwinStartupAttribute(typeof(TeamProject_Forum_ASP_NET.Startup))]
namespace TeamProject_Forum_ASP_NET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ForumDBContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
