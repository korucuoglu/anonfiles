using FileUpload.Api.Domain.Common;
using System.Collections.Generic;

namespace FileUpload.Api.Domain.Entities
{
    public class Category: BaseIdentity
    {
       
        public string Title { get; set; }
        public virtual ICollection<FileCategory> FilesCategories { get; set; }




    }
}
