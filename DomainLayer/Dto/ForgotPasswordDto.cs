using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
