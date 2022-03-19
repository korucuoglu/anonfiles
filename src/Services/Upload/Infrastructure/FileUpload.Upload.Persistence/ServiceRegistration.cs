using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Repositories;
using FileUpload.Upload.Application.Interfaces.Context;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Persistence.Context;
using FileUpload.Upload.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = configuration.GetSection("ServiceApiSettings").GetSection("IdentityBaseUri").Value;
                options.Audience = "resource_upload";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };

            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        }
    }
}
