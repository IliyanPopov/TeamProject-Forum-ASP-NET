using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class QuestionController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        // GET: Question
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Author);
            return View(questions.ToList());
        }

        // GET: Question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Question/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "Email");
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,DateAdded,AuthorId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "Email", question.AuthorId);
            return View(question);
        }

        // GET: Question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "Email", question.AuthorId);
            return View(question);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,DateAdded,AuthorId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "Email", question.AuthorId);
            return View(question);
        }

        // GET: Question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
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
