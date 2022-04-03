using FileUpload.Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileUpload.Data.Entity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {

        public ApplicationUser()
        {
            UserInfo = new()
            {
                UsedSpace = 0
            };
        }
        public UserInfo UserInfo { get; set; }
    }
}
