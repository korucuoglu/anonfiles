using System.Collections.Generic;

namespace FileUpload.Data.Entity
{
    public class File : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
