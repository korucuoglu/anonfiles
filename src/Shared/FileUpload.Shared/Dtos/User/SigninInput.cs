using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Dtos.User
{
    public class SigninInput
    {
        [Required]
        [Display(Name = "Kullanıcı Adınız")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

    }
}
