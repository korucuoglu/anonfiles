using AnonFilesUpload.MVC.Handler;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services;
using AnonFilesUpload.MVC.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AnonFilesUpload.MVC.Extensions
{
    public static class ServiceExtension
    {

        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            services.AddScoped<ClientCredentialTokenHandler>();

            // services.AddScoped(typeof(IApiService), typeof(ApiService));

            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IApiService, ApiService>(opt =>

            {
                opt.BaseAddress = new Uri(serviceApiSettings.ApiBaseUri);
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            
        }
    }
}
