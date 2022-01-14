using AnonFilesUpload.Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace AnonFilesUpload.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();


            using (var scope = host.Services.CreateScope())
            {

                //var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                ////context.Database.EnsureCreated();
                ////context.Database.Migrate();


                //if(!context.Data.Any())
                //{
                //    context.Data.AddRange(
                        
                //        new Data.Entity.Data() {MetaDataId = "d8G2ffBfx7", Name = "halo.cfg.txt", ShortUri= "https://anonfiles.com/d8G2ffBfx7" },    
                //        new Data.Entity.Data() {MetaDataId = "z2F1f1Bbxf", Name = "halo.cfg.txt", ShortUri= "https://anonfiles.com/z2F1f1Bbxf" },

                //        new Data.Entity.Data() { MetaDataId = "n242ffB1x9", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/d8G2ffBfx7" },
                //        new Data.Entity.Data() { MetaDataId = "h83df0Bexb", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/d8G2ffBfx7" },


                //        new Data.Entity.Data() { MetaDataId = "1eV0f1B3x8", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/1eV0f1B3x8" },
                //        new Data.Entity.Data() { MetaDataId = "TaY1f1B1x4", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/TaY1f1B1x4" },
                //        new Data.Entity.Data() { MetaDataId = "50e0g3Bfxc", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/50e0g3Bfxc" },
                //        new Data.Entity.Data() { MetaDataId = "J3e6gfBax8", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/J3e6gfBax8" },
                //        new Data.Entity.Data() { MetaDataId = "D5zbj4B9x1", Name = "PDF", ShortUri = "https://anonfiles.com/D5zbj4B9x1" },
                //        new Data.Entity.Data() { MetaDataId = "15K3jcBbx3", Name = "PDF", ShortUri = "https://anonfiles.com/15K3jcBbx3"}

                //    );
                //}

                //context.SaveChanges();

            }

                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
