using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FileWriteRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddFileWithSp(string fileName, long size, string fileKey, int userId)
        {

            try
            {

                var data = await _context.Database.ExecuteSqlRawAsync("call add_file ({0}, {1}, {2}, {3}, {4})",
                fileName, size, fileKey, userId, null);

                return 1;

            }

            catch
            {
                return 0;
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
