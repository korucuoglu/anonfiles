
using FileUpload.Upload.Domain.Common;

namespace FileUpload.Upload.Domain.Entities
{
    public class UserInfo : BaseEntity
    {
        public long UsedSpace { get; set; }
    }
}
