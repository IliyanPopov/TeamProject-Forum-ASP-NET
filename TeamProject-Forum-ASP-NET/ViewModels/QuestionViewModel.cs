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

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Answer Answer { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public DateTime DateAdded { get; set; }
    }
}