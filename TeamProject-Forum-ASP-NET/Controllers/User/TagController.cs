using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult ListQuestionsByTag(int? id)
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
                    .ToList();

                return View(questions);
            }
        }
    }
}