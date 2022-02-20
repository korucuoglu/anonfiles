
using FileUpload.Application.Interfaces.Services;
using FileUpload.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileUpload.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ISharedIdentityService, SharedIdentityService>();
        }
    }
}
