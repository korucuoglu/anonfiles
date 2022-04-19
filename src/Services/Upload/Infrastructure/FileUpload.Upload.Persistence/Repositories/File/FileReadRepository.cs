using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        private readonly ApplicationDbContext _context;
        public FileReadRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetFileKey(int fileId, int userId)
        {
            return await _context.Files.
                Where(x => x.Id == fileId && x.UserId == userId).
                AsNoTracking().Select(x => x.FileKey).
                FirstOrDefaultAsync();


        }
    }
}
