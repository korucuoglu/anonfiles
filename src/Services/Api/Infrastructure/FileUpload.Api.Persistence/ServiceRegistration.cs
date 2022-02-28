using FileUpload.Api.Application.Interfaces.Settings;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Persistence.Settings;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FileUpload.Api.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = configuration.GetSection("ServiceApiSettings").GetSection("IdentityBaseUri").Value;
                options.Audience = "resource_api_password";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };

            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
        }
    }
}
