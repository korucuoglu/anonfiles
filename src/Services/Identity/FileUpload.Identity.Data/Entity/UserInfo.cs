
using FileUpload.Data.Entity;
using System;

namespace FileUpload.Identity.Data.Entity
{
    public class UserInfo
    {
        public int Id { get; set; }
        public long UsedSpace { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
