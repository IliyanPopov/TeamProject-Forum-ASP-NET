using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int ViewCount { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Answer Answer { get; set; }

        public IPagedList<Answer> Answers { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<Category> Categories { get; set; }

        public string Tags { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsAuthor(string authorName, string postAuthor)
        {
            return authorName.Equals(postAuthor);
        }
    }
}