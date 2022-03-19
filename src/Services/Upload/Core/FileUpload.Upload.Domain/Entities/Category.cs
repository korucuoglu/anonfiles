using FileUpload.Upload.Domain.Common;
using System.Collections.Generic;

namespace FileUpload.Upload.Domain.Entities
{
    public class Category: BaseIdentity
    {
       
        public string Title { get; set; }
        public virtual ICollection<FileCategory> FilesCategories { get; set; }
    }
}
