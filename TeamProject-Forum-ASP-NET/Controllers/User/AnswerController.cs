using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.Controllers.User
{
    public class AnswerController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        // GET: Answer
        public ActionResult Index()
        {
            return View();
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