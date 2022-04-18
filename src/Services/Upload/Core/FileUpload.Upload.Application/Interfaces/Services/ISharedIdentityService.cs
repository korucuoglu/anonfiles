namespace FileUpload.Upload.Application.Interfaces.Services
{
    public interface ISharedIdentityService
    {
        public int GetUserId { get; }
        public string GetMail { get; }
        public string GetUserName { get; }
    }


}
