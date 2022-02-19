using FileUpload.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FileUpload.Persistence.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
