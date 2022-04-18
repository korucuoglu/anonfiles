
using FileUpload.Upload.Domain.Common;

namespace FileUpload.Upload.Domain.Entities
{
    public class UserInfo : BaseIdentity
    {
        public long UsedSpace { get; set; }
    }
}
