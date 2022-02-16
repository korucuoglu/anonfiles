using FileUpload.Data.Entity.Base;
using System.Collections.Generic;

namespace FileUpload.Data.Entity
{
    public class File : BaseIdentity
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<File_Category> File_Category { get; set; }
    }
}
