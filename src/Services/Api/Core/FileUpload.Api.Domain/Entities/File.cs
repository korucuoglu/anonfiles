using FileUpload.Domain.Common;
using System.Collections.Generic;

namespace FileUpload.Domain.Entities
{
    public class File : BaseIdentity
    {

        public File()
        {
            FilesCategories = new List<FileCategory>();
        }

        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }

        public virtual ICollection<FileCategory> FilesCategories { get; set; }
        
        


    }
}
