using FileUpload.Upload.Application.Interfaces.Context;
using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Persistence.Context;
using FileUpload.Upload.Persistence.Identity;
using FileUpload.Upload.Persistence.Repositories;
using FileUpload.Upload.Persistence.Repositories.Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace FileUpload.Upload.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.EnableSensitiveDataLogging(true);
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FileUpload.Upload.Persistence");

                });
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();


            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();



        }
    }
}
