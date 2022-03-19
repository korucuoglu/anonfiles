using FileUpload.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Context
{
    public interface IApplicationDbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileCategory> FilesCategories { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        Task<int> SaveChanges();
    }
}
