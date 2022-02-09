﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FileUpload.Data.Entity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public long UsedSpace { get; set; }

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
