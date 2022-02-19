using FileUpload.Api.Filters;
using FileUpload.Api.Hubs;
using FileUpload.Api.Services;
using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FileUpload.Api
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
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetSection("ServiceApiSettings").GetSection("IdentityBaseUri").Value;
                options.Audience = "resource_api_password";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };

                // Aud parametresinden Identity Server, hangi akış tipinde oldu�unu anlar ve ona göre davranış sergiler. 
            });


            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.EnableSensitiveDataLogging(true);
                opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FileUpload.Data");

                });

            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // Api içerisinden UserId değelerini okuyabilmek için bunu ekledik. 


            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

      

            services.AddHttpContextAccessor();
            services.AddScoped<ILogger, ConsoleLogger>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<MinIOService>();
            services.AddScoped<CategoriesService>();
            services.AddScoped(typeof(NotFoundFilter<>));
            services.AddScoped(typeof(ValidationFilterAttribute<>));

            services.AddAutoMapper(typeof(Startup));

            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 102400000;
                e.EnableDetailedErrors = true;
            });

            services.AddSingleton<ISharedIdentityService, SharedIdentityService>();


            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("ServiceApiSettings").GetSection("MVCClient").Value).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).
            AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            }).
            AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<FileHub>("/fileHub");

        });
        }
    }
}
