using System.ComponentModel.DataAnnotations;

namespace FileUpload.Shared.Dtos.User
{
    public class ConfirmEmailModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }

    }
}
