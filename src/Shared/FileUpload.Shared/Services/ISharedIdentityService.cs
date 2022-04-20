namespace FileUpload.Shared.Services
{
    public interface ISharedIdentityService
    {
        public int GetUserId { get; }
        public string GetMail { get; }
        public string GetUserName { get; }
    }


}
