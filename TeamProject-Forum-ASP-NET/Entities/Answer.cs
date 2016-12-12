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
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}