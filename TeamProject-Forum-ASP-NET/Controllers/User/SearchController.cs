using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.ViewModels;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        public ActionResult Search()
        {
            var model = new SearchViewModel();

            return View(model);
        }

        [HttpGet]
        public ActionResult SearchResult(SearchViewModel model)
        {

            if (ModelState.IsValid)
            {
                using (var db = new ForumDBContext())
                {
                    var words = model.Query
                        .ToLower()
                        .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                    var questions = new List<Question>();

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
                                .Where(q => q.Title.Contains(word) || q.Content.Contains(word) || q.Tags.Any(t => t.Name == word)));
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
                                .Where(q => q.Title.Contains(word)));
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
                                .Where(q => q.Content.Contains(word)));
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
                                .Where(q => q.Tags.Any(t => t.Name == word)));
                        }
                    }

                    questions = questions.Distinct().ToList();

                    return View(questions);
                }
            }

            return RedirectToAction("Search");
        }
    }
}