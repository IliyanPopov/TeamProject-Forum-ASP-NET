using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Entities;

namespace TeamProject_Forum_ASP_NET.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Search")]
        public string Query { get; set; }
        
        public bool Title { get; set; }

        public bool Content { get; set; }

        public bool Tag { get; set; }

        public IPagedList<Question> Questions { get; set; }
    }
}