using AnonFilesUpload.Api.Hubs;
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Registiration;
using AnonFilesUpload.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

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
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddHttpContextAccessor();

            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // Api içerisinden UserId deðerlerini okuyabilmek için bunu ekledik. 

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_api_password"; 
                options.RequireHttpsMetadata = false;

                // AUd parametresinden Identity Server, hangi akýþ tipinde olduðunu anlar ve ona göre davranýþ sergiler. 
            });

            services.AddSignalR();

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });



            services.AddHttpClient<FileService>();

            services.AddDataContext(Configuration);

            services.AddCors(opt =>
            {

                opt.AddPolicy("CorsPolicy", builder =>
                {

                    builder.WithOrigins(Configuration.GetSection("VueClient").Value).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });


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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HubTestApi>("/hub");
            });
        }
    }
}
