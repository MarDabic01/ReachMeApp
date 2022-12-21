using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]*$",ErrorMessage = "Username cannot contain spaces")]
        public string Username { get; set; }
        [Required]
        [MinLength(8,ErrorMessage = "Password needs to be at least 8 characters long")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{0,}$", 
         ErrorMessage = "Password needs at least one lowercase, one uppercase, one number and one special character")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not matching")]
        [DisplayName("Repeat password")]
        public string RepeatPassword { get; set; }
    }
}
