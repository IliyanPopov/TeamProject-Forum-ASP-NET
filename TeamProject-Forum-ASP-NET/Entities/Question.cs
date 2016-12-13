using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.Entities
{
    public class Question
    {
        //private ICollection<Tag> tags;
        private ICollection<Answer> answers;

        public Question()
        {
            this.DateAdded = DateTime.Now;
            this.answers = new HashSet<Answer>();
            // this.tags = new HashSet<Tag>();
        }

        public Question(string authorId, string title, string content)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Content = content;
            this.DateAdded = DateTime.Now;
            this.answers = new HashSet<Answer>();
            // this.tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int ViewCount { get; set; }

        public DateTime DateAdded { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Answer> Answers
        {
            get { return this.answers; }
            set { this.answers = value; }
        }

        //[ForeignKey("Category")]
        //public int CategoryId { get; set; }

        //public virtual Category Category { get; set; }

        //public virtual ICollection<Tag> Tags
        //{
        //    get { return this.tags; }
        //    set { this.tags = value; }
        //}

        public bool isAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}