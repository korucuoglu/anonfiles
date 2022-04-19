using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Persistence.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FileWriteRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AddFileWithSp(File entity)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("call add_file ({0}, {1}, {2}, {3})",
             entity.FileName, entity.Size, entity.FileKey, entity.UserId);
                return true;
            }

            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFileWithSp(int fileId, int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("call delete_file ({0}, {1})",
             fileId, userId);
                return true;
            }

            catch
            {
                return false;
            }
        }
    }
}
