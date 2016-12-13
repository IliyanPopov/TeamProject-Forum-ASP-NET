using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.ViewModels;

namespace TeamProject_Forum_ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Question");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult RankList()
        {
            using (var db = new ForumDBContext())
            {
                var modelList = new List<RankUserViewModel>();
                var users = db.Users.OrderByDescending(u => u.PostsCount).ToList();

                foreach (var user in users)
                {
                    var model = new RankUserViewModel
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName,
                        PostsCount = user.PostsCount,
                    };

                    modelList.Add(model);
                }

                return View(modelList);
            }
        }
    }
}