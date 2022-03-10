using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Dtos.User
{
    public class SignupInput
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        
    }
}
