namespace FileUpload.Shared.Dtos.User
{
    public class ResetPasswordConfirmModel
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; } = "";
    }
}
