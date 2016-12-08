﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.Entities
{

    public class ForumDBContext : IdentityDbContext<ApplicationUser>
    {
        public ForumDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public string NameTest { get; set; }

        public static ForumDBContext Create()
        {
            return new ForumDBContext();
        }
    }

}