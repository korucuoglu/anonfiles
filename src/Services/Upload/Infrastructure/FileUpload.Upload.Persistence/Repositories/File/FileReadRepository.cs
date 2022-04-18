using FileUpload.Upload.Application.Interfaces.Repositories.File;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories.File
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
