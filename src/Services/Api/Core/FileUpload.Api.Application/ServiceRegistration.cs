using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FileUpload.Api.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assm);

            services.AddMediatR(assm);



        }
    }
}
