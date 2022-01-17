using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnonFilesUpload.Data.Entity
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Data> Data { get; set; }

        public DataContext()
        {

        }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=anonfiles;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }

            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
