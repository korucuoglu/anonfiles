using FileUpload.Upload.Application.Interfaces.Context;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileCategory> FilesCategories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<FileCategory>(entity =>
            {

                entity.Property(x => x.CategoryId).HasColumnName("category_id");
                entity.Property(x => x.FileId).HasColumnName("file_id");

                entity.ToTable("file_category");
                entity.HasKey(table => new
                {
                    table.CategoryId,
                    table.FileId
                });

                entity.HasOne(x => x.File)
              .WithMany(x => x.FilesCategories)
              .HasForeignKey(x => x.FileId);

                entity.HasOne(x => x.Category)
              .WithMany(x => x.FilesCategories)
              .HasForeignKey(x => x.CategoryId);


            });

            builder.Entity<File>(entity =>
            {
                entity.ToTable("files");

                entity.Property(x => x.FileKey).HasColumnName("filekey");
                entity.Property(x => x.FileName).HasColumnName("filename");
                entity.Property(x => x.ApplicationUserId).HasColumnName("user_id");
                entity.Property(x => x.Size).HasColumnName("size");
                entity.Property(x => x.Extension).HasColumnName("extension");
                entity.Property(x => x.CreatedDate).HasColumnName("created_date");
                entity.Property(x => x.Id).HasColumnName("id");
            });

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.CreatedDate).HasColumnName("created_date");
                entity.Property(x => x.ApplicationUserId).HasColumnName("user_id");
                entity.Property(x => x.Title).HasColumnName("title");
            });

            builder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("user_info");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.CreatedDate).HasColumnName("created_date");
                entity.Property(x => x.ApplicationUserId).HasColumnName("user_id");
                entity.Property(x => x.UsedSpace).HasColumnName("used_space");
            });



            Seed.AddData(builder);
            

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        async Task<int> IApplicationDbContext.SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}