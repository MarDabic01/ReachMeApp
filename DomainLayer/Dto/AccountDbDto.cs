using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class AccountDbDto
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "Username cannot contain spaces")]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password needs to be at least 8 characters long")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{0,}$",
         ErrorMessage = "Password needs at least one lowercase, one uppercase, one number and one special character")]
        public string Password { get; set; }
        public string ProfileBio { get; set; }
        public string ProfilePic { get; set; }
    }
}
