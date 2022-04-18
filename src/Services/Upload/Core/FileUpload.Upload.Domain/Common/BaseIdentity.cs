namespace FileUpload.Upload.Domain.Common
{
    public abstract class BaseIdentity : BaseEntity
    {
        public int UserId { get; set; }
    }
}
