using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Models;
using TeamProject_Forum_ASP_NET.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace TeamProject_Forum_ASP_NET.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(string searchString, int? page)
        {
            var users = db.Users
                .ToList();

            var admins = GetAdminUserNames(users, db);
            ViewBag.Admins = admins;

            if (searchString == null)
            {
                return View(users.ToPagedList(page ?? 1, 3));
            }

            searchString = searchString.ToLower();
            users = users.Where(u => u.UserName.ToLower().Contains(searchString)).ToList();

            return View(users.ToPagedList(page ?? 1, 3));
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ForumDBContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    admins.Add(user.UserName);
                }
            }
            return admins;
        }

        public ActionResult Edit(string id)
        {
            //validate id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get username from db
            var user = db.Users
                .First(u => u.Id == id);

            //check if user exists
            if (user == null)
            {
                return HttpNotFound();
            }

            //create a viewmodel and fill it 
            var viewModel = new EditUserViewModel();
            viewModel.User = user;
            viewModel.Roles = GetUserRoles(user, db);


            //pass the model to the view
            return View(viewModel);
        }

        private IList<Role> GetUserRoles(ApplicationUser user, ForumDBContext db)
        {
            //create user manager
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            //get all application roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            //foreach app role, check if the user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.IsSelected = true;
                }
                userRoles.Add(role);
            }

            //return a list with all roles
            return userRoles;
        }

        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                //get user from db
                var user = db.Users.FirstOrDefault(u => u.Id == id);

                //check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                //if password field is not empty, change pass
                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    var hasher = new PasswordHasher();
                    var passwordHash = hasher.HashPassword(viewModel.Password);
                    user.PasswordHash = passwordHash;
                }

                //set user properties
                user.Email = viewModel.User.Email;
                user.FullName = viewModel.User.FullName;
                user.UserName = viewModel.User.UserName;
                this.SetUserRoles(viewModel, user, db);

                //save changes
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List");
            }
            return View();
        }

        private void SetUserRoles(EditUserViewModel viewModel, ApplicationUser user, ForumDBContext db)
        {
            var userManager = HttpContext
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in viewModel.Roles)
            {
                if (role.IsSelected && !userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected && userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get user from db
            var user = db.Users
                .First(u => u.Id == id);

            //check if user exists
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get default admin from db
            var admin = db.Users.First(u => u.Email == "admin@admin.com");

            //get user from db
            var user = db.Users
                .First(u => u.Id.Equals(id));

            //get user questions and answers from db
            var userQuestions = db.Questions
                .Where(q => q.Author.Id == user.Id);

            var userAnswers = db.Answers
                .Where(a => a.Author.Id == user.Id);

            //make user questions and answers to point to the default admin
            foreach (var question in userQuestions)
            {
                question.AuthorId = admin.Id;
                admin.PostsCount++;
            }

            foreach (var answer in userAnswers)
            {
                answer.AuthorId = admin.Id;
                admin.PostsCount++;
            }

            //delete the profile photo
            string fullPath = Request.MapPath("~/Images/ProfilePhotos/" + user.UserName + ".png");
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            //delete user and save changes
            db.Entry(admin).State = EntityState.Modified;
            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("List");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}