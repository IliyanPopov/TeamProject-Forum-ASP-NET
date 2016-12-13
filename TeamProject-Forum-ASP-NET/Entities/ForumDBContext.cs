using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using TeamProject_Forum_ASP_NET.Models;
using System.Data.Entity;

namespace TeamProject_Forum_ASP_NET.Entities
{
    public class ForumDBContext : IdentityDbContext<ApplicationUser>
    {
        public ForumDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Question> Questions { get; set; }

        public virtual IDbSet<Answer> Answers { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public static ForumDBContext Create()
        {
            return new ForumDBContext();
        }
    }

}