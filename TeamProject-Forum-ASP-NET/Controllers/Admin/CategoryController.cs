using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.Controllers.Admin
{
    public class CategoryController : Controller
    {
        private ForumDBContext db = new ForumDBContext();

        // GET: Category
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