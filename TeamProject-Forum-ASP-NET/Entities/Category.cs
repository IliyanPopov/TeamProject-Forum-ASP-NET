using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeamProject_Forum_ASP_NET.Entities
{
    public class Category
    {
        private ICollection<Question> questions;

        public Category()
        {
            this.questions = new HashSet<Question>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}