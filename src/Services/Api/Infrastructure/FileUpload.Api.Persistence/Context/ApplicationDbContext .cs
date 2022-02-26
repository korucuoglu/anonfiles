using FileUpload.Application.Interfaces.Context;
using FileUpload.Domain.Entities;
using FileUpload.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileUpload.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
    {

        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileCategory> Files_Categories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.HasPostgresExtension("uuid-ossp");

            builder.Entity<FileCategory>(entity =>
            {
                entity.HasKey(table => new {
                    table.CategoryId,
                    table.FileId
                });

                entity.HasOne(x => x.File)
              .WithMany(x => x.Files_Categories)
              .HasForeignKey(x => x.FileId);

                entity.HasOne(x => x.Category)
              .WithMany(x => x.Files_Categories)
              .HasForeignKey(x => x.CategoryId);

            });

          
              

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("uuid_generate_v4()").IsRequired();
            });


            builder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()").IsRequired();
            });

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