using FileUpload.Upload.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FileUpload.Upload.Persistence.Identity
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
