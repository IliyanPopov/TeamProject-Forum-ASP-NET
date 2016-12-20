using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Search(SearchViewModel model, int? page)
        {
            var questions = new List<Question>();

            if (model.Query == null)
            {
                model.Questions = new List<Question>().ToPagedList(page ?? 1, 3);

                return View(model);
            }

            using (var db = new ForumDBContext())
            {
                var words = model.Query
                    .ToLower()
                    .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (model.Title.Equals(false) &&
                    model.Content.Equals(false) &&
                    model.Tag.Equals(false))
                {
                    foreach (var word in words)
                    {
                        questions.AddRange(db.Questions
                            .Include(q => q.Author)
                            .Include(q => q.Answers)
                            .Include(q => q.Tags)
                            .Where(q => q.Title
                            .ToLower().Contains(word) ||
                            q.Content.ToLower().Contains(word) ||
                            q.Tags.Any(t => t.Name.ToLower() == word)));
                    }
                }

                if (model.Title.Equals(true))
                {
                    foreach (var word in words)
                    {
                        questions.AddRange(db.Questions
                            .Include(q => q.Author)
                            .Include(q => q.Answers)
                            .Include(q => q.Tags)
                            .Where(q => q.Title.ToLower().Contains(word)));
                    }
                }

                if (model.Content.Equals(true))
                {
                    foreach (var word in words)
                    {
                        questions.AddRange(db.Questions
                            .Include(q => q.Author)
                            .Include(q => q.Answers)
                            .Include(q => q.Tags)
                            .Where(q => q.Content.ToLower().Contains(word)));
                    }
                }

                if (model.Tag.Equals(true))
                {
                    foreach (var word in words)
                    {
                        questions.AddRange(db.Questions
                            .Include(q => q.Author)
                            .Include(q => q.Answers)
                            .Include(q => q.Tags)
                            .Where(q => q.Tags.Any(t => t.Name.ToLower() == word)));
                    }
                }

                //add photo to questions

                foreach (var question in questions)
                {
                    var questionAuthorPhotoPath = Url.Content("~/Content/Images/ProfilePhotos/" + question.Author.UserName + ".png") + "?time=" + DateTime.Now.ToString();
                    string fullQuestionAuthorPhotoPath = Request.MapPath("~/Content/Images/ProfilePhotos/" + question.Author.UserName + ".png");
                    var defaultPhotoPathQuestion = Url.Content("~/Content/Images/ProfilePhotos/NoPhoto.png");

                    question.AuthorPhotoPath = System.IO.File.Exists(fullQuestionAuthorPhotoPath) ? questionAuthorPhotoPath : defaultPhotoPathQuestion;
                }

                questions = questions.Distinct().ToList();
                model.Questions = questions.ToPagedList(page ?? 1, 3);

                return View(model);
            }

        }
    }
}