using System.ComponentModel.DataAnnotations;

namespace AnonFilesUpload.MVC.Models.User
{
    public class SignupInput
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Şehir")]
        public string City { get; set; }
    }
}
