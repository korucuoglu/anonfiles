
using FileUpload.Api.Domain.Common;

namespace FileUpload.Api.Domain.Entities
{
    public class UserInfo: BaseIdentity
    {
        public long UsedSpace { get; set; }
        // public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
