using FileUpload.Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace FileUpload.Data.Entity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser<int>
    {
        public UserInfo UserInfo { get; set; }
    }
}
