using AnonFilesUpload.Api.Hubs;
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AnonFilesUpload.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddHttpClient<FileService>();

            services.AddDbContext<DataContext>(opt =>
            {

                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
                {

                    configure.MigrationsAssembly("AnonFilesUpload.Api");

                });

            });


            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });


            services.AddCors(opt =>
            {

                opt.AddPolicy("CorsPolicy", builder =>
                {

                    builder.WithOrigins("https://localhost:44361", "http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HubTestApi>("/hub");
            });
        }
    }
}
