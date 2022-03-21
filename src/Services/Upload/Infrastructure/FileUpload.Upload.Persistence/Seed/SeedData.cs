using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileUpload.Upload.Persistence
{
    public static class Seed
    {
        public static void AddData(ModelBuilder builder)
        {
            var AdminRoleId = Guid.NewGuid();
            var AdminUserId = Guid.NewGuid();

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN" });

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = AdminUserId,
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Password123.,"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );

            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole()
                {
                    RoleId = AdminRoleId,
                    UserId = AdminUserId
                }
            );


            var userRoleId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = userRoleId, Name = "User", NormalizedName = "USER" });

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = userId,
                    Email = "user@gmail.com",
                    UserName = "user",
                    NormalizedUserName = "USER",
                    NormalizedEmail = "USER@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Password123.,"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );

            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole()
                {
                    RoleId = userRoleId,
                    UserId = userId
                }
            );

            builder.Entity<UserInfo>().HasData(

               new UserInfo()
               {
                   Id = Guid.NewGuid(),
                   UsedSpace = 0,
                   ApplicationUserId = AdminUserId,
               },
                new UserInfo()
                {
                    Id = Guid.NewGuid(),
                    UsedSpace = 0,
                    ApplicationUserId = userId,
                });

            builder.Entity<Category>().HasData(
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = AdminUserId,
                    Title = "Ödevler",
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = AdminUserId,
                    Title = "Tasarımlar",
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = AdminUserId,
                    Title = "Dosyalar",
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = userId,
                    Title = "Ödevler",
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = userId,
                    Title = "Tasarımlar",
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = userId,
                    Title = "Dosyalar",
                }
            );

        }
    }
}
