// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.



using FileUpload.Domain.Entities;
using FileUpload.Persistence.Context;
using FileUpload.Persistence.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace FileUpload.IdentityServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    var applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

                    try
                    {
                        applicationDbContext.Database.EnsureCreated();
                        applicationDbContext.Database.Migrate();
                    }

                    catch
                    {
                        Log.Warning("Migration kısmında hata meydana geldi");
                    }


                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                    if (!roleManager.Roles.Any())
                    {
                        roleManager.CreateAsync(new ApplicationRole { Name = "Admin" }).Wait();
                        roleManager.CreateAsync(new ApplicationRole { Name = "User" }).Wait();
                    }

                    if (!userManager.Users.Any())
                    {
                        ApplicationUser userAdmin = new()
                        {
                            UserName = "admin@gmail.com",
                            Email = "admin@gmail.com",
                        };

                        userManager.CreateAsync(userAdmin, "Password123.,").Wait();
                        userManager.AddToRoleAsync(userAdmin, "Admin").Wait();

                        applicationDbContext.UserInfo.Add(new UserInfo() { ApplicationUserId = userAdmin.Id });
                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = userAdmin.Id, Title = "Ödevler" });
                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = userAdmin.Id, Title = "Tasarımlar" });
                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = userAdmin.Id, Title = "Dosyalar" });


                        ApplicationUser user = new()
                        {
                            UserName = "user@gmail.com",
                            Email = "user@gmail.com",
                        };

                        userManager.CreateAsync(user, "Password123.,").Wait();
                        userManager.AddToRoleAsync(user, "User").Wait();

                        applicationDbContext.UserInfo.Add(new UserInfo() { ApplicationUserId = user.Id });

                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = user.Id, Title = "Ödevler" });
                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = user.Id, Title = "Tasarımlar" });
                        applicationDbContext.Categories.Add(new Category() { ApplicationUserId = user.Id, Title = "Dosyalar" });

                        applicationDbContext.SaveChanges();
                    }


                }

                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}