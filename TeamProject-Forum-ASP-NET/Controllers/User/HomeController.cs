﻿using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.ViewModels;

namespace TeamProject_Forum_ASP_NET.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            using (var db = new ForumDBContext())
            {
                var questions = db.Questions
                    .Include(q => q.Answers)
                    .Include(q => q.Tags)
                    .Include(q => q.Author)
                    .OrderBy(q => q.DateAdded)
                    .ToList();

                return View(questions.ToPagedList(page ?? 1, 3));
            }
        }

        public ActionResult ListCategories(int? categoryId)
        {
            using (var db = new ForumDBContext())
            {
                var categories = db.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                ViewBag.categoryId = categoryId;

                return PartialView("ListCategories", categories);
            }
        }

        [HttpGet]
        public ActionResult ListQuestionsByCategory(int? categoryId, int? page)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ForumDBContext())
            {
                var questions = db.Questions
                    .Where(q => q.CategoryId == categoryId)
                    .Include(q => q.Answers)
                    .Include(q => q.Tags)
                    .Include(q => q.Author)
                    .OrderBy(q => q.DateAdded)
                    .ToList()
                    .ToPagedList(page ?? 1, 3);

                if (questions == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.categoryId = categoryId;

                return View(questions);
            }
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult RankList(int? page)
        {
            using (var db = new ForumDBContext())
            {
                var users = db.Users
                    .OrderByDescending(u => u.PostsCount)
                    .ThenBy(u => u.UserName)
                    .ToList();

                //foreach (var user in users)
                //{
                //    var photoPath = Url.Content("~/Content/Images/ProfilePhotos/" + user.UserName + ".png") + "?time=" + DateTime.Now.ToString();
                //    string fullPhotoPathUser = Request.MapPath("~/Content/Images/ProfilePhotos/" + user.UserName + ".png");

                //    var defaultPhotoPath = Url.Content("~/Content/Images/ProfilePhotos/NoPhoto.png");
                //    user.ProfilePhotoPath = System.IO.File.Exists(fullPhotoPathUser) ? photoPath : defaultPhotoPath;

                //}

                return View(users.ToPagedList(page ?? 1, 3));
            }
        }
    }
}