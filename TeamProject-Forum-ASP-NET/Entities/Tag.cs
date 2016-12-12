using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamProject_Forum_ASP_NET.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}