using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using PagedList;
using PagedList.Mvc;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult ListQuestionsByTag(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (var db = new ForumDBContext())
            {
                var questions = db.Tags
                    .Include(t => t.Questions.Select(q => q.Tags))
                    .Include(t => t.Questions.Select(q => q.Author))
                    .Include(t => t.Questions.Select(q => q.Answers))
                    .FirstOrDefault(t => t.Id == id)
                    .Questions
                    .OrderByDescending(q => q.DateAdded)
                    .ToList();

                //add photo to questions
                foreach (var question in questions)
                {
                    var questionAuthorPhotoPath = Url.Content("~/Content/Images/ProfilePhotos/" + question.Author.UserName + ".png") + "?time=" + DateTime.Now.ToString();
                    string fullQuestionAuthorPhotoPath = Request.MapPath("~/Content/Images/ProfilePhotos/" + question.Author.UserName + ".png");
                    var defaultPhotoPathQuestion = Url.Content("~/Content/Images/ProfilePhotos/NoPhoto.png");

                    question.AuthorPhotoPath = System.IO.File.Exists(fullQuestionAuthorPhotoPath) ? questionAuthorPhotoPath : defaultPhotoPathQuestion;
                }

                return View(questions.ToPagedList(page ?? 1, 3));
            }
        }
    }
}