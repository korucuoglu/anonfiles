using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AnonFilesUpload.Data.Registiration
{
    public static class ServiceRegistration
    {

        public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), configure =>
                {

                    configure.MigrationsAssembly("AnonFilesUpload.Data");

                });

            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
        }
    }
}
