using FileUpload.Domain.Common;
using System.Collections.Generic;

namespace FileUpload.Domain.Entities
{
    public class Category: BaseIdentity
    {
       
        public string Title { get; set; }
        public virtual ICollection<FileCategory> Files_Categories { get; set; }




    }
}
