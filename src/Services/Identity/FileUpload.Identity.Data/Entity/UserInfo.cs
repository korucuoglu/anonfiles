using FileUpload.Data.Entity.Base;

namespace FileUpload.Data.Entity
{
    public class UserInfo: BaseIdentity
    {
        public long UsedSpace { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
