using FileUpload.Domain.Common;
using System.Collections.Generic;

namespace FileUpload.Domain.Entities
{
    public class File : BaseIdentity
    {

        public File()
        {
            Files_Categories = new List<FileCategory>();
        }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }

        public virtual ICollection<FileCategory> Files_Categories { get; set; }
        
        


    }
}
