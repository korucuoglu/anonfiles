using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileUpload.Data.Entity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {

        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileCategory> Files_Categories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("uuid-ossp");



            builder.Entity<FileCategory>()
                .HasOne(x => x.File)
                .WithMany(x => x.File_Category)
                .HasForeignKey(x => x.FileId);

            builder.Entity<FileCategory>()
              .HasOne(x => x.Category)
              .WithMany(x => x.File_Category)
              .HasForeignKey(x => x.CategoryId);

            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("uuid_generate_v4()").IsRequired();
            });


            builder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}