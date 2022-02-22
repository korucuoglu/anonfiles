using FileUpload.MVC.Application.Dtos.Settings;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.MVC.Infrastructure.Handler;
using FileUpload.MVC.Infrastructure.Services;
using FileUpload.MVC.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileUpload.MVC.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration Configuration)
        {

            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            services.AddScoped<ClientCredentialTokenHandler>();
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();



            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
            services.AddHttpClient<IIdentityService, IdentityService>();

            services.AddScoped<IUserService, UserService>();


            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IApiService, ApiService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.ApiBaseUri);


            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        }
    }
}
