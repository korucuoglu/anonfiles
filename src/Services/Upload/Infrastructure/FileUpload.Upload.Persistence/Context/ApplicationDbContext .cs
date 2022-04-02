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

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).UseIdentityColumn().UseSerialColumn();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.Id).UseIdentityColumn().UseSerialColumn();
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