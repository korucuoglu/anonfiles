using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
