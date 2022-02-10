using FileUpload.Data.Entity.Base;
using System.Collections.Generic;

namespace FileUpload.Data.Entity
{
    public class Category: BaseIdentity
    {
       
        public string Title { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}
