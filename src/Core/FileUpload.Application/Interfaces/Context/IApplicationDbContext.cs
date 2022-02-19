using FileUpload.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Application.Interfaces.Context
{
    public interface IApplicationDbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileCategory> Files_Categories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
