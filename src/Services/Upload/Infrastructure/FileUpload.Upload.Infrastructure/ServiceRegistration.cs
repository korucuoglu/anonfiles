
using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Filters;
using FileUpload.Upload.Infrastructure.Services.Redis;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Infrastructure.Attribute;
using FileUpload.Upload.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using FileUpload.Shared.Services;

namespace FileUpload.Upload.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped(typeof(NotFoundFilterAttribute<>));

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

            #region Redis
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

            services.AddSingleton<IRedisSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            });

            services.AddSingleton<IRedisService, RedisService>();

            #endregion

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #region SignalR & Cors
          
            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 102400000;
                e.EnableDetailedErrors = true;
            });
           
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(configuration.GetSection("ServiceApiSettings").GetSection("MVCClient").Value).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            #endregion
        }
    }
}
