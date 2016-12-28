using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<TeamProject_Forum_ASP_NET.Entities.ForumDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            // ContextKey = "TeamProject_Forum_ASP_NET.Entities.ForumDBContext";
        }

        protected override void Seed(TeamProject_Forum_ASP_NET.Entities.ForumDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
            }

            if (!context.Users.Any())
            {
                this.CreateUser(context, "Admin", "admin@admin.com", "Admin", "123");
                this.SetRoleToUser(context, "admin@admin.com", "Admin");
            }

        }

        private void SetRoleToUser(ForumDBContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(
                  new UserStore<ApplicationUser>(context));

            var user = context.Users.First(u => u.Email == email);

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(ForumDBContext context, string userName, string email, string fullName, string password)
        {
            //create user manager
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //set user manager password validator
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            //create user object
            var defaultPhotoPath = HostingEnvironment.MapPath("~/Content/Images/ProfilePhotos/NoPhoto.png");
            var admin = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                ProfilePhotoPath = defaultPhotoPath
            };

            //create user
            var result = userManager.Create(admin, password);

            //validate result
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateRole(ForumDBContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }

        }


    }
}
