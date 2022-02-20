
using FileUpload.Application.Features.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FileUpload.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assm);

            services.AddMediatR(assm);
            // services.AddMediatR(typeof(GetAllCategoriesByUserIdQueryHandler).Assembly);



        }
    }
}
