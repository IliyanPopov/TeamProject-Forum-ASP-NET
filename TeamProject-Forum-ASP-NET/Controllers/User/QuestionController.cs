﻿using PagedList;
using PagedList.Mvc;
using System;
using System.Collections;
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
        public ActionResult ViewAnswers(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new QuestionViewModel();
            var question = db.Questions
                .Include(a => a.Author)
                .FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            var answers = db.Answers
                .Where(q => q.QuestionId == id)
                .Include(a => a.Author)
                .OrderBy(a => a.DateAdded)
                .ToList()
                .ToPagedList(page ?? 1, 3);

            question.ViewCount++;

            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.ViewCount = question.ViewCount;
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
            model.Categories = db.Categories.OrderBy(c => c.Name).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var author = db.Users
                    .First(u => u.UserName == this.User.Identity.Name);
                var authorId = author.Id;

                var question = new Question(authorId, model.Title, model.Content, model.CategoryId);
                author.PostsCount++;

                this.SetQuestionTags(question, model, db);

                db.Entry(author).State = EntityState.Modified;
                db.Questions.Add(question);
                db.SaveChanges();

                return RedirectToAction("ViewAnswers", "Question", new { id = question.Id });
            }

            model.Categories = db.Categories.OrderBy(c => c.Name).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = db.Questions.Include(a => a.Author).FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            var model = new QuestionViewModel();
            model.Author = question.Author;

            if (!IsUserAutorizedToEdit(model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.CategoryId = question.CategoryId;
            model.Categories = db.Categories.OrderBy(c => c.Name).ToList();

            model.Tags = string.Join(", ", question.Tags.Select(t => t.Name));

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
                question.CategoryId = model.CategoryId;
                this.SetQuestionTags(question, model, db);

                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ViewAnswers", "Question", new { id = question.Id });
            }

            model.Categories = db.Categories.OrderBy(c => c.Name).ToList();
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
                .Include(q => q.Category)
                .FirstOrDefault();

            if (question == null)
            {
                return HttpNotFound();
            }

            ViewBag.TagsString = string.Join(", ", question.Tags.Select(t => t.Name));

            var model = new QuestionViewModel();
            model.Author = question.Author;

            if (!IsUserAutorizedToEdit(model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.CategoryId = question.CategoryId;
            model.Category = question.Category;

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

            var questionAuthor = question.Author;
            var answers = question.Answers.ToList();

            foreach (var answer in answers)
            {
                answer.Author.PostsCount--;
                db.Answers.Remove(answer);
            }

            questionAuthor.PostsCount--;
            db.Entry(questionAuthor).State = EntityState.Modified;
            db.Questions.Remove(question);
            db.SaveChanges();

            return RedirectToAction("ListQuestionsByCategory", "Home", new { categoryId = question.CategoryId });
        }

        private bool IsUserAutorizedToEdit(QuestionViewModel questionViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = questionViewModel.IsAuthor(this.User.Identity.Name, questionViewModel.Author.UserName);

            return isAdmin || isAuthor;
        }

        private void SetQuestionTags(Question question, QuestionViewModel model, ForumDBContext db)
        {
            var tagsString = model.Tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToLower()).Distinct();
            question.Tags.Clear();

            foreach (var tagString in tagsString)
            {
                Tag tag = db.Tags.FirstOrDefault(t => t.Name.Equals(tagString));

                if (tag == null)
                {
                    tag = new Tag() { Name = tagString };
                    db.Tags.Add(tag);
                }

                question.Tags.Add(tag);
            }
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