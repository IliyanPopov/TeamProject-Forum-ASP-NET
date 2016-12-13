using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamProject_Forum_ASP_NET.ViewModels
{
    public class RankUserViewModel
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int PostsCount { get; set; }
    }
}