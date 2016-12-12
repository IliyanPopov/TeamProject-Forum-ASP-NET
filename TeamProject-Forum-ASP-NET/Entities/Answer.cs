using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.Entities
{
    public class Answer
    {
        public Answer()
        {
            this.DateAdded = DateTime.Now;
        }

        public Answer(string authorId, string content, int questionId)
        {
            this.AuthorId = authorId;
            this.Content = content;
            this.QuestionId = questionId;
            this.DateAdded = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DateAdded { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}