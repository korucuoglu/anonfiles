using FileUpload.Api.Application.Interfaces.Services;
using FileUpload.Api.Infrastructure.Attribute;
using FileUpload.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUpload.Api.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileService, FileService>();
            // services.AddScoped(typeof(NotFoundFilterAttribute<>));
            services.AddScoped<ValidationFilterAttribute>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

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
        }
    }
}
