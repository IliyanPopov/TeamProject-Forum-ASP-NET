﻿using System;
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
    public class QuestionController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        [HttpGet]
        public ActionResult List()
        {
            var questions = db.Questions
                .Include(a => a.Author)
                .ToList();

            return View(questions);
        }

        [HttpGet]
        public ActionResult ViewAnswers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new QuestionViewModel();
            var question = db.Questions.Include(a => a.Author).FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            var answers = db.Answers.Where(q => q.QuestionId == id).ToList();

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.DateAdded = question.DateAdded;
            model.Author = question.Author;
            model.Answers = answers;

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var model = new QuestionViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorId = db.Users
                    .First(u => u.UserName == this.User.Identity.Name).Id;

                var question = new Question(authorId, model.Title, model.Content);

                db.Questions.Add(question);
                db.SaveChanges();

                return RedirectToAction("List");
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

            var question = db.Questions.FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            var model = new QuestionViewModel();

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = db.Questions
                        .FirstOrDefault(q => q.Id == model.Id);

                question.Title = model.Title;
                question.Content = model.Content;

                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ViewAnswers", model);
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

            var question = db.Questions
                .Where(q => q.Id == id)
                .Include(q => q.Author)
                .FirstOrDefault();

            if (question == null)
            {
                return HttpNotFound();
            }

            var model = new QuestionViewModel();
            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = db.Questions
                    .FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            db.Questions.Remove(question);
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