﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }
        public string ProfileBio { get; set; }
        public string ProfilePic { get; set; }
        public List<Post> Posts { get; set; }
        public List<Follow> Follows { get; set; }
    }
}
