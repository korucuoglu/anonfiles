using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Persistence.Context;
using FileUpload.Upload.Persistence.Identity;
using FileUpload.Upload.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUpload.Upload.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.EnableSensitiveDataLogging(true);
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FileUpload.Upload.Persistence");

                });
            });

            services.AddIdentity<User, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();


            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IUserInfoReadRepository, UserInfoReadRepository>();
            services.AddScoped<IUserInfoWriteRepository, UserInfoWriteRepository>();

            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
