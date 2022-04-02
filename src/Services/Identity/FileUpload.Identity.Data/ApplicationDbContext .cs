using FileUpload.Identity.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileUpload.Data.Entity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {

        public DbSet<UserInfo> UserInfo { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).UseIdentityColumn().UseSerialColumn();
            });


            builder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.Id).UseIdentityColumn().UseSerialColumn();
            });

            builder.Entity<UserInfo>(b =>
            {
                b.Property(u => u.Id).UseIdentityColumn().UseSerialColumn();
                b.Property(x => x.UsedSpace).HasDefaultValue(0);
            });


            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}