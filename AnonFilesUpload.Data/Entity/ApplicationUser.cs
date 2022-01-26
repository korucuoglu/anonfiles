using Microsoft.AspNetCore.Identity;

namespace AnonFilesUpload.Data.Entity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
