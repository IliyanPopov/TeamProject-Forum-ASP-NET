using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamProject_Forum_ASP_NET.Entities;
using TeamProject_Forum_ASP_NET.Models;

namespace TeamProject_Forum_ASP_NET.ViewModels
{
    public class EditUserViewModel
    {

        public ApplicationUser User { get; set; }

        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password",ErrorMessage = "Passwords doesn't match")]
        public string ConfirmPassword { get; set; }

        public IList<Role> Roles { get; set; }
    }
}