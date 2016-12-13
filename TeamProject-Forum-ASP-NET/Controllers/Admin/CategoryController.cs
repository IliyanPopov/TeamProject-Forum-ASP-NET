using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = db.Categories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteProcess(int? id)
        {
            var category = db.Categories
                .Include(c=>c.Questions)
                .FirstOrDefault(c => c.Id == id);

            //to delete the articles that contain
           // var categoryQuestions = category.Questions.Include(c=>c.).ToList();
            //var categoryQuestionsAnswers = category.Questions.ans

            db.Categories.Remove(category);
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