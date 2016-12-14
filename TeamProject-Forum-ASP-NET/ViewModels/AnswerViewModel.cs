using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.ViewModels
{
    public class AnswerViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
        
    }
}