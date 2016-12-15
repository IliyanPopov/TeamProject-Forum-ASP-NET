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
        //
        // GET: Search
        [HttpGet]
        public ActionResult Search()
        {
            var model = new SearchViewModel();

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult SearchResult(SearchViewModel model, int? page)
        {
            var questions = new List<Question>();

            if (ModelState.IsValid)
            {
                using (var db = new ForumDBContext())
                {
                    if (model.Query == null)
                    {
                        return View(questions.ToPagedList(page??1,3));
                    }

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

                    questions = questions.Distinct().ToList();

                    return View(questions.ToPagedList(page ?? 1, 3));
                }
            }

            return View(questions.ToPagedList(page ?? 1, 3));
        }
    }
}