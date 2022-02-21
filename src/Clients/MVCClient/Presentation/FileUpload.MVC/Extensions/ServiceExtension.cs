using FileUpload.MVC.Handler;
using FileUpload.MVC.Models;
using FileUpload.MVC.Services;
using FileUpload.MVC.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileUpload.MVC.Extensions
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
