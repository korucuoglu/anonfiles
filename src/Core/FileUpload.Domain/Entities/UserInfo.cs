
using FileUpload.Domain.Common;

namespace FileUpload.Domain.Entities
{
    public class UserInfo: BaseIdentity
    {
        public long UsedSpace { get; set; }
        // public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
