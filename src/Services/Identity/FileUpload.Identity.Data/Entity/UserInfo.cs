
using FileUpload.Data.Entity;
using System;

namespace FileUpload.Identity.Data.Entity
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public long UsedSpace { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
