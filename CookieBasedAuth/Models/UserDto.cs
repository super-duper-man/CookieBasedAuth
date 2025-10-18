using System.ComponentModel.DataAnnotations;

namespace CookieBasedAuth.Models
{
    public class UserDto
    {
        public string? FullName
        {
            get; set;
        }
        [Required]
        public string Email
        {
            get; set;
        }
        [Required]
        public string Password
        {
            get; set;
        }
    }
}
