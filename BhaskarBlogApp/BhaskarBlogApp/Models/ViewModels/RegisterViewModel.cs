using System.ComponentModel.DataAnnotations;

namespace BhaskarBlogApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6,ErrorMessage="Password has to be atleast 6 chars")]
        public string Password { get; set; }
    }
}
