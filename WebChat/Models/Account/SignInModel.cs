using System.ComponentModel.DataAnnotations;

namespace WebChat.Models.Account
{
    public class SignInModel
    {
        [Required]
        [Display(Name = "Enter your email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter your password")]
        public string Password { get; set; }
    }
}