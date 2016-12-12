using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.ViewModels;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class AnswerController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        [HttpGet]
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new AnswerViewModel();
            var question = db.Questions.FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            model.QuestionId = question.Id;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(AnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorId = db.Users
                   .First(u => u.UserName == this.User.Identity.Name).Id;

                var answer = new Answer(authorId, model.Content, model.QuestionId);

                db.Answers.Add(answer);
                db.SaveChanges();

                return RedirectToAction("ViewAnswers", "Question", new { id = model.QuestionId });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new AnswerViewModel();
            var answer = db.Answers.FirstOrDefault(q => q.Id == id);

            if (answer == null)
            {
                return HttpNotFound();
            }

            model.Id = answer.Id;
            model.Content = answer.Content;
            model.QuestionId = answer.QuestionId;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = db.Answers.FirstOrDefault(a => a.Id == model.Id);

                answer.Content = model.Content;

                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ViewAnswers", "Question", new { id = model.QuestionId });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new AnswerViewModel();
            var answer = db.Answers.FirstOrDefault(q => q.Id == id);

            if (answer == null)
            {
                return HttpNotFound();
            }

            model.Id = answer.Id;
            model.Content = answer.Content;
            model.QuestionId = answer.QuestionId;

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(AnswerViewModel model)
        {

            var answer = db.Answers.FirstOrDefault(a => a.Id == model.Id);

            db.Answers.Remove(answer);
            db.SaveChanges();

            return RedirectToAction("ViewAnswers", "Question", new { id = model.QuestionId });
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