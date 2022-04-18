using FileUpload.Upload.Application.Interfaces.Repositories.File;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories.File
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
