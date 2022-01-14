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

                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                //context.Database.EnsureCreated();
                //context.Database.Migrate();


                if(!context.Data.Any())
                {
                    context.Data.AddRange(
                        
                        new Data.Entity.Data() {MetaDataId = "d8G2ffBfx7", Name = "halo.cfg.txt", ShortUri= "https://anonfiles.com/d8G2ffBfx7", DirectLink= "https://cdn-133.anonfiles.com/d8G2ffBfx7/ca2a441c-1642092957/halo.cfg.txt" },    
                        new Data.Entity.Data() {MetaDataId = "z2F1f1Bbxf", Name = "halo.cfg.txt", ShortUri= "https://anonfiles.com/z2F1f1Bbxf", DirectLink= "https://cdn-127.anonfiles.com/z2F1f1Bbxf/93112eca-1642093348/halo.cfg.txt" },

                        new Data.Entity.Data() { MetaDataId = "n242ffB1x9", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/d8G2ffBfx7", DirectLink = "https://cdn-131.anonfiles.com/n242ffB1x9/317339b5-1642093374/halo.cfg.txt" },
                        new Data.Entity.Data() { MetaDataId = "h83df0Bexb", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/d8G2ffBfx7", DirectLink = "https://cdn-130.anonfiles.com/h83df0Bexb/bc4ec432-1642093400/halo.cfg.txt" },


                        new Data.Entity.Data() { MetaDataId = "1eV0f1B3x8", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/1eV0f1B3x8", DirectLink = "https://cdn-107.anonfiles.com/1eV0f1B3x8/f917af2b-1642093903/halo.cfg.txt" },
                        new Data.Entity.Data() { MetaDataId = "TaY1f1B1x4", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/TaY1f1B1x4", DirectLink = "https://cdn-102.anonfiles.com/TaY1f1B1x4/fa2ba4a1-1642093906/halo.cfg.txt" },
                        new Data.Entity.Data() { MetaDataId = "50e0g3Bfxc", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/50e0g3Bfxc", DirectLink = "https://cdn-149.anonfiles.com/50e0g3Bfxc/24b2b0e3-1642093914/halo.cfg.txt" },
                        new Data.Entity.Data() { MetaDataId = "J3e6gfBax8", Name = "halo.cfg.txt", ShortUri = "https://anonfiles.com/J3e6gfBax8", DirectLink = "https://cdn-131.anonfiles.com/J3e6gfBax8/ddd1df91-1642093925/halo.cfg.txt" },
                        new Data.Entity.Data() { MetaDataId = "D5zbj4B9x1", Name = "PDF", ShortUri = "https://anonfiles.com/D5zbj4B9x1", DirectLink = "https://cdn-103.anonfiles.com/D5zbj4B9x1/8953608e-1642116765/=?utf-8?B?MjAxOCBLUFNTIEXEn2l0aW0gQmlsaW1sZXJpIEtvbnUgS29udSBEw7x6ZW5sZW5tacWfIFRhbWFtxLEgw4fDtnrDvG1sw7wgMjAwOC0yMDE3IMOHxLFrbcSxxZ8gU29ydWxhci5wZGY=?=" },
                        new Data.Entity.Data() { MetaDataId = "15K3jcBbx3", Name = "PDF", ShortUri = "https://anonfiles.com/15K3jcBbx3", DirectLink = "https://cdn-106.anonfiles.com/15K3jcBbx3/906cd2c3-1642120533/Tarih.pdf" }

                    );
                }

                context.SaveChanges();

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
