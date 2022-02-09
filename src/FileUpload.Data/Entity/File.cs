using System.Collections.Generic;

namespace FileUpload.Data.Entity
{
    public class File : BaseEntity
    {
        public string UserId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
