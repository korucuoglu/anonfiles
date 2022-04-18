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
            var AdminRoleId = 1;
            var AdminUserId = 1;

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN" });

            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(
                new User
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


            var userRoleId = 2;
            var userId = 2;

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = userRoleId, Name = "User", NormalizedName = "USER" });

            builder.Entity<User>().HasData(
                new User
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
                   Id = 1,
                   UsedSpace = 0,
                   UserId = AdminUserId,
               },
                new UserInfo()
                {
                    Id = 2,
                    UsedSpace = 0,
                    UserId = userId,
                });

            builder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    UserId = AdminUserId,
                    Title = "Ödevler",
                },
                new Category()
                {
                    Id = 2,
                    UserId = AdminUserId,
                    Title = "Tasarımlar",
                },
                new Category()
                {
                    Id = 3,
                    UserId = AdminUserId,
                    Title = "Dosyalar",
                },
                new Category()
                {
                    Id = 4,
                    UserId = userId,
                    Title = "Ödevler",
                },
                new Category()
                {
                    Id = 5,
                    UserId = userId,
                    Title = "Tasarımlar",
                },
                new Category()
                {
                    Id = 6,
                    UserId = userId,
                    Title = "Dosyalar",
                }
            );

        }
    }
}
