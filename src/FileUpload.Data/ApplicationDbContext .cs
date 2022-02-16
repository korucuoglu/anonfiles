using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Data.Entity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<File_Category> Files_Categories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<File_Category>()
                .HasOne(x => x.File)
                .WithMany(x => x.File_Category)
                .HasForeignKey(x => x.FileId);

            builder.Entity<File_Category>()
              .HasOne(x => x.Category)
              .WithMany(x => x.File_Category)
              .HasForeignKey(x => x.CategoryId);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}