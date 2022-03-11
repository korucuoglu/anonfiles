using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Dtos.User
{
    public class ResetPasswordModel
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

    }
}
