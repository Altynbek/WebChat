using System.ComponentModel.DataAnnotations;

namespace WebChat.Models.Account
{
    public class SignUpModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter your password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat your password")]
        [Compare("Password", ErrorMessage ="Entered passwords are not equal")]
        public string PasswordConfirm { get; set; }
    }
}